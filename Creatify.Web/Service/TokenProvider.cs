using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class TokenProvider : ITokenProvider
{
	private readonly IHttpContextAccessor httpContextAccessor;

	public TokenProvider(IHttpContextAccessor httpContextAccessor)
	{
		this.httpContextAccessor = httpContextAccessor;
	}

	public void ClearToken()
	{
		httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.TokenCookie);
	}

	public string? GetToken()
	{
		string token = null;
		bool hasToken = httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);
		return hasToken ? token : null;
	}

	public void SetToken(string token)
	{
		httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.TokenCookie, token);
	}
}
