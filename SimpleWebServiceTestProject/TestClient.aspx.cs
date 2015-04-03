using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Alcode.SimpleWebService;

namespace Alcode.SimpleWebServiceTestProject
{
    /// <summary>
    /// Test page for requesting method to a remote SimpleWebService
    /// Normally it would be a service in a remote server or in another project at least
    /// To handle parsing complex types just share the Model Library for the service.
    /// </summary>                              
    public partial class TestClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "http://localhost/SimpleServiceTest/Ajax.aspx";

            Response.Write(
                SimpleWebServiceClient.Request(url, "Hello", new { Name = "Teste" }).Result.ToString()
            );
            Response.Write("<br/>");
            // OR

            SimpleWebServiceClient ssc = new SimpleWebServiceClient(url);
            
            Response.Write(
                ssc.Request("AddNumbers", new { x = 12, y = 10 }).Result.ToString()
            );
            Response.Write("<br/>");

            Response.Write(
                "Jobs Found: " + ssc.Request<List<TestJob>>("GetJobs").Result.Count
            );
        }
    }
}