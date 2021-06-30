using MyCompany.Api.Clients.WeatherForecastClient;
using System;
using System.Threading.Tasks;

namespace WeatherApiConsoleCheck
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Presss enter to start");
			Console.ReadLine();
			IWeatherForecastClient apiClient = new WeatherForecastClient("http://localhost:5000");
			var forecasts = await apiClient.GetAllAsync();

			var resultOfAdding = await apiClient.AddAsync(new AddWeatherData()
			{
				Kind = TemperatureType.Celcium,
				Region = "Ukraine",
				Temperature = 50
			});
		}
	}
}
