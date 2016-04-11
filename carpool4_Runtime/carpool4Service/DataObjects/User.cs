using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
   public class User : EntityData 
    {
        public string email { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public int age { get; set; }

        public string gender { get; set; }

        public string phone { get; set; }
    }
}
