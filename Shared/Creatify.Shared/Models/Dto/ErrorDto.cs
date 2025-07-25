namespace Creatify.Shared.Models.Dto;

public class ErrorDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string>? Errors { get; set; }
}
