using MyCompany.Api.Clients.WeatherForecastClient;
using System;
using System.Threading.Tasks;

namespace WeatherApiConsoleCheck
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var baseUrl = "http://localhost:5000";
			Console.WriteLine("Presss enter to start");
			Console.ReadLine();
			// Interface can be resolved in API clients over DI
			IWeatherForecastClient apiClient = new WeatherForecastClient(baseUrl);

			// Call to get data
			var forecasts = await apiClient.GetAllAsync();

			// set username and pass
			apiClient.UseUserAuth("admin", "321123");


			// add data to api closed with AUTH
			var resultOfAdding = await apiClient.AddAsync(new AddWeatherData()
			{
				Kind = TemperatureType.Celcium,
				Region = "Ukraine",
				Temperature = 50
			});
			Console.WriteLine($"Is succeffully added: {resultOfAdding}");

			// Example that another controller also has client:
			var authClient = new AuthClient(baseUrl);
			try
			{
				var token = await authClient.LoginAsync("", new LoginModel()
				{
					UserName = "HasNoAccess",
					Password = "11111"
				});
			} catch (SwaggerException e){
				Console.WriteLine("Unauthorized");
			}

			Console.ReadLine();
		}
	}
}
