using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BankServices.Api.Models;
using System.Net.Mail;

namespace BankServices.Api.Helpers
{
    public class AdminDBConnection
    {
        public static string AdminFname { get; set; }
        public static string AdminLname { get; set; }
        public static string AdminEmail { get; set; }
        private static string Code = "Test123";
        private static string Pwd = "@@__nathan@@";


        SqlConnection con = new SqlConnection(@"Data Source=ALEX-OVO-NATHAN\SQLSERVER2017DEV;Initial Catalog=Bank__DB;Integrated Security=True");

        //Admin signup
        public static string AdminSignUpDB(AdminInfo adminDetails)
        {
            AdminFname = adminDetails.AdminFName;
            AdminLname = adminDetails.AdminLName;
            AdminEmail = adminDetails.AdminEmail;
            DateTime current = DateTime.Now;

            SqlConnection con = new SqlConnection(@"Data Source=ALEX-OVO-NATHAN\SQLSERVER2017DEV;Initial Catalog=Bank__DB;Integrated Security=True");

            SqlCommand cmd = new SqlCommand();

            try
            {

                if (AdminFname == "" || AdminLname == "")
                {
                    throw new Exception();
                }

                if (AdminEmail == "")
                {
                    throw new Exception();
                }

                if (!AdminEmail.Contains("@gmail.com"))
                {
                    throw new Exception();
                }
                cmd.Parameters.Clear();
                cmd.Connection = con;
                cmd.CommandText = @"INSERT INTO Admin(AdminFirstName, AdminLastName, AdminEmail, Created_at) VALUES(@fname, @lname, @email, @created)";

                cmd.Parameters.AddWithValue("@fname", AdminFname);
                cmd.Parameters.AddWithValue("@lname", AdminLname);
                cmd.Parameters.AddWithValue("@email", AdminEmail);
                cmd.Parameters.AddWithValue("@created", current);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("aov.nathan@gmail.com");
                    mail.To.Add(AdminEmail);
                    mail.Subject = "Admin Info";
                    mail.Body = $"Your Access code: {Code}";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("aov.nathan@gmail.com", Pwd);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    return $"Failed msg sending why => {ex.Message}";
                }

                return $"Sucessfull Signup";

            }
            catch (Exception ex)
            {
                return $"Failed why => {ex.Message}";
            }

           
        }

      
    }
}
