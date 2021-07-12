using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace WebApplication2.Controllers
{
    public class EnrollmentController : Controller
    {
        public string value = "";
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Enroll e)
        {

            Enroll er = new Enroll();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ToString());
            
            SqlCommand cmd = new SqlCommand("proc_EnrollDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
            cmd.Parameters.AddWithValue("@LastName", e.LastName);
            cmd.Parameters.AddWithValue("@Password", e.Password);
            cmd.Parameters.AddWithValue("@Email", e.Email);
            cmd.Parameters.AddWithValue("@Phone", e.PhoneNumber);
            cmd.Parameters.AddWithValue("@Gender", e.Gender);
            //cmd.Parameters.AddWithValue("@status", "INSERT");
            con.Open();
            ViewData["result"]=cmd.ExecuteNonQuery();
            return View();
        }
    }
}