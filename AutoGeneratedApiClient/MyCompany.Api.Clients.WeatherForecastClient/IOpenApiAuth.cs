using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.Api.Clients.WeatherForecastClient
{
	public interface IOpenApiAuth
	{
		void UseUserAuth(string userName, string password);
	}
}
