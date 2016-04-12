using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Reservation : EntityData
    {
        public string Id_User { get; set; }

        public string Id_Route { get; set; }

        public string Id_Owner { get; set; }  
    }
}