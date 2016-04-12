using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carpool4.Models
{
    public class Reservation
    {
        public string Id { get; set; }

        public string Id_User { get; set; }
        
        public string Id_Route { get; set; }

        public string Id_Owner { get; set; }
    }
}
