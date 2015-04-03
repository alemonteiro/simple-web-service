using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using System.Reflection;
using System.Data;
using System.IO;
using System.Web.UI;

namespace Alcode.SimpleWebService
{
    /// <summary>
    /// Base class for Simple Web Services
    /// </summary>
    public class SimpleWebServiceInterface : Page
    {
        /// <summary>
        /// Request completed successfull
        /// </summary>
        public static int Success = 0;
        /// <summary>
        /// Default error code for not treated exceptions
        /// </summary>
        public static int Fail = -1;

        /// <summary>
        /// Called before processing each request
        /// </summary>
        protected virtual void Setup()
        {
            
        }

        /// <summary>
        /// Called before processing each request
        /// </summary>
        /// <returns>Return null for a valid request or a ServiceError object</returns>
        protected virtual SimpleWebServiceError ValidadeRequest()
        {
            return null;
        }

        /// <summary>
        /// Format for parsing date time objects
        /// </summary>
        protected IsoDateTimeConverter isoDate = new IsoDateTimeConverter()
        {
            DateTimeFormat = Alcode.SimpleWebService.Properties.Settings.Default.DateFormat
        };
        
        /// <summary>
        /// Serialize the result object
        /// </summary>
        /// <param name="result">Result Object</param>
        /// <returns>JSON String</returns>
        protected virtual string SerializeResult(object result)
        {
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Converters = new List<JsonConverter>() { isoDate },
                MaxDepth = 32
            });
        }

        /// <summary>
        /// Parses the Request Params to a SimpleServiceParams object
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>SimpleServiceParams</returns>
        protected virtual SimpleWebServiceParams GetParams(HttpRequest request)
        {
            String method = request.Params["m"].ToString();
            String stringRequest = Uri.UnescapeDataString(request.Params["e"].ToString());
            SimpleWebServiceParams ap = new SimpleWebServiceParams(method, stringRequest);
            return ap;
        }

        /// <summary>
        /// Parse an ServiceParams as an object
        /// </summary>
        /// <param name="param">ServiceParams</param>
        /// <returns>Same ServiceParams or custom object</returns>
        /// <remarks>This method must be override for custom objects</remarks>
        protected virtual object ParseParam(SimpleWebServiceParams param)
        {
            return param;
        }

        /// <summary>
        /// Process the request
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Setup the service
            this.Setup();

            //Validate the request
            SimpleWebServiceError err = ValidadeRequest();
            
            if (err != null)
            {
                SimpleServiceResponse<string> ret = new SimpleServiceResponse<string>(err);

                string result = JsonConvert.SerializeObject(ret);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();

                Response.Write(result);
                return;
            }

            // Parse the request query string 
            String escaped = Request.Params["e"].ToString();
            escaped = Uri.UnescapeDataString(escaped);
            
            string strResult = "";
            string strError = "";
            int status = Fail;
            Exception exp = null;
            SimpleWebServiceParams ap = GetParams(Request);

            try
            {

                //Parse the param
                object objParam = ParseParam(ap);

                //Call the requested method
                object result = this.GetType()
                    .InvokeMember(ap.Method, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, this, new object[] { objParam });

                if (result.GetType() != typeof(SimpleServiceResponse<>))
                {
                    result = new SimpleServiceResponse<object>(result);
                }

                strResult = SerializeResult(result);
            }
            catch (SimpleWebServiceException ex)
            {
                status = ex.ErrorCode;
                strError = ex.Message;
                exp = ex;
            }
            catch (MethodAccessException ex)
            {
                status = (int)SimpleWebServiceErrors.MethodAccessDenied;
                strError = Resources.MethodAccessDenied;
                exp = ex;
            }
            catch (MissingMethodException ex)
            {
                status = (int)SimpleWebServiceErrors.MethodNotFound;
                strError = Resources.MethodNotFound;
                exp = ex;
            }
            catch (ArgumentException ex)
            {
                status = (int)SimpleWebServiceErrors.InvalidArguments;
                strError = Resources.InvalidArguments;
                exp = ex;
            }
            catch (Exception ex)
            {
                status = (int)SimpleWebServiceErrors.InternalServiceError;
                strError = Resources.InternalServiceError;
                exp = ex;
            }

            if (strError != "")
            {
                SimpleServiceResponse<string> ret = 
                    new SimpleServiceResponse<string>(new SimpleWebServiceError(status, ap.Method, strError,
                        Alcode.SimpleWebService.Properties.Settings.Default.WriteExceptionDetails ? exp : null));

                strResult = SerializeResult(ret);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            
            Response.Write(strResult);
        }
    }

}
