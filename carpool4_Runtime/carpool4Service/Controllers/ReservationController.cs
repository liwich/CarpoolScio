using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using carpool4Service.DataObjects;
using carpool4Service.Models;
using System.Collections.Generic;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;

namespace carpool4Service.Controllers
{
    public class ReservationController : TableController<Reservation>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            carpool4Context context = new carpool4Context();
            DomainManager = new EntityDomainManager<Reservation>(context, Request);
        }

        // GET tables/Reservation
        public IQueryable<Reservation> GetAllReservations()
        {
            return Query();
        }

        // GET tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Reservation> GetReservation(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Reservation> PatchReservation(string id, Delta<Reservation> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Reservation
        public async Task<IHttpActionResult> PostReservation(Reservation item)
        {
            Reservation current = await InsertAsync(item);

            // Get the settings for the server project.
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
                this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Create a new Notification Hub client.
            NotificationHubClient hub = NotificationHubClient
            .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Sending the message so that all template registrations that contain "messageParam"
            // will receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = item.Id_Owner + ",Se ha reservado tu ruta";

            try
            {
                // Send the push notification and log the results.
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                // Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReservation(string id)
        {
            // Get the settings for the server project.
            HttpConfiguration config = this.Configuration;
            MobileAppSettingsDictionary settings =
                this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            // Create a new Notification Hub client.
            NotificationHubClient hub = NotificationHubClient
            .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            // Sending the message so that all template registrations that contain "messageParam"
            // will receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = "Se ha cancelado la ruta";

            try
            {
                // Send the push notification and log the results.
                var result = hub.SendTemplateNotificationAsync(templateParams);

                // Write the success result to the logs.
                //config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                // Write the failure result to the logs.
                config.Services.GetTraceWriter()
                    .Error(ex.Message, null, "Push.SendAsync Error");
            }

            return DeleteAsync(id); 
        }
    }
}