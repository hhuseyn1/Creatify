using static Creatify.Web.Utility.StaticDetails;

namespace Creatify.Web.Models;

public class RequestDto
{
	public APIType APIType { get; set; } = APIType.GET;
	public string Url { get; set; }
	public object Data { get; set; }
	public string AccessToken { get; set; }

	public ContentType ContentType { get; set; } = ContentType.Json;
}
