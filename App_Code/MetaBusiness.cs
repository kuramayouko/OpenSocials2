namespace OpenSocials.App_Code
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

	public class MetaBusiness
	{
		public Facebook FacebookApi { get; set; }
		public Instagram InstagramApi { get; set; }

		// REFAZER: passar o token criptografado no appconfig.json e depois descriptografar na hora de chamar na url 
		private string FacebookPageId;
		private string InstagramPageId;
        private readonly string UserToken;
        private readonly string PageToken;
        
        // easy to change api versions
        private readonly string _facebookApiBaseUrl = "https://graph.facebook.com/v18.0";
        
        //pegas ids de contas precisa usar usertokken ENDPOINT!!!
		// https://graph.facebook.com/v18.0/me/accounts?fields=instagram_business_account%2Cname&access_token=
		
		public async List<string> GetPagesId()
		{
			using (HttpClient client = new HttpClient())
			{
				string apiUrl = $"{_facebookApiBaseUrl}/me/accounts?fields=instagram_business_account,name&access_token={this.UserTokken}";

				var response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					var jsonObject = JObject.Parse(jsonData);
					var data = jsonObject["data"];

					if (data != null && data.HasValues)
					{
						var firstItem = data[0];
						
						var List<ids>;

						// 0 is Instagram
						// 1 is Facebook
						ids[0] = firstItem["instagram_business_account"]?["id"]?.ToString();
						ids[1] = firstItem["id"]?.ToString();
					}
				}
				else
				{
					Console.WriteLine($"Erro: {response.StatusCode}");
				}
			}
		}
		    
		public async Task<bool> IsUserTokenValid(string userToken)
		{
			var apiUrl = $"{_facebookApiBaseUrl}/me?fields=id&access_token={userToken}";

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(apiUrl);
				return response.IsSuccessStatusCode;
			}
		}

		public async Task<bool> IsPageTokenValid(string pageToken)
		{
			var apiUrl = $"{_facebookApiBaseUrl}/me?fields=id&access_token={pageToken}";

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(apiUrl);
				return response.IsSuccessStatusCode;
			}
		}
	}
	
	public class Facebook
	{

	}

	public class Instagram
	{
		// Get live feed param int limit metodo
		// InstagramPageID/media/?fields=caption,thumbnail_url,media_url&limit=10
		
		// Posts to instagram requires
		// InstagramPageID/media ?fields=caption,image_url,media_type the response generates and id post
		// THE id_post needs another request 
		// InstagramPageID/media_publish ?fields=creation_id the previous id
		
	}
		
}

