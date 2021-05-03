using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace graphdaemon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = LoadAppSettings();
            if (config == null)
            {
                Console.WriteLine("Invalid appsettings.json file.");
                return;
            }

            var client = GetAuthenticatedGraphClient(config);

            var users = await client.Users.Request().Select("id").GetAsync();

            foreach (var user in users)
            {
                var queryOptions = new List<QueryOption>()
                {
                    new QueryOption("startdatetime", "2021-05-03T15:49:18.531Z"),
                    new QueryOption("enddatetime", "2021-05-10T15:49:18.531Z")
                };

                var calendarView = await client.Users[user.Id].CalendarView
                    .Request(queryOptions)
                    .GetAsync();
            }
        }

        private static GraphServiceClient GetAuthenticatedGraphClient(IConfigurationRoot config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);
            return new GraphServiceClient(authenticationProvider);
        }

        private static IAuthenticationProvider CreateAuthorizationProvider(IConfigurationRoot config)
        {
            var tenantId = config["tenantId"];
            var clientId = config["applicationId"];
            var clientSecret = config["applicationSecret"];
            var authority = $"https://login.microsoftonline.com/{config["tenantId"]}/v2.0";

            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(authority)
                                                    .WithClientSecret(clientSecret)
                                                    .Build();
            return MsalAuthenticationProvider.GetInstance(cca, scopes.ToArray());
        }

        private static IConfigurationRoot LoadAppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                                  .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", false, true)
                                  .Build();

                if (string.IsNullOrEmpty(config["applicationId"]) ||
                    string.IsNullOrEmpty(config["applicationSecret"]) ||
                    string.IsNullOrEmpty(config["tenantId"]))
                {
                    return null;
                }

                return config;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }
    }
}
