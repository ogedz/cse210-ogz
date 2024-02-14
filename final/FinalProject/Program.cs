using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        WeatherApp app = new WeatherApp(new UserInterface(), new WeatherAPI());
        await app.Run();
    }
}

class UserInterface
{
    public void DisplayWelcomeMessage()
    {
        Console.WriteLine("Welcome to the Ogeds Weather Forecast App!");
    }

    public string GetCityFromUser()
    {
        Console.WriteLine("Do you want to use your current location? (Y/N): ");
        string choice = Console.ReadLine()?.Trim().ToLower();

        if (choice == "y" || choice == "yes")
        {
            return "current";
        }
        else
        {
            Console.Write("Enter city name: ");
            return Console.ReadLine()?.Trim();
        }
    }

    public DateTime GetDateFromUser()
    {
        Console.Write("Enter date (YYYY-MM-DD): ");
        return DateTime.Parse(Console.ReadLine()?.Trim());
    }

    public void DisplayWeatherForecast(float temp, int humidity, string description, string city)
    {
        Console.WriteLine($"Weather Forecast for {city}");
        Console.WriteLine($"Temperature: {temp}°C");
        Console.WriteLine($"Humidity: {humidity}%");
        Console.WriteLine($"Weather: {description}");
    }

    public void DisplayErrorMessage(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public void DisplayLocation(string city)
    {
        Console.WriteLine($"Your current location: {city}");
    }
}

class WeatherApp
{
    private readonly UserInterface ui;
    private readonly WeatherAPI weatherAPI;

    public WeatherApp(UserInterface userInterface, WeatherAPI weatherApi)
    {
        ui = userInterface;
        weatherAPI = weatherApi;
    }

    public async Task Run()
    {
        ui.DisplayWelcomeMessage();
        string currentCity = await weatherAPI.GetCityFromIP();
        ui.DisplayLocation(currentCity);

        string city = ui.GetCityFromUser();

        if (city == "current")
        {
            city = currentCity;
        }

        DateTime date = ui.GetDateFromUser();

        try
        {
            var forecast = await weatherAPI.GetWeatherForecastAsync(city, date);
            ui.DisplayWeatherForecast(forecast.Temp, forecast.Humidity, forecast.Description, city);
        }
        catch (HttpRequestException ex)
        {
            ui.DisplayErrorMessage($"Error retrieving weather data: {ex.Message}");
        }
        catch (Exception ex)
        {
            ui.DisplayErrorMessage($"An unexpected error occurred: {ex.Message}");
        }
    }
}

class WeatherAPI
{
    private const string apiKey = "1e2bec4abb80b8f25eba7c9e5a5d556f";
    private const string apiUrl = "https://api.openweathermap.org/data/2.5/weather";
    private const string ipApiUrl = "https://ipinfo.io/json";
    private readonly HttpClient client;

    public WeatherAPI()
    {
        client = new HttpClient();
    }

    public async Task<WeatherForecast> GetWeatherForecastAsync(string city, DateTime date)
    {
        HttpResponseMessage response = await client.GetAsync($"{apiUrl}?q={city}&appid={apiKey}&units=metric");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var forecast = JsonConvert.DeserializeObject<WeatherForecastResponse>(responseBody);
        return new WeatherForecast
        {
            Temp = forecast.Main.Temp,
            Humidity = forecast.Main.Humidity,
            Description = forecast.Weather[0].Description
        };
    }

    public async Task<string> GetCityFromIP()
    {
        HttpResponseMessage response = await client.GetAsync(ipApiUrl);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var locationData = JsonConvert.DeserializeObject<LocationResponse>(responseBody);
        return locationData.City;
    }
}

class WeatherForecast
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; }
}

class WeatherForecastResponse
{
    public MainData Main { get; set; }
    public List<WeatherInfo> Weather { get; set; }
}

class MainData
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
}

class WeatherInfo
{
    public string Description { get; set; }
}

class LocationResponse
{
    public string City { get; set; }
}
