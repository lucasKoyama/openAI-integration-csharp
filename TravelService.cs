namespace TrybeHotel.Services;
using TrybeHotel.Dto;
using System.Text;
using Newtonsoft.Json;
using TrybeHotel.Repository;

public class TravelService : ITravelService
{
    protected readonly HttpClient _client;
    protected readonly ICityRepository _cityRepository;
    protected readonly IHotelRepository _hotelRepository;
    
    private string token = "token-chat-gpt";
    
    public TravelService(HttpClient client, ICityRepository cityRepository, IHotelRepository hotelRepository)
    {
        _client = client;
        _cityRepository = cityRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task<TravelPlanResponse> GetTravelPlan(TravelPlanRequest request)
    {

        List<CityDto> cities = _cityRepository.GetCities().ToList();
        List<HotelDto> hotels = _hotelRepository.GetHotels().ToList();

        string citiesNames = "";
        foreach(var city in cities)
        {
            citiesNames += city.Name + ", ";
        }
        string Prompt = "Dentre as seguintes cidades: " + citiesNames
                    + ", escolha a melhor cidade baseada no pedido: '" + request.Prompt+ "'."
                    + " Escreva um texto de 100 palavras explicando de maneira positiva sobre a cidade escolhida. Caso nenhuma cidade se enquadre na solicitação, responda apenas o texto: 'NOK'";

        var requestBody = JsonConvert.SerializeObject(new {
             model = "gpt-3.5-turbo",
             messages = new object[]{
                new {
                    role = "system",
                    content = Prompt
                }
             }
        });

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://api.openai.com/v1/chat/completions")
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };
        httpRequest.Headers.Add("Accept", "application/json");
        httpRequest.Headers.Add("User-Agent", "trybehotel");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(httpRequest);

        if(!response.IsSuccessStatusCode) return default!;

        var result = await response.Content.ReadFromJsonAsync<IAResponseDto>();
        var resultText = result!.choices!.First().message!.content!.ToString();

        CityDto cityChoiced = null!;
        foreach(var city in cities)
        {
            if (resultText.IndexOf(city.Name) > -1)
            {
                cityChoiced = city;
            }
        }

        TravelPlanResponse travelResponse = new TravelPlanResponse {
            CityName = cityChoiced.Name,
            Text = resultText,
            Hotels = hotels.Where(hotel => hotel.CityId == cityChoiced.CityId).ToList()
        };
        return travelResponse;

    }
}