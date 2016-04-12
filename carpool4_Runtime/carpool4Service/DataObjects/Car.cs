using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Car : EntityData
    {
        public string Color { get; set; }

        public string Id_User { get; set; }

        public string Model { get; set; } 
    }
}