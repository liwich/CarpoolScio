using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carpool4.Models
{
    public class Route
    {
        public string Id { get; set; }
        
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

        public override string ToString()
        {
            return string.Format("[Routes: ID={0}, " +
                                 "ID_User={1}, " +
                                 "ID_Car={2}, " +
                                 "From={3}, " +
                                 "From_Longitude={4}," +
                                 "From_Latitude={5}, " +
                                 "To={6}," +
                                 "To_Latitude={7}," +
                                 "To_Longitude={8}," +
                                 "Capacity={9}," +
                                 "Comments={10}," +
                                 "depart_time={11}]",
                                 Id, Id_User, Id_Car, From, From_Longitude, From_Latitude, To, To_Latitude, To_Longitude, Capacity, Comments, Depart_Time);
        }

    }
}
