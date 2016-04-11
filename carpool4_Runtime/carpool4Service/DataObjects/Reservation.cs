using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Reservation : EntityData
    {
        public string id_user { get; set; }

        public string id_route { get; set; }
    }
}