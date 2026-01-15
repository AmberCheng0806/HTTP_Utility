using CredentialManagement;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HTTP_Utility
{
    public class Token
    {
        private string RefreshToken;
        private string AccessToken;
        private DateTime ExpireTime;
        private string ClientId;
        private string ClientSecret;
        private HttpClient HttpClient = new HttpClient();
        private HttpUtility HttpUtility = new HttpUtility();

        public Token()
        {
            Credential credential = new Credential()
            {
                Target = "YoutubeCredential"
            };

            if (credential.Load())
            {
                string password = credential.Password;
                string[] passwords = password.Split('|');
                ClientId = passwords[0];
                ClientSecret = passwords[1];
                AccessToken = passwords[2];
                RefreshToken = passwords[3];
                //ExpireTime = passwords[4] == "expireTime" ? DateTime.Now : DateTime.Parse(passwords[2]);
                ExpireTime = new DateTime(2026, 1, 12, 12, 0, 0);
            }
        }

        public async Task<string> GetAccessToken()
        {
            if (string.IsNullOrEmpty(AccessToken) || AccessToken == "accessToken")
            {
                await GetTokenByCode();
            }
            if (DateTime.Now > ExpireTime)
            {
                await ExchangeAccessTokenByRefreshToken();
            }
            return AccessToken;
        }

        private async Task ExchangeAccessTokenByRefreshToken()
        {
            //var body = new Dictionary<string, string>
            //{
            //    { "client_id", ClientId },
            //    { "client_secret", ClientSecret },
            //    { "refresh_token", RefreshToken },
            //    { "grant_type", "refresh_token" }
            //};
            //var input = new FormUrlEncodedContent(body);

            var input = new
            {
                client_id = ClientId,
                client_secret = ClientSecret,
                refresh_token = RefreshToken,
                grant_type = "refresh_token"
            };
            AccessTokenModel tokenModel = await HttpUtility.PostAsync<AccessTokenModel>("https://oauth2.googleapis.com/token", input);
            AccessToken = tokenModel.access_token;
            //ExpireTime = DateTime.Now.AddSeconds(tokenModel.expires_in);

            Credential credential = new Credential();
            credential.Target = "YoutubeCredential";
            credential.Username = "chi";
            credential.Password = $"{ClientId}|{ClientSecret}|{tokenModel.access_token}|{RefreshToken}|{DateTime.Now.AddSeconds(tokenModel.expires_in)}";
            credential.Save();
        }

        private async Task<string> GetCode()
        {
            string url = "http://localhost:5000/";
            util.GetCodeVerifier();

            var body = new Dictionary<string, string>
            {
                {"scope","https%3A//www.googleapis.com/auth/drive.metadata.readonly%20https%3A//www.googleapis.com/auth/calendar.readonly%20email%20profile%20https%3A//www.googleapis.com/auth/youtube.upload%20https%3A//www.googleapis.com/auth/youtube%20https%3A//www.googleapis.com/auth/youtubepartner%20https%3A//www.googleapis.com/auth/youtube.force-ssl" },
                {"access_type","offline" },
                {"prompt","consent" },
                {"response_type","code" },
                {"state","123" },
                {"redirect_uri", "http://localhost:5000"},
                {"client_id", ClientId },
                {"code_challenge",util.GetCodeChallenge(util.CodeVerifier)},
                {"code_challenge_method",util.challengeMethod }
            };
            //await HttpUtility.GetAsync("https://accounts.google.com/o/oauth2/v2/auth", body);


            string fullUrl = "https://accounts.google.com/o/oauth2/v2/auth?" + string.Join("&", body.Select(x => $"{x.Key}={x.Value}"));

            Process.Start(new ProcessStartInfo
            {
                FileName = fullUrl,
                UseShellExecute = true
            });

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);

            listener.Start();
            Console.WriteLine($"Listening on {url}");
            Console.WriteLine("Waiting for callback...");
            string code = "";

            // 等待請求進來
            HttpListenerContext context = await listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine("=== Callback Received ===");
            Console.WriteLine($"Method: {request.HttpMethod}");
            Console.WriteLine($"URL: {request.Url}");

            // QueryString 參數
            foreach (string key in request.QueryString.AllKeys)
            {
                if (key == "code")
                {
                    code = request.QueryString[key].ToString();
                    break;
                }
            }
            Console.WriteLine(code);
            return code;

        }

        private async Task GetTokenByCode()
        {
            string code = await GetCode();
            //var body = new Dictionary<string, string>
            //{
            //    { "code", code },
            //    { "client_id", ClientId },
            //    { "client_secret", ClientSecret },
            //    { "redirect_uri", "http://localhost:5000" },
            //    { "grant_type", "authorization_code" },
            //    { "code_verifier", util.CodeVerifier }
            //};
            //var input = new FormUrlEncodedContent(body);

            var input = new
            {
                code = code,
                client_id = ClientId,
                client_secret = ClientSecret,
                redirect_uri = "http://localhost:5000",
                grant_type = "authorization_code",
                code_verifier = util.CodeVerifier,
            };

            TokenModel tokenModel = await HttpUtility.PostAsync<TokenModel>("https://oauth2.googleapis.com/token", input);
            AccessToken = tokenModel.access_token;
            RefreshToken = tokenModel.refresh_token;
            //ExpireTime = DateTime.Now.AddSeconds(tokenModel.expires_in);

            Credential credential = new Credential();
            credential.Target = "YoutubeCredential";
            credential.Username = "chi";
            credential.Password = $"{ClientId}|{ClientSecret}|{tokenModel.access_token}|{tokenModel.refresh_token}|{DateTime.Now.AddSeconds(tokenModel.expires_in)}";
            credential.Save();
        }
    }
}
