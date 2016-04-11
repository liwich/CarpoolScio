using System;
using Microsoft.Azure.Mobile.Server;

namespace carpool4Service.DataObjects
{
    public class Route : EntityData
    {
        public string id_user { get; set; }

        public string id_car { get; set; }

        public string from { get; set; }

        public string from_longitude { get; set; }

        public string from_latitude { get; set; }

        public string to { get; set; }

        public string to_latitude { get; set; }

        public string to_longitude { get; set; }

        public int capacity { get; set; }

        public string comments { get; set; }

        public string depart_time { get; set; }

        public DateTime depart_date { get; set; }
    }
}