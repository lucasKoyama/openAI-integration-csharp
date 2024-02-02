namespace TrybeHotel.Dto;

public class TravelPlanRequest {
    public string? Prompt { get; set; }
}
public class TravelPlanResponse {
    public string? CityName { get; set; }
    public string? Text { get; set; }
    public List<HotelDto>? Hotels { get; set; }
}