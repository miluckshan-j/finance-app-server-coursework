using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PredictionServer
{
    /// <summary>
    /// Summary description for PredictionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PredictionService : System.Web.Services.WebService
    {

        SQLiteConnection con = new SQLiteConnection(@"Data Source=C:\Users\miluckshan.j_niftron\Desktop\cw2.db;", true);
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public DataTable RetrievePredictions(String start, String end)
        {
            DataTable datatable = new DataTable("predictions");

            // Retrieve Transactions
            con.Open();
            adapter = new SQLiteDataAdapter("SELECT `type`, COUNT (`id`) as 'count', SUM(`amount`) as 'amount' FROM transactions WHERE `date` > '" + start + "' AND `date` < '" + end + "' GROUP BY `type` ORDER BY `type` DESC", con);

            adapter.Fill(datatable);

            return datatable;
        }
    }
}
