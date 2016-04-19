
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


        private static CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
        private static CloudBlobClient client = account.CreateCloudBlobClient();
        private static CloudBlobContainer container = client.GetContainerReference(Constants.containerName);

        public static string UploadPhoto(byte[] photobytes, string photoName)
        {
            container.CreateIfNotExists();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(containerPermissions);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);

            photo.UploadFromByteArray(photobytes, 0, photobytes.Length);

            return photo.Uri.ToString();
        }

        public static Uri DownloadPhoto(string photoName)
        {
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

        public static bool DeletePhoto(string photoName)
        {
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            try
            {
                return photo.DeleteIfExists();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static bool ExistPhoto(string photoName)
        {
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            try
            {
                return photo.Exists();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
