using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Creatify.Web.Utility.StaticDetails;

namespace Creatify.Web.Service;

public class BaseService : IBaseService
{
	private readonly IHttpClientFactory httpClientFactory;
	private readonly ITokenProvider _tokenProvider;

	public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
	{
		this.httpClientFactory = httpClientFactory;
		this._tokenProvider = tokenProvider;
	}

	public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBaerer = true)
	{
		try
		{

			HttpClient httpClient = httpClientFactory.CreateClient("CreatifyAPI");
			HttpRequestMessage message = new();
			message.Headers.Add("Accept", "application/json");

			//token
			if (withBaerer)
			{
				var token = _tokenProvider.GetToken();
				message.Headers.Add("Authorization", $"Bearer {token}");
			}

			message.RequestUri = new Uri(requestDto.Url);
			if (requestDto.Data is not null)
				message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponse = null;

			message.Method = (requestDto.APIType) switch
			{
				APIType.POST => HttpMethod.Post,
				APIType.PUT => HttpMethod.Put,
				APIType.DELETE => HttpMethod.Delete,
				_ => HttpMethod.Get
			};

			httpResponse = await httpClient.SendAsync(message);

			switch (httpResponse.StatusCode)
			{

				case HttpStatusCode.NotFound:
					return new() { isSuccess = false, Message = "Not Found" };
				case HttpStatusCode.Forbidden:
					return new() { isSuccess = false, Message = "Access Denied" };
				case HttpStatusCode.Unauthorized:
					return new() { isSuccess = false, Message = "Unauthorized" };
				case HttpStatusCode.InternalServerError:
					return new() { isSuccess = false, Message = "Internal Server Error" };
				default:
					var apiContent = await httpResponse.Content.ReadAsStringAsync();
					var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
					return apiResponseDto;
			}


		}
		catch (Exception ex)
		{
			var dto = new ResponseDto
			{
				isSuccess = false,
				Message = ex.Message.ToString()
			};
			return dto;
		}
	}
}
