using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using carpool4Service.DataObjects;
using carpool4Service.Models;
using Owin;

namespace carpool4Service
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new carpool4Initializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<carpool4Context>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class carpool4Initializer : CreateDatabaseIfNotExists<carpool4Context>
    {
        protected override void Seed(carpool4Context context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "3First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "2Second item", Complete = false },
            };

            List<Car> cars = new List<Car>
            {
                new Car { Id = Guid.NewGuid().ToString(), Color = "lFirst item" },
                new Car { Id = Guid.NewGuid().ToString(), Color = "2Second item" },
            };
            List<Reservation> reservations = new List<Reservation>
            {
                new Reservation { Id = Guid.NewGuid().ToString()},
                new Reservation { Id = Guid.NewGuid().ToString()},
            };
            //List<Route> routes = new List<Route>
            //{
            //    new Route { Id = Guid.NewGuid().ToString()},
            //    new Route { Id = Guid.NewGuid().ToString()},
            //};
            List<User> users = new List<User>
            {
                new User { Id = Guid.NewGuid().ToString()},
                new User { Id = Guid.NewGuid().ToString()},
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }
            foreach (Car car in cars)
            {
                context.Set<Car>().Add(car);
            }
            foreach (Reservation reservation in reservations)
            {
                context.Set<Reservation>().Add(reservation);
            }
            //foreach (Route route in routes)
            //{
            //    context.Set<Route>().Add(route);
            //}
            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }


            base.Seed(context);
        }
    }
}

