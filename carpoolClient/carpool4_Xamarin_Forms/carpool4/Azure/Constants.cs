using System;

namespace carpool4
{
	public static class Constants
	{
		// Replace strings with your mobile services and gateway URLs.
		public static string ApplicationURL = @"https://carpool4.azurewebsites.net";

        public static string connectionString = 
        "DefaultEndpointsProtocol=https;AccountName=carpoolimages;AccountKey=VVslL6BFXbyK3CqzRWQ5A9dyElkflazFkOv5qClrFu2/n8HXJenIM4nnDEI9f/YSTTcVUCqXOVwryC3c8Jwc7w==";

        // container to be generated to upload files to
        public static string containerName = "images";
    }
}

