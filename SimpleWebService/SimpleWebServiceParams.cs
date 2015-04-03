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
    /// Arguments for Simple Service
    /// </summary>
    public class SimpleWebServiceParams
    {

        #region Constructors
        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="properties">Dictionary with arguments</param>
        public SimpleWebServiceParams(Dictionary<string, object> properties)
        {
            this.properties = properties;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="method">Requested Method</param>
        /// <param name="dataString">Requested Data String</param>
        public SimpleWebServiceParams(String method, string dataString)
        {
            this._method = method;
            this.stringRequest = dataString;
        }
        #endregion

        #region Propriedades
        protected string _method;
        /// <summary>
        /// Requested Method
        /// </summary>
        public string Method { get { return _method; } }

        /// <summary>
        /// Arguments as JSON Escaped String
        /// </summary>
        protected string stringRequest { get; set; }

        /// <summary>
        /// Properties
        /// </summary>
        protected Dictionary<string, object> properties { get; set; }

        /// <summary>
        /// Gets or Sets if we should check each parameter and throw an exception in case one of them don't exist
        /// </summary>
        public bool CheckProperties { get; set; }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Check if the property name exists in the arguments list
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>True or False</returns>
        public bool HasProperty(string name)
        {
            return this.getProperties().ContainsKey(name);
        }

        /// <summary>
        /// Check if a property exists. If it does not exists, throws an exception
        /// </summary>
        /// <param name="name">Property name</param>
        public bool CheckProperty(string name)
        {
            if (!HasProperty(name))
            {
                throw new SimpleWebServiceException(SimpleWebServiceErrors.MissingArgument, "Argument: " + name);
            }
            return true;
        }

        /// <summary>
        /// Get an property
        /// </summary>
        /// <param name="name">Property Name</param>
        /// <returns>Object if property exists, null otherwise. Throws exception if CheckProperties is set to True and the property doesn't exist.</returns>
        public object get(string name, object defaultValue = null)
        {
            if (CheckProperties) CheckProperty(name);
            else if (HasProperty(name)) return getProperties()[name];
            else if (defaultValue != null) return defaultValue;
            return null;
        }

        /// <summary>
        /// Convert the escaped string to an Object
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <returns>Objeto of type T</returns>
        public T asObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(stringRequest, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
        }

        /// <summary>
        /// Returns a Dictionary with the indexed properties
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<string, object> getProperties()
        {
            if (this.properties != null)
            {
                return this.properties;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (stringRequest == null || stringRequest == String.Empty || stringRequest.Trim() == "null") return dic;

            object obj = JsonConvert.DeserializeObject(stringRequest);

            Newtonsoft.Json.Linq.JObject o = (Newtonsoft.Json.Linq.JObject)obj;

            foreach (JProperty prop in o.Properties())
            {
                dic.Add(prop.Name, prop.Value.ToString());
            }

            this.properties = dic;

            return dic;
        }

        /// <summary>
        /// Gets a property as T
        /// </summary>
        /// <typeparam name="T">Type of Property</typeparam>
        /// <param name="name">Name of Property</param>
        /// <returns>Property Value</returns>
        public T getProperty<T>(string name)
        {
            if (this.properties == null)
                this.getProperties();

            if (this.properties.ContainsKey(name))
            {
                return (T)this.properties[name];
            }
            return default(T);
        }

        /// <summary>
        /// Gets a property as String
        /// </summary>
        /// <param name="name">Name of Property</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Property Value or Default Value</returns>
        public string getString(string name, string defaultValue = null)
        {
            object s = get(name, defaultValue);
            return s == null ? null : s.ToString();
        }

        /// <summary>
        /// Gets a property as String[]
        /// </summary>
        /// <param name="name">Name of Property</param>
        /// <returns>String Array</returns>
        public string[] getStringArray(string name)
        {
            if (CheckProperties) CheckProperty(name);
            if (!HasProperty(name))
            {
                return new string[] { };
            }
            return getObject<string[]>(name);
        }

        /// <summary>
        /// Gets an Int
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="defaultValue">Default value in case the property doesn't exists.</param>
        /// <returns>int</returns>
        public int getInt(string name, int? defaultValue = null)
        {
            if (CheckProperties) CheckProperty(name);
            if (defaultValue != null && !HasProperty(name))
            {
                return defaultValue.Value;
            }
            return int.Parse(this.getProperties()[name].ToString());
        }

        /// <summary>
        /// Gets an Int?
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>int?</returns>
        public int? getIntNull(string name)
        {
            if (HasProperty(name))
            {
                object value = this.getProperties()[name];
                if (value == null || value.ToString().Trim() == "")
                {
                    return null;
                }
                return int.Parse(value.ToString());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an long?
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>long?</returns>
        public long? getLongNull(string name)
        {
            if (HasProperty(name))
            {
                object value = this.getProperties()[name];
                if (value == null || value.ToString().Trim() == "")
                {
                    return null;
                }
                return long.Parse(value.ToString());
            }
            else
            {
                return null;
            }
        }
                
        /// <summary>
        /// Gets an Long
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="defaultValue">Default value in case the property doesn't exists.</param>
        /// <returns>long</returns>
        public long getLong(string name, long? defaultValue = null)
        {
            object l = get(name, defaultValue);
            return long.Parse(l.ToString());
        }

        /// <summary>
        /// Gets an Boolean
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="defaultValue">Default value in case the property doesn't exists.</param>
        /// <returns>bool</returns>
        public bool getBool(string name, bool defaultValue = false)
        {
            return bool.Parse(get(name, defaultValue).ToString());
        }

        /// <summary>
        /// Gets an Boolean?
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>bool?</returns>
        public bool? getBoolNull(string name)
        {
            object value = get(name);

            if (value == null || value.ToString().Trim() == "")
            {
                return null;
            }
            else if (value.ToString().Length == 1)
            {
                return Convert.ToBoolean(int.Parse(value.ToString()));
            }
            return Convert.ToBoolean(value.ToString());

        }

        /// <summary>
        /// Gets an Float?
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>float?</returns>
        public float? getFloatNull(string name)
        {
            object o = get(name);
            if (o == null) return null;
            
            return float.Parse(o.ToString());
        }
        
        /// <summary>
        /// Gets an DateTime
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="defaultValue">Default value in case the property doesn't exists.</param>
        /// <returns>DateTime</returns>
        /// <remarks>Returns new DateTime() if property not found, defaultValue equals null and CheckProperties set to false</remarks>
        public DateTime getDate(string name, DateTime? defaultValue = null)
        {
            object o = get(name, defaultValue);
            
            if (o == null) return new DateTime();

            if (o.GetType().Equals(typeof(DateTime))) return (DateTime)o;

            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(o.ToString());
            }
            catch
            {
                dt = (DateTime)getObject<DateTime>(name);
            }
            return dt;
        }

        /// <summary>
        /// Gets an DateTime?
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="defaultValue">Default value in case the property doesn't exists.</param>
        /// <returns>DateTime?</returns>
        public DateTime? getDateNull(string name, DateTime? defaultValue = null)
        {
            object o = get(name, defaultValue);

            if (o == null || o.GetType().Equals(typeof(DateTime))) return (DateTime)o;

            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(o.ToString());
            }
            catch
            {
                dt = (DateTime)getObject<DateTime>(name);
            }
            return dt;
        }

        /// <summary>
        /// Gets an object of type T
        /// </summary>
        /// <typeparam name="T">Type of Object</typeparam>
        /// <param name="name">Property Name</param>
        /// <returns>Object</returns>
        /// <remarks>If property not found and CheckProperties set to false, returns default(T). If the property exists but there's an error on the parser also returns default(T).</remarks>
        public T getObject<T>(string name)
        {
            if (CheckProperties) CheckProperty(name);
            if (!HasProperty(name))
            {
                return default(T);
            }
            try
            {
                string str = getProperty<string>(name);
                JsonSerializerSettings set = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                    TypeNameHandling = TypeNameHandling.All
                };

                return (T)(JsonConvert.DeserializeObject<T>(str, set));
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

    }
}
