using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alcode.SimpleWebService
{
    public class SimpleWebServiceException : Exception
    {
        private int _errorCode;
        public int ErrorCode { get { return _errorCode; } }

        public SimpleWebServiceException(SimpleWebServiceErrors errorCode)
            : base()
        {
            this._errorCode = (int)errorCode;
        }

        public SimpleWebServiceException(SimpleWebServiceErrors errorCode, string message)
            : base(message)
        {
            this._errorCode = (int)errorCode;
        }
        public SimpleWebServiceException(SimpleWebServiceErrors errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this._errorCode = (int)errorCode;
        }

        public SimpleWebServiceException(int errorCode)
            : base()
        {
            this._errorCode = errorCode;
        }

        public SimpleWebServiceException(int errorCode, string message)
            :base(message)
        {
            this._errorCode = errorCode;
        }

        public SimpleWebServiceException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this._errorCode = errorCode;
        }

    }
}
