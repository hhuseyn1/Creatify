namespace Services.Product.API.Models.Dto;

public class ResponseDto
{
	public object? Result { get; set; }
	public bool isSuccess { get; set; } = true;
	public string Message { get; set; } = "";
}
