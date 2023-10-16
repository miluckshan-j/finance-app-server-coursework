using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace UserManagementServer
{
    /// <summary>
    /// Summary description for UserManagementService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserManagementService : System.Web.Services.WebService
    {
        SQLiteConnection con = new SQLiteConnection(@"Data Source=C:\Users\miluckshan.j_niftron\Desktop\cw2.db;", true);
        SQLiteCommand cmd;

        //[WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public bool registerUser(String username, String password)
        {

            int rows;

            // Insert User 
            cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "INSERT INTO users (username, password) VALUES(@username, @password)";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            rows = cmd.ExecuteNonQuery();
            con.Close();

            return (rows > 0) ? true : false;

        }

        [WebMethod]
        public int loginUser(String username, String password)
        {

            // Check User 
            cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "SELECT COUNT(*) FROM users WHERE username=@username AND password=@password";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            var response = cmd.ExecuteScalar();

            con.Close();

            return Convert.ToInt32(response);

        }
    }
}
