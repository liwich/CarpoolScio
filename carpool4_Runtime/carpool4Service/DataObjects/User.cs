using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
   public class User : EntityData 
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }
    }
}
