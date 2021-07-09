using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Hospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int Fk_patient_identification_card { get; set; }
        public string Patient_name { get; set; }
        public int Fk_doctor_identification_card { get; set; }
        public string Doctor_name { get; set; }
        public string Clinic_name { get; set; }
        public DateTime Date_time { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minutes { get; set; }
        public string Seconds { get; set; }
        public string Speciality { get; set; }
        public string Diagnosis { get; set; }

        public Appointment(int Id, int Fk_patient_identification_card, string Patient_name, int Fk_doctor_identification_card, string Doctor_name, string Clinic_name, DateTime Date_time, string Speciality, string Diagnosis) 
        {
            this.Id = Id;
            this.Fk_patient_identification_card = Fk_patient_identification_card;
            this.Patient_name = Patient_name;
            this.Fk_doctor_identification_card = Fk_doctor_identification_card;
            this.Doctor_name = Doctor_name;
            this.Clinic_name = Clinic_name;
            this.Date_time = Date_time;
            this.Speciality = Speciality;
            this.Diagnosis = Diagnosis;
        }

        public Appointment() 
        {
        }
    }
}
