using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Car : EntityData
    {
        public string color { get; set; }

        public string id_user { get; set; }

        public string model { get; set; }
    }
}