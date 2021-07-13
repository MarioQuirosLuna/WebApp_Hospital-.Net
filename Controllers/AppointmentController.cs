﻿using Microsoft.AspNetCore.Mvc;
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
    public class AppointmentController : Controller
    {
        public IConfiguration Configuration { get; }

        public AppointmentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            List<Appointment> appointments = new List<Appointment>();

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getAppointmentByPatientId @id ='{id}'";
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
                            int DoctorIdentification = Int32.Parse(dataReader["doctor_code"].ToString());
                            string Doctor_name = dataReader["doctor_name"].ToString();
                            string Clinic_name = dataReader["clinic_name"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            string Speciality = dataReader["speciality"].ToString();
                            string Diagnosis = dataReader["diagnosis_description"].ToString();

                            appointments.Add(new Appointment(Id, PatientIdentification, Patiend_name, DoctorIdentification, Doctor_name, Clinic_name, Date, Speciality, Diagnosis));
                        }//while
                        connection.Close();
                    }
                }
            }

            ViewBag.IdPatient = id;

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {

            //TODO: Validate not found

            Appointment temp = null;

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getAppointmentById @id='{id}'";
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
                            int DoctorIdentification = Int32.Parse(dataReader["doctor_code"].ToString());
                            string Doctor_name = dataReader["doctor_name"].ToString();
                            string Clinic_name = dataReader["clinic_name"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            string Speciality = dataReader["speciality"].ToString();
                            string Diagnosis = dataReader["diagnosis_description"].ToString();

                            temp = new Appointment(Id, PatientIdentification, Patiend_name, DoctorIdentification, Doctor_name, Clinic_name, Date, Speciality, Diagnosis);
                        }//while
                        connection.Close();
                    }
                }
            }

            return View(temp);
        }

        // GET: Appointments/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {

            //TODO: Validate not found

            Appointment temp = null;

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getPatientById @id='{id}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dataReader = await command.ExecuteReaderAsync();
                        while (dataReader.Read())
                        {
                            int PatientIdentification = Int32.Parse(dataReader["patient_identification_card"].ToString());
                            string Patiend_name = dataReader["patient_name"].ToString();

                            temp = new Appointment(PatientIdentification, Patiend_name);
                        }//while
                        connection.Close();
                    }
                }
            }

            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            string dateCreate="";
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);

                dateCreate = appointment.Year+"-"+appointment.Day+"-"+appointment.Month+ " "+appointment.Hour+":00:00";

                string sqlQuery = $"exec addAppointment @patient={appointment.Fk_patient_identification_card}, @doctor={appointment.Doctor_code}, @clinic='{appointment.Clinic_name}', @speciality='{appointment.Speciality}', @date='{dateCreate}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    await command.ExecuteReaderAsync();
                    connection.Close();
                }
            }

            ViewBag.Mensaje = "Successful";

            return RedirectToAction("Index", "Patient");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            //TODO: Validate not found

            Appointment temp = null;

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getAppointmentById @id='{id}'";
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
                            int DoctorIdentification = Int32.Parse(dataReader["doctor_code"].ToString());
                            string Doctor_name = dataReader["doctor_name"].ToString();
                            string Clinic_name = dataReader["clinic_name"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            string Speciality = dataReader["speciality"].ToString();
                            string Diagnosis = dataReader["diagnosis_description"].ToString();

                            temp = new Appointment(Id, PatientIdentification, Patiend_name, DoctorIdentification, Doctor_name, Clinic_name, Date, Speciality, Diagnosis);
                        }//while
                        connection.Close();
                    }
                }
            }

            return View(temp);
        }

        //[HttpPut]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Appointment appointment) 
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);

                string sqlQuery = $"exec updateDiagnostic @appointment='{appointment.Id}', @description='{appointment.Diagnosis}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    await command.ExecuteReaderAsync();
                    connection.Close();
                }
            }

            ViewBag.Mensaje = "Successful";

            return RedirectToAction("Index", "Patient");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) 
        {
            //TODO: Validate not found
            Appointment temp = null;

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec getAppointmentById @id='{id}'";
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
                            int DoctorIdentification = Int32.Parse(dataReader["doctor_code"].ToString());
                            string Doctor_name = dataReader["doctor_name"].ToString();
                            string Clinic_name = dataReader["clinic_name"].ToString();
                            DateTime Date = (DateTime)dataReader["date_time"];
                            string Speciality = dataReader["speciality"].ToString();
                            string Diagnosis = dataReader["diagnosis_description"].ToString();

                            temp = new Appointment(Id, PatientIdentification, Patiend_name, DoctorIdentification, Doctor_name, Clinic_name, Date, Speciality, Diagnosis);
                        }//while
                        connection.Close();
                    }
                }
            }

            return View(temp);
        }

        //[HttpDelete]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Appointment appointment) 
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);

                string sqlQuery = $"exec deleteAppointment @appointment='{appointment.Id}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    await command.ExecuteReaderAsync();
                    connection.Close();
                }
            }

            ViewBag.Mensaje = "Successful";

            return RedirectToAction("Index", "Patient");
        }
    }
}
