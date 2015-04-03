using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Alcode.SimpleWebService
{
    /// <summary>
    /// Client for calling Simple Services
    /// </summary>
    public class SimpleWebServiceClient
    {
        private string _url;
        /// <summary>
        /// Service URL
        /// </summary>
        /// <example>http://192.168.1.5/MyService/Service.aspx</example>
        public string URL { get { return _url; } }

        /// <summary>
        /// Constructor for making mutiples request to the same URL
        /// </summary>
        /// <param name="url"></param>
        public SimpleWebServiceClient(string url)
        {
            this._url = url;
        }

        #region Public Methods
        public SimpleServiceResponse<object> Request(string method, object data = null)
        {
            return Request<object>(method, data);
        }

        public SimpleServiceResponse<T> Request<T>(string method, object data = null)
        {
            return SimpleWebServiceClient.Request<T>(this.URL, method, data);
        }
        #endregion

        #region Static Methods
        public static SimpleServiceResponse<object> Request(string url, string method, object data = null)
        {
            return Request<object>(url, method, data);
        }

        public static SimpleServiceResponse<T> Request<T>(string url, string method, object data = null)
        {
            String strJSON = Uri.EscapeDataString(JsonConvert.SerializeObject(data));

            String strQuery = String.Format("m={0}&e={1}&s={2}", method, strJSON, getMilliTimeStampgetMilliTimeStamp());

            if (url.IndexOf("?") > 0)
            {
                url += "&" + strQuery;
            }
            else
            {
                url += "?" + strQuery;
            }

            string result = RequestHttp(url, "GET");
            
            return (SimpleServiceResponse<T>)JsonConvert.DeserializeObject<SimpleServiceResponse<T>>(result);
        }

        #region Utils
        /// <summary>
        /// Same as Date.getTime() on JavaScript
        /// </summary>
        /// <returns>Milliseconds from 01/01/1970</returns>
        private static string getMilliTimeStampgetMilliTimeStamp()
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = DateTime.UtcNow;
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// Request HTTP and return content string
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="httpMethod">Method(GET/POST)</param>
        /// <param name="postData">Arguments</param>
        /// <returns>HTTP Response</returns>
        private static string RequestHttp(string url, string httpMethod, string strEnconding = "iso-8859-1")
        {
            string pstrHTTPCapturado = "";

            System.Net.HttpWebRequest objHTTPRequest;
            System.Net.HttpWebResponse objHTTPResponse;

            Uri objResponseUri;
            Uri objRequestUri;

            System.IO.StreamReader objBodyReader;
            System.IO.Stream objResponseStream;

            string strBodyText = "";

            objRequestUri = new Uri(url);

            objHTTPRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(objRequestUri);
            objHTTPRequest.AllowAutoRedirect = false;
            objHTTPRequest.MaximumAutomaticRedirections = 10;
            objHTTPRequest.Method = httpMethod;

            objHTTPRequest.ProtocolVersion = new Version(1, 0);
            objHTTPRequest.Timeout = Convert.ToInt32((new TimeSpan(0, 0, 60)).TotalMilliseconds);
            objHTTPRequest.AllowWriteStreamBuffering = false;
            
            objHTTPResponse = (System.Net.HttpWebResponse)objHTTPRequest.GetResponse();

            objResponseUri = objHTTPResponse.ResponseUri;

            objResponseStream = objHTTPResponse.GetResponseStream();

            if (objResponseStream != null)
            {

                objBodyReader = new System.IO.StreamReader(objResponseStream, System.Text.Encoding.GetEncoding(objHTTPResponse.CharacterSet));

                strBodyText += objBodyReader.ReadToEnd();

                objBodyReader.Close();

                objBodyReader = null;
            }

            objHTTPResponse.Close();

            objHTTPResponse = null;
            objHTTPRequest = null;

            pstrHTTPCapturado = strBodyText;

            return pstrHTTPCapturado;
        }
        #endregion

        #endregion

    }
}
