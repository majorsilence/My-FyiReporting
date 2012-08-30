using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace ReportServer
{
    public partial class Reports : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();

            try
            {


                string sql = "SELECT a.reportname, a.tag, c.description FROM reportfiles a JOIN roleaccess b ON a.tag=b.tag JOIN roletags c ON a.tag = c.tag WHERE b.role = @roleid ;";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = new SQLiteConnection(Code.DAL.ConnectionString);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.Add("@roleid", DbType.String).Value = SessionVariables.LoggedRoleId;

                DataTable dt = Code.DAL.ExecuteCmdTable(cmd);

                if (dt.Rows.Count == 0)
                {
                    return;
                }


                sb.Append("<table><tr><td>Report Name</td><td>Description</td></tr>");

               

                
                foreach (DataRow row in dt.Rows)
                {
                    string path =  System.IO.Path.Combine(SessionVariables.ReportDirectory, row["reportname"].ToString());
                    string filename = System.IO.Path.GetFileNameWithoutExtension(row["reportname"].ToString());
                    string description =  row["description"].ToString();

                    sb.Append(string.Format("<tr><td><a href=\"ShowReport.aspx?rs:url={0}\" target=_self>{1}</a></td><td>{2}</td></tr>",  path, filename, description ));

                }

                sb.Append("</table>");

                Literal1.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                LabelError.Text = ex.Message;
            }




 
        }
    }
}