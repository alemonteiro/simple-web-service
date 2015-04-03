using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alcode.SimpleWebService
{
    /// <summary>
    /// Simple service that converts ServiceParams to T
    /// </summary>
    /// <typeparam name="T">Request Param Type</typeparam>
    public class SimpleServiceGeneric<T> : SimpleWebServiceInterface
    {
        /// <summary>
        /// Convert the SimpleServiceParams to a object of type T, or creates a new instance in case the params are null
        /// </summary>
        /// <param name="param">SimpleServiceParams</param>
        /// <returns>Object of Type T</returns>
        protected override object ParseParam(SimpleWebServiceParams param)
        {
            T obj = param.asObject<T>();
            if (obj == null) 
                obj = (T)Activator.CreateInstance(typeof(T));
            return obj;
        }
    }
}
