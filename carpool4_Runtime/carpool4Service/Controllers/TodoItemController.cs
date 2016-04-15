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

using System;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace carpool4Service.Controllers
{
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            carpool4Context context = new carpool4Context();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        //// POST tables/TodoItem
        //public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        //{
        //    TodoItem current = await InsertAsync(item);

        //    // Get the settings for the server project.
        //    HttpConfiguration config = this.Configuration;
        //    MobileAppSettingsDictionary settings =
        //        this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

        //    // Get the Notification Hubs credentials for the Mobile App.
        //    string notificationHubName = settings.NotificationHubName;
        //    string notificationHubConnection = settings
        //        .Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

        //    // Create a new Notification Hub client.
        //    NotificationHubClient hub = NotificationHubClient
        //    .CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

        //    // Sending the message so that all template registrations that contain "messageParam"
        //    // will receive the notifications. This includes APNS, GCM, WNS, and MPNS template registrations.
        //    Dictionary<string, string> templateParams = new Dictionary<string, string>();
        //    templateParams["messageParam"] = "778245330a7a46178cdfffe22ec48166," + item.Text + " was added to the list.";

        //    try
        //    {
        //        // Send the push notification and log the results.
        //        var result = await hub.SendTemplateNotificationAsync(templateParams);

        //        // Write the success result to the logs.
        //        config.Services.GetTraceWriter().Info(result.State.ToString());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        // Write the failure result to the logs.
        //        config.Services.GetTraceWriter()
        //            .Error(ex.Message, null, "Push.SendAsync Error");
        //    }

        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}


        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            string storageAccountName= "carpoolimages";
            string storageAccountKey= "VVslL6BFXbyK3CqzRWQ5A9dyElkflazFkOv5qClrFu2/n8HXJenIM4nnDEI9f/YSTTcVUCqXOVwryC3c8Jwc7w==";

            //// Try to get the Azure storage account token from app settings.  
            //if (!(Services.Settings.TryGetValue("STORAGE_ACCOUNT_NAME", out storageAccountName) |
            //Services.Settings.TryGetValue("STORAGE_ACCOUNT_ACCESS_KEY", out storageAccountKey)))
            //{
            //    Services.Log.Error("Could not retrieve storage account settings.");
            //}

            // Set the URI for the Blob Storage service.
            Uri blobEndpoint = new Uri(string.Format("https://{0}.blob.core.windows.net", storageAccountName));

            // Create the BLOB service client.
            CloudBlobClient blobClient = new CloudBlobClient(blobEndpoint,
                new StorageCredentials(storageAccountName, storageAccountKey));

            if (item.containerName != null)
            {
                // Set the BLOB store container name on the item, which must be lowercase.
                item.containerName = item.containerName.ToLower();

                // Create a container, if it doesn't already exist.
                CloudBlobContainer container = blobClient.GetContainerReference(item.containerName);
                await container.CreateIfNotExistsAsync();

                // Create a shared access permission policy. 
                BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

                // Enable anonymous read access to BLOBs.
                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(containerPermissions);

                // Define a policy that gives write access to the container for 5 minutes.                                   
                SharedAccessBlobPolicy sasPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    Permissions = SharedAccessBlobPermissions.Write
                };

                // Get the SAS as a string.
                item.sasQueryString = container.GetSharedAccessSignature(sasPolicy);

                // Set the URL used to store the image.
                item.imageUri = string.Format("{0}{1}/{2}", blobEndpoint.ToString(),
                    item.containerName, item.resourceName);
            }

            // Complete the insert operation.
            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }


        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}