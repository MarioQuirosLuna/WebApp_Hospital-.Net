using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Hospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int Doctor_identification_card { get; set; }
        public int Doctor_code { get; set; }
        public string Doctor_name { get; set; }
        public string Doctor_password { get; set; }
    }
}
