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

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<ResponseDto> SendAsync(RequestDto requestDto)
    {
        try
        {

            HttpClient httpClient = httpClientFactory.CreateClient("CreatifyAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            //token

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

            var response = httpResponse.StatusCode switch
            {
                HttpStatusCode.Forbidden => new ResponseDto { isSuccess = false, Message = "Forbidden" },
                HttpStatusCode.InternalServerError => new ResponseDto { isSuccess = false, Message = "Internal Server Error" },
                HttpStatusCode.NotFound => new ResponseDto { isSuccess = false, Message = "Not Found" },
                HttpStatusCode.Unauthorized => new ResponseDto { isSuccess = false, Message = "Unauthorized" },
            };

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return apiResponseDto ?? response;

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
