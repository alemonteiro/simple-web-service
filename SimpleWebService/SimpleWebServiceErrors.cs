using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alcode.SimpleWebService
{
    public enum SimpleWebServiceErrors
    {
        MethodNotFound = 1,
        InvalidArguments = 2,
        MethodInvalid = 3,
        InternalServiceError = 4,
        MissingArgument = 5,
        MethodAccessDenied = 6,
        Uknown = 99
    }
}
