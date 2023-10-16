using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TransactionServer
{
    /// <summary>
    /// Summary description for TransactionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TransactionService : System.Web.Services.WebService
    {
        SQLiteConnection con = new SQLiteConnection(@"Data Source=C:\Users\miluckshan.j_niftron\Desktop\cw2.db;", true);
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;

        //[WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public bool InsertTransaction(String type, Double amount, String date)
        {
            int rows;

            // Insert Transaction 
            cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "INSERT INTO transactions (type, amount, date) VALUES (@type, @amount, @date)";
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@date", date);
            rows = cmd.ExecuteNonQuery();
            con.Close();

            return (rows > 0) ? true : false;

        }

        [WebMethod]
        public bool UpdateTransaction(int id, double amount)
        {
            int rows;

            // Update Transaction 
            cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "UPDATE transactions SET amount=@amount WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@amount", amount);
            rows = cmd.ExecuteNonQuery();
            con.Close();

            return (rows > 0) ? true : false;
        }

        [WebMethod]
        public DataTable RetrieveTransactions()
        {
            DataTable datatable = new DataTable("transactions");

            // Retrieve Transactions
            con.Open();
            adapter = new SQLiteDataAdapter("SELECT * FROM transactions", con);

            // TODO: Handle empty table scenario
            adapter.Fill(datatable);

            //datatable.WriteXml("transactions.xml");

            //var xmlString = ConvertDatatableToXML(datatable);

            //return xmlString;
            return datatable;
        }

        [WebMethod]
        public bool DeleteTransaction(int id)
        {
            int rows;

            // Delete Transaction 
            cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "DELETE FROM transactions WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            rows = cmd.ExecuteNonQuery();
            con.Close();

            return (rows > 0) ? true : false;
        }

        [WebMethod]
        public DataTable RetrieveWeeklySummary()
        {
            // Calculate first and last day of week
            DateTime baseDate = DateTime.Today;
            var weekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var weekEnd = weekStart.AddDays(7).AddSeconds(-1);

            DataTable datatable = new DataTable("weeklyTransactions");

            // Retrieve Weekly Transactions Summary
            con.Open();
            string query = "SELECT type, SUM(amount) AS amount FROM transactions WHERE date BETWEEN '" + weekStart.ToString("yyyy-MM-dd") + "' AND '" + weekEnd.ToString("yyyy-MM-dd") + "' GROUP BY type";
            adapter = new SQLiteDataAdapter(query, con);

            // TODO: Handle empty table scenario
            adapter.Fill(datatable);

            con.Close();

            return datatable;

        }

        public string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
    }
}
