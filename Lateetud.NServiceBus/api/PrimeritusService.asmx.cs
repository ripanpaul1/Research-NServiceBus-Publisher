using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.Services;

namespace Lateetud.NServiceBus.api
{
    /// <summary>
    /// Summary description for PrimeritusService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PrimeritusService : System.Web.Services.WebService
    {

        [WebMethod]
        public string ExcelDataExtraction()
        {
            return ReadExcel(Server.MapPath("~\\" + ConfigurationManager.AppSettings["ExcelFile"]));
        } 

        public string ReadExcel(string file)
        {
            #region ConfigueConnection
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (Path.GetExtension(file).CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007 
            #endregion

            string strdata = "";
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    new OleDbDataAdapter("select * from [Sheet1$]", con).Fill(dtexcel);
                    for (int row = 1; row < dtexcel.Rows.Count; row++)
                    {
                        foreach (DataColumn col in dtexcel.Columns)
                        {
                            if (strdata.Trim().Length == 0) strdata += dtexcel.Rows[0][col] + ": " + dtexcel.Rows[row][col];
                            else strdata += "|" + dtexcel.Rows[0][col] + ": " + dtexcel.Rows[row][col];
                        }
                    }
                }
                catch(Exception err)
                {
                    strdata = "error";
                }
            }
            return strdata;
        }

        public string ReadExcel_by_3rdPartyDll(string file)
        {
            string str = "";
            foreach (var worksheet in Excel.Workbook.Worksheets(file))
            {
                foreach (var row in worksheet.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (str.Trim().Length == 0) str += cell.Text;
                        else str += "|" + cell.Text;
                    }
                }
            }
            return str;
        }


    }
}
