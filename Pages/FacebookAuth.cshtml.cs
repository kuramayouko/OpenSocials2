using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using OpenSocials.App_Code;

namespace OpenSocials.Pages
{
    public class FacebookAuthModel : PageModel
    {
        private readonly IConfiguration _configuration;
		private String appId;
		private String appSecret;
		private String redirectUri;

        public FacebookAuthModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        private readonly DataContext _context;

        public void OnGet()
        {
			Config Config = _context.Config.FirstOrDefault();

			if (Config != null)
			{
				//Pegar os valores do BD
				this.appId = Config.AppId;
				this.appSecret = Config.AppSecret;

				string facebookLoginUrl = $"https://www.facebook.com/v18.0/dialog/oauth?client_id={appId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=token";

				Redirect(facebookLoginUrl);
			}
			else
			{
				// Redirecionar pagina erro
			}

        }

        public async Task<IActionResult> OnGetCallback(string access_token)
        {
            if (!string.IsNullOrEmpty(access_token))
            {
                string longLivedToken = await ExchangeForLongLivedToken(access_token);

                var userDetails = await GetFacebookUserDetails(longLivedToken);

                // Resposta JSON implementar
                // Ex:
                // var userId = userDetails["id"];
                // var userName = userDetails["name"];
            }
            return Page();
        }
   
		private async Task<dynamic> GetFacebookUserDetails(string accessToken)
		{
			using (HttpClient client = new HttpClient())
			{
				var response = await client.GetAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");
				response.EnsureSuccessStatusCode();

				string json = await response.Content.ReadAsStringAsync();
				dynamic data = JsonConvert.DeserializeObject(json);

				return data;
			}
		}

		private async Task<string> ExchangeForLongLivedToken(string shortLivedToken)
		{

			using (HttpClient client = new HttpClient())
			{
				var response = await client.GetAsync($"https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id={appId}&client_secret={appSecret}&fb_exchange_token={shortLivedToken}");
				response.EnsureSuccessStatusCode();

				string json = await response.Content.ReadAsStringAsync();
				dynamic data = JsonConvert.DeserializeObject(json);

				return data.access_token;
			}
		}
    }
}
