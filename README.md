# simple-web-service
.Net library to create simple JSON based Web Services with ASP.NET

The .Net library requires Newtonsoft.Json.dll (http://www.newtonsoft.com/json) (version 4 and 6 tested and working; probably just won't work with 3 or below because I think there was change in the settings)
The JavaScript client requires json2.js (http://www.JSON.org/js.html)

## C# Implementation

The c# implementation is quite easy and the library has some documentation that will be later published online.

```
	public partial class Ajax : SimpleWebServiceInterface
	{
		public int AddNumbers(SimpleWebServiceParams param)
		{
			return param.getInt("x") + param.getInt("y");
		}

		public string Hello(SimpleWebServiceParams param)
		{
			return String.Format("Hello {0}!", param.getString("Name", "Anônimo"));
		}

		public string RecievePerson(SimpleWebServiceParams param)
		{
			TestPerson tp = param.asObject<TestPerson>();
			return tp.Summary;
		}

		public List<TestJob> GetJobs(SimpleWebServiceParams param)
		{
			List<TestJob> l = new List<TestJob>();

			for (int i = 0; i < 15; i++)
			{
				l.Add(new TestJob()
				{
					Title = "Test Job N# " + i
				});
			}

			return l;
		}
	}
```

## simple-web-service-0.1.js
```	
	//SimpleWebService Usage 

    // Object parameters
	SimpleWebService.Request({
			[required] url: Service.aspx', 
			[required] method: 'Hello', 
			[optional] onResult: function(response) { } Response = { Status: 0, Result: {}, Error: {} },
			[optional] onSuccess: function(result) { },
			[optional] onError: function(error) { } Error = { Code: -1, Message: 'Error Message', Details: 'Error Details' }
		});

	// Just call the method, no parameters and no result
	SimpleWebService.Request(url, method);

    // Just post some data
	SimpleWebService.Request(url, method, data);
    
    // Post data and handle Success and error in the same function
    SimpleWebService.Request(url, method, data, onResult);
    
    // Post data and handle Success and error in the differents function
    SimpleWebService.Request(url, method, data, onSuccess, onError);

    // No post data; handle Success and error in the same function
    SimpleWebService.Request(url, method, onResult);

    // No post data; handle Success and error in the differents function
    SimpleWebService.Request(url, method, onSuccess, onError);
```	
Read the comments on simple-web-service-0.1.js for further options

## The HTTP Request
```
	Request template: <url> // Full or relative url to Simple Web Service Page
								?m=<method> // Method to be called
								&e=encodeURIComponent(JSON.stringify(<data>)) // Escaped JSON notification of post parameters
								&s=new Date().getTime() // To avoid unwanted cache
	Ex.: myService.aspx?m=Hello&e=%FA%22&s=25824721
```
This can be modified and improved by overriding SimpleWebServiceInterface.GetParams on C# and SimpleWebService.Request on JS.
```
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
```
## About
Couple of years ago I got really tired of using .NET Service Model for my heavy AJAX projects and started building something to replace it for me. 
Since then I've been copying and pasting similar code in a couple of projects, but now I think it's in a stage that can already help somebody else. Hope it does. =)

I'm already using this lib for my new project so I'll add a android client lib soon.

Also, this is my first open source project so if I'm missing something, just let me know ^^