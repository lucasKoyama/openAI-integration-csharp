namespace TrybeHotel.Dto;

public class IAResponseDto
{
    public List<IAChoiceDto>? choices { get; set; }
}

public class IAChoiceDto
{
    public IAMessageDto? message { get; set; }
}

public class IAMessageDto
{
    public string? content { get; set; }
}