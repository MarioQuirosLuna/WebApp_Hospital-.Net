using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Hospital.Models;

namespace WebApp_Hospital.Controllers
{
    public class VaccineController : Controller
    {
        public IConfiguration Configuration { get; }

        public VaccineController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            List<Vaccine> vaccines = new List<Vaccine>();

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getVaccineByPatientId @id ='{id}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dataReader = await command.ExecuteReaderAsync();
                        while (dataReader.Read())
                        {
                            int Id = Int32.Parse(dataReader["id"].ToString());
                            int PatientIdentification = Int32.Parse(dataReader["fk_patient_identification_card"].ToString());
                            string Patiend_name = dataReader["patient_name"].ToString();
                            string Vaccine_name = dataReader["vaccine_name"].ToString();
                            string Vaccine_description = dataReader["vaccine_description"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            DateTime Next_dose_date_time = (DateTime)dataReader["next_dose_date_time"];
                            string Clinic_name = dataReader["clinic_name"].ToString();

                            vaccines.Add(new Vaccine(Id, PatientIdentification, Patiend_name, Vaccine_name, Vaccine_description, Date, Next_dose_date_time, Clinic_name));
                        }//while
                        connection.Close();
                    }
                }
            }

            ViewBag.IdPatient = id;

            return View(vaccines);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {

            //TODO: Validate not found

            Vaccine temp = null;

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getVaccineById @id='{id}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dataReader = await command.ExecuteReaderAsync();
                        while (dataReader.Read())
                        {
                            int Id = Int32.Parse(dataReader["id"].ToString());
                            int PatientIdentification = Int32.Parse(dataReader["fk_patient_identification_card"].ToString());
                            string Patiend_name = dataReader["patient_name"].ToString();
                            string Vaccine_name = dataReader["vaccine_name"].ToString();
                            string Vaccine_description = dataReader["vaccine_description"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            DateTime Next_dose_date_time = (DateTime)dataReader["next_dose_date_time"];
                            string Clinic_name = dataReader["clinic_name"].ToString();

                            temp = (new Vaccine(Id, PatientIdentification, Patiend_name, Vaccine_name, Vaccine_description, Date, Next_dose_date_time, Clinic_name));

                        }//while
                        connection.Close();
                    }
                }
            }

            return View(temp);
        }

    }
}
