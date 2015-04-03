using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Alcode.SimpleWebService
{
    /// <summary>
    /// Response from a Simple Service
    /// </summary>
    /// <typeparam name="T">Type of Result</typeparam>
    public class SimpleServiceResponse<T>
    {
        private T _result;
        private int _status;
        private SimpleWebServiceError _error;

        /// <summary>
        /// Result
        /// </summary>
        public T Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Status
        /// </summary>
        public int Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Error
        /// </summary>
        public SimpleWebServiceError Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result</param>
        [JsonConstructor]
        public SimpleServiceResponse(T Result, int Status, SimpleWebServiceError Error)
        {
            this._result = Result;
            this._status = Status;
            this._error = Error;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result</param>
        public SimpleServiceResponse(T result)
        {
            this._status = SimpleWebServiceInterface.Success;
            _result = result;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result">Result</param>
        public SimpleServiceResponse(object result)
        {
            this._status = SimpleWebServiceInterface.Success;
            _result = (T)result;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">Error</param>
        public SimpleServiceResponse(SimpleWebServiceError error)
        {
            this._status = error.Code;
            _error = error;
        }

        /// <summary>
        /// Constutor for Errors
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="erroMetodo">Method</param>
        /// <param name="message">Message</param>
        /// <param name="details">Details</param>
        public SimpleServiceResponse(int errorCode, string method, string message, string details)
        {
            this._status = errorCode;
            _error = new SimpleWebServiceError(errorCode, method, message, details);
        }
        /// <summary>
        /// Constutor for Errors
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="erroMetodo">Method</param>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception</param>
        public SimpleServiceResponse(int errorCode, string method, string message, Exception ex)
        {
            this._status = errorCode;
            _error = new SimpleWebServiceError(errorCode, method, message, ex);
        }
        /// <summary>
        /// Constutor for Errors
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="erroMetodo">Method</param>
        /// <param name="message">Message</param>
        public SimpleServiceResponse(int errorCode, string method, string message)
        {
            this._status = errorCode;
            _error = new SimpleWebServiceError(errorCode, method, message);
        }

        /// <summary>
        /// Constutor for Errors
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="erroMetodo">Method</param>
        public SimpleServiceResponse(int errorCode, string method)
        {
            this._status = errorCode;
            _error = new SimpleWebServiceError(errorCode, method);
        }
        /// <summary>
        /// Constutor for Errors
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="erroMetodo">Method</param>
        /// <param name="message">Message</param>
        /// <param name="ex">Exception</param>
        public SimpleServiceResponse(int errorCode, string method, Exception ex)
        {
            this._status = errorCode;
            _error = new SimpleWebServiceError(errorCode, method, ex);
        }
    }
}
