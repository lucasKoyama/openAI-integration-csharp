namespace TrybeHotel.Services;
using TrybeHotel.Dto;

public interface ITravelService
{
    Task<TravelPlanResponse> GetTravelPlan(TravelPlanRequest request);
}