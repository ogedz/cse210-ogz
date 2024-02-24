using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        WeatherApp weatherApp = new WeatherApp(new UserInterface(), new WeatherAPI());

        try
        {
            await weatherApp.Run();
        }
        catch (HttpRequestException ex)
        {
            weatherApp.DisplayErrorMessage($"Error retrieving weather data: {ex.Message}");
        }
        catch (Exception ex)
        {
            weatherApp.DisplayErrorMessage($"An unexpected error occurred: {ex.Message}");
        }
    }
}

class WeatherApp
{
    private readonly UserInterface userInterface;
    private readonly WeatherAPI weatherAPI;

    public WeatherApp(UserInterface ui, WeatherAPI api)
    {
        userInterface = ui;
        weatherAPI = api;
    }

    public async Task Run()
    {
        var userLocation = await GetUserLocationAsync();
        userInterface.DisplayLocationAndDateTime(userLocation);

        userInterface.DisplayWelcomeMessage();
        string city = userInterface.GetCityFromUser();
        DateTime startDate = userInterface.GetDateFromUser();

        var forecast = await weatherAPI.GetWeatherForecastAsync(city, startDate);
        userInterface.DisplayWeatherForecast(forecast, city);
    }

    private async Task<string> GetUserLocationAsync()
    {
        string apiKey = "1203eb5d00b2466bb89befc93b06922f";
        string url = $"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}";

        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic locationData = JsonConvert.DeserializeObject(responseBody);
            return $"{locationData.city}, {locationData.country_name}";
        }
    }

    public void DisplayErrorMessage(string message)
    {
        userInterface.DisplayErrorMessage(message);
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
        string city;
        do
        {
            Console.Write("Enter city name: ");
            city = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(city)); // handling error
        return city;
    }

    public DateTime GetDateFromUser()
    {
        DateTime date;
        string inputDate;
        do
        {
            Console.Write("Enter date (MM-DD-YYYY): ");
            inputDate = Console.ReadLine();
        } while (!DateTime.TryParseExact(inputDate, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out date)); // Keep prompting until a valid date is provided
        return date;
    }

    public void DisplayLocationAndDateTime(string location)
    {
        Console.WriteLine($"Your current location: {location}");

        // Displaying current date and time
        DateTime currentDateTime = DateTime.Now;
        Console.WriteLine($"Current date and time: {currentDateTime.ToString("MM-dd-yyyy HH:mm:ss")}");
    }

    public void DisplayWeatherForecast(WeatherForecast forecast, string city)
    {
        Console.WriteLine($"Weather Forecast for {city}");
        forecast.DisplayWeatherDetails();
    }

    public void DisplayErrorMessage(string message)
    {
        Console.WriteLine($"Error: {message}");
    }
}

abstract class WeatherData
{
    public abstract void DisplayWeatherDetails();
}

class WeatherForecast : WeatherData
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; }
    public float WindSpeed { get; set; }
    public string WindDirection { get; set; }
    public int Pressure { get; set; }

    public override void DisplayWeatherDetails()
    {
        Console.WriteLine($"Temperature: {Temp}°C");
        Console.WriteLine($"Humidity: {Humidity}%");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Wind Speed: {WindSpeed}m/s");
        Console.WriteLine($"Wind Direction:{WindDirection}");
        Console.WriteLine($"Pressure: {Pressure}hPa");
    }
}

class WeatherForecastResponse : WeatherData
{
    public MainData Main { get; set; }
    public WeatherInfo[] Weather { get; set; }
    public WindInfo Wind { get; set; }

    public override void DisplayWeatherDetails()
    {
        Console.WriteLine($"Temperature: {Main.Temp}°C");
        Console.WriteLine($"Humidity: {Main.Humidity}%");
        Console.WriteLine($"Description: {Weather[0].Description}");
        Console.WriteLine($"Wind Speed: {Wind.Speed}m/s");
        Console.WriteLine($"Wind Direction:{Wind.Direction}");
        Console.WriteLine($"Pressure: {Main.Pressure}hPa");
    }
}

class MainData
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
    public int Pressure { get; set; }
}

class WeatherInfo
{
    public string Description { get; set; }
}

class WindInfo
{
    public float Speed { get; set; }
    public string Direction { get; set; }
}

class WeatherAPI
{
    private const string apiKey = "1e2bec4abb80b8f25eba7c9e5a5d556f";
    private const string apiUrl = "https://api.openweathermap.org/data/2.5/weather";
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
        var forecastResponse = JsonConvert.DeserializeObject<WeatherForecastResponse>(responseBody);

        // Extract relevant data from the response of Open Weather
        var forecast = new WeatherForecast
        {
            Temp = forecastResponse.Main.Temp,
            Humidity = forecastResponse.Main.Humidity,
            Description = forecastResponse.Weather[0].Description,
            WindSpeed = forecastResponse.Wind.Speed,
            WindDirection = forecastResponse.Wind.Direction,
            Pressure = forecastResponse.Main.Pressure,
        };

        return forecast;
    }
}
