using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using WebApplication2.Models;
using System.Web.Security;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        public string status;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ToString());
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Enroll e)
        {
            con.Open();
            string query = "select Email,Password from Enrollment where Email=@Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Email", e.Email);
            cmd.Parameters.AddWithValue("@Password", e.Password);
            SqlDataReader sdr = cmd.ExecuteReader();
            if(sdr.Read())
            {
                Session["Email"] = e.Email.ToString();
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewData["message"] = "User Login Details Failed!";
            }
            if(e.Email.ToString()!=null)
            {
                Session["Email"] = e.Email.ToString();
                status = "1";
            }
            else
            {
                status = "3";
            }
            con.Close();
            return View();
        }
        [HttpGet]
        public ActionResult Welcome(Enroll e)
        {
            DataSet ds = new DataSet();
            Enroll user = new Enroll();
            con.Open();
            SqlCommand cmd = new SqlCommand("proc_GetEnrollmentdetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 30).Value = Session["Email"].ToString();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(ds);
            List<Enroll> userlist = new List<Enroll>();
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                Enroll uobj = new Enroll();
                uobj.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                uobj.FirstName=ds.Tables[0].Rows[i]["Firstname"].ToString();
                uobj.LastName = ds.Tables[0].Rows[i]["secondname"].ToString();
                uobj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                uobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                uobj.PhoneNumber = ds.Tables[0].Rows[i]["Phone"].ToString();
                uobj.Gender = ds.Tables[0].Rows[i]["Gender"].ToString();
                userlist.Add(uobj);
            }
            user.Enrollsinfo = userlist;
            con.Close();
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","User");
        }
    }
}