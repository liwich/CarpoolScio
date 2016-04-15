
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace carpool4
{
    public class AzureStorage
    {

        public static string connectionString =
        "DefaultEndpointsProtocol=https;AccountName=carpoolimages;AccountKey=VVslL6BFXbyK3CqzRWQ5A9dyElkflazFkOv5qClrFu2/n8HXJenIM4nnDEI9f/YSTTcVUCqXOVwryC3c8Jwc7w==";

        // container to be generated to upload files to
        public static string containerName = "images";

        public static string UploadPhoto(byte[] photobytes, string photoName)
        {
            // define your azure cloud account
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            // intilaize a client for the created account
            CloudBlobClient client = account.CreateCloudBlobClient();
            // intialize the container
            CloudBlobContainer container = client.GetContainerReference(containerName);
            // create the container if doesn't exist
            container.CreateIfNotExists();
            // set public permission for your blob to be able to be used by everyone 
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            // set the photo path ... note the photo name can be either "name.jpg" or it can has a virtual path like "pics/folder/name.jpg"
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            // start uploading
            photo.UploadFromByteArray(photobytes, 0, photobytes.Length);
            // return the url of the photo after uploading
            return photo.Uri.ToString();
        }

        public static byte[] DownloadPhoto(string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            byte[] photobytes = null;
            photo.DownloadToByteArray(photobytes, 0);
            return photobytes;
        }
    }
}
