namespace OpenSocials.App_Code
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	
	public class InstaPost
	{
		public string Id { get; set; }
		public string Caption { get; set; }
		public string MediaType { get; set; }
		public string MediaUrl { get; set; }
		public string ThumbnailUrl { get; set; }
		public string Permalink { get; set; }
		public string Timestamp { get; set; }
		public List<InstaComment> InstaComments { get; set; }
	}

	public class InstaComment
	{
		public string Timestamp { get; set; }
		public string Text { get; set; }
		public string From { get; set; }
	}

	public class Instagram
	{
		private string pageId;
		private string accessToken;

		public Instagram(string pageId, string accessToken)
		{
			this.pageId = pageId;
			this.accessToken = accessToken;
		}

		// refazer os links da API
		public async Task<List<InstaPost>> GetPostsAndInstaComments()
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{this.pageId}/media?fields=id,caption,media_type,media_url,thumbnail_url,permalink,timestamp,InstaComments{{timestamp,text,from}},children{{id,media_type,media_url,thumbnail_url,permalink,timestamp}}&access_token={this.accessToken}";

					HttpResponseMessage response = await client.GetAsync(requestUrl);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						var data = JsonConvert.DeserializeObject<JObject>(responseContent);

						if (data != null && data["data"] != null)
						{
							List<InstaPost> posts = new List<InstaPost>();

							foreach (var item in data["data"])
							{
								InstaPost post = new InstaPost
								{
									Id = item["id"].ToString(),
									Caption = item["caption"]?["text"]?.ToString(),
									MediaType = item["media_type"].ToString(),
									MediaUrl = item["media_url"].ToString(),
									ThumbnailUrl = item["thumbnail_url"]?.ToString(),
									Permalink = item["permalink"]?.ToString(),
									Timestamp = item["timestamp"].ToString(),
									InstaComments = new List<InstaComment>()
								};

								if (item["InstaComments"] != null)
								{
									foreach (var InstaComment in item["InstaComments"]["data"])
									{
										InstaComment postInstaComment = new InstaComment
										{
											Timestamp = InstaComment["timestamp"].ToString(),
											Text = InstaComment["text"].ToString(),
											From = InstaComment["from"]["username"].ToString()
										};
										post.InstaComments.Add(postInstaComment);
									}
								}

								posts.Add(post);
							}

							return posts;
						}
					}
					else
					{
						Console.WriteLine($"Falha ao receber posts. Erro: {response.StatusCode}");
						return null;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return null;
				}

				return null;
			}
		}
		
		public async Task<bool> PostTextOnly(string caption)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{this.pageId}/media?access_token={this.accessToken}";

					var payload = new
					{
						caption = caption,
						media_type = "TEXT",
					};

					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync(requestUrl, content);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Post criado com successo.");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao criar Post. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}
		}
		
		public async Task<bool> PostWithMedia(Media media)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{this.pageId}/media?access_token={this.accessToken}";


					var payload = new
					{
						caption = media.MediaTitle,
						media_type = media.MediaType,
						image_base64 = media.MediaType == "photo" ? media.Base64 : null,
						video_url = media.MediaType == "video" ? media.MediaLocalUrl : null
					};

					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

					
					HttpResponseMessage response = await client.PostAsync(requestUrl, content);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Post criado com sucesso.");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao criar Post. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}
		}
		
		public async Task<bool> PostCarousel(List<Media> mediaList, string caption)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{this.pageId}/children?access_token={this.accessToken}";

					
					foreach (var media in mediaList)
					{
						object mediaObject;
						if (media.MediaType == "photo")
						{
							mediaObject = new { media_type = media.MediaType, image_base64 = media.Base64 };
						}
						else
						{
							mediaObject = new { media_type = media.MediaType, video_url = media.MediaLocalUrl };
						}
					}


					var payload = "";
					//{
					//	media_type = "CAROUSEL_ALBUM",
					//	caption = caption,
					//	children = mediaArray
					//};

					var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

					
					HttpResponseMessage response = await client.PostAsync(requestUrl, content);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Post criado com sucesso.");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao criar post. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}
		}
		
		public async Task<bool> EditPostCaption(string postId, string newCaption)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{postId}?access_token={this.accessToken}";

					var payload = new
					{
						caption = newCaption
					};

					var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync(requestUrl, content);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Post atualizado com sucesso.");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao atualizar post. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}
		}

		public async Task<bool> DeletePost(string postId)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{postId}?access_token={this.accessToken}";

					HttpResponseMessage response = await client.DeleteAsync(requestUrl);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Post deletado com sucesso");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao deletar post. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}
		}
		
		public async Task<bool> ReplyToPost(string postId, string commentText)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string requestUrl = $"https://graph.instagram.com/v18.0/{postId}/comments?access_token={this.accessToken}";

					var payload = new
					{
						text = commentText
					};

					var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync(requestUrl, content);

					if (response.IsSuccessStatusCode)
					{
						Console.WriteLine("Comentario postado com sucesso.");
						return true;
					}
					else
					{
						Console.WriteLine($"Falha ao postar comentario. Erro: {response.StatusCode}");
						return false;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Erro: " + ex.Message);
					return false;
				}
			}

		}
	}
}
