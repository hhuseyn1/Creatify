namespace Creatify.Shared.Models;

public class ErrorDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string>? Errors { get; set; }
}
