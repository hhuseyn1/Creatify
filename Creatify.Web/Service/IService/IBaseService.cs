using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface IBaseService
{
	Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBaerer = true);
}
