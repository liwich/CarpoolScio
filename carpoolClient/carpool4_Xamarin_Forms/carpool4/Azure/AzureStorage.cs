
using System;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace carpool4
{
    public class AzureStorage
    {
        public static string connectionString =
        "DefaultEndpointsProtocol=https;AccountName=" + Constants.StorageAccount + ";AccountKey=" + Constants.StorageKey;

        // container to be generated to upload files to

        public static string UploadPhoto(byte[] photobytes, string photoName)
        {
            // define your azure cloud account
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            // intilaize a client for the created account
            CloudBlobClient client = account.CreateCloudBlobClient();
            // intialize the container
            CloudBlobContainer container = client.GetContainerReference(Constants.containerName);
            // create the container if doesn't exist
            container.CreateIfNotExists();
            // set public permission for your blob to be able to be used by everyone 
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            // set the photo path ... note the photo name can be either "name.jpg" or it can has a virtual path like "pics/folder/name.jpg"
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            // start uploading
            
            photo.Delete(DeleteSnapshotsOption.IncludeSnapshots);
            photo.UploadFromByteArray(photobytes, 0, photobytes.Length);

            // return the url of the photo after uploading
            return photo.Uri.ToString();
        }

        public static Uri DownloadPhoto(string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(Constants.containerName);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            try
            {
                //photo.DownloadToByteArray(photobytes, 0);
                if (photo.Exists())
                {
                    return photo.Uri;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }


        }
    }
}
