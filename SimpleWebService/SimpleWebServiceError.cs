﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;

namespace Alcode.SimpleWebService
{
    /// <summary>
    /// Error generated by Service Interface
    /// </summary>
    public class SimpleWebServiceError
    {

        #region Construtores
        public SimpleWebServiceError(string method, string message, Exception ex = null)
        {
            _set(null, method, message, null, ex);
        }

        public SimpleWebServiceError(int errorCode, string method, Exception ex = null)
        {
            _set(errorCode, method, null, null, ex);
        }

        public SimpleWebServiceError(int errorCode, string method, string message)
        {
            _set(errorCode, method, message, null, null);
        }

        public SimpleWebServiceError(int errorCode, string method, string message, string details)
        {
            _set(errorCode, method, message, details, null);
        }

        public SimpleWebServiceError(int errorCode, string method, string message, Exception ex)
        {
            _set(errorCode, method, message, null, ex);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Set private variables
        /// </summary>
        /// <param name="errorCode">Error Code</param>
        /// <param name="method">Requested Method</param>
        /// <param name="message">Error Message</param>
        /// <param name="details">Error Details</param>
        /// <param name="ex">Exception</param>
        /// <remarks>If a exception is passed, this.Details will have the exception stack trace</remarks>
        private void _set(int? errorCode, string method, string message, string details, Exception ex)
        {
            this._code = errorCode == null ? (int)SimpleWebServiceErrors.Uknown : errorCode.Value;
            
            this._method = method;
            this._message = message;
            this._details = details;

            if (ex != null)
            {
                this._details = ex.ToString();
                if (message == null || message == string.Empty)
                {
                    this._message = ex.Message;
                }
            }
        }

        #endregion

        #region Propriedades

        private int _code = 0;
        private string _message = String.Empty;
        private string _details = String.Empty;
        private string _method = String.Empty;

        /// <summary>
        /// Error Code
        /// </summary>
        public int Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Error Details
        /// </summary>
        public string Details
        {
            get { return _details; }
        }

        /// <summary>
        /// Method Requested
        /// </summary>
        public string Method
        {
            get { return _method; }
        }

        #endregion

    }
}
