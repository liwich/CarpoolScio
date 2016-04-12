using System;
using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Route : EntityData
    {
        public string Id_User { get; set; }

        public string Id_Car { get; set; }

        public string From { get; set; }

        public string From_Longitude { get; set; }

        public string From_Latitude { get; set; }

        public string To { get; set; }

        public string To_Latitude { get; set; }

        public string To_Longitude { get; set; }

        public int Capacity { get; set; }

        public string Comments { get; set; }

        public string Depart_Time { get; set; }

        public DateTime Depart_Date { get; set; } 
    }
}