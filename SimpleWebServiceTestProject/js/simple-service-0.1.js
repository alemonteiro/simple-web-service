/*
    simple-web-service-0.1.js

    Date: 2015-04-02
    Project: Simple Web Service (https://github.com/alemonteiro/simple-web-service)
    Apache License Version 2.0, January 2004 (http://www.apache.org/licenses/)
            
    This file creates an object to request and handle response from .NET Simple Web Service.

    Dependency: json2.js (http://www.json.org/js.html)

    Request template: <url> // Full or relative url to Simple Web Service Page
                            ?m=<method> // Method to be called
                            &e=encodeURIComponent(JSON.stringify(<data>)) // Escaped JSON notification of post parameters
                            &s=new Date().getTime() // To avoid unwanted cache
             Ex.: myService.aspx?m=Hello&e=%FA%22&s=25824721
                            				
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

		
	//SimpleWebService.Client Usage:
	
    // This is just a easy way to make multiple requests to the same url (calls SimpleWebService.Request internally)
	var ssc = new SimpleWebService.Client('Service.aspx');
	
    // Object parameters
    ssc.Request({
			[required] method: 'Hello',
			[required] onResult: function(response) { } // Response = { Status: 0, Result: {}, Error: {} }
			[required] onSuccess: function(result) { } // Result = ? (each method can have a different result type)
			[required] onError: function(error) { } //  Error = { Code: -1, Message: 'Error Message', Details: 'Error Details' }
		});

	// Just call the method, no parameters and no result
	ssc.Request(method);
    // Just post some data
	ssc.Request(method, data);
    // Post data and handle Success and error in the same function
    ssc.Request(method, data, onResult);
    // Post data and handle Success and error in the differents function
    ssc.Request(method, data, onSuccess, onError);
    // No post data; handle Success and error in the same function
    ssc.Request(method, onResult);
    // No post data; handle Success and error in the differents function
    ssc.Request(method, onSuccess, onError);
    
    //jQuery Extension
    //If jQuery is enable it's created $.ssRequest() method and it'll accept the same arguments as SimpleWebService.Request.
    //Ex.: $.ssRequest(url, method);

    Default Status: 
        SimpleWebService.Status.Success = 0;
        SimpleWebService.Status.Fail = -1; 

	Remarks:
		
        If onResult is specified, it will be the only triggered callback. In other words: neither onSuccess nor onError will be called if onResult is called.

        onResult recieves the Response object, which contains the Status and Result or Error object.
        onSuccess recieves the Result object and it's only called if Response.Status == SimpleWebService.Status.Success and if onResult is not especified.
        onError recieves the Result object and it's only called if Response.Status != SimpleWebService.Status.Success and if onResult is not especified.
		
        SimpleWebService.Status.Fail is the default status code for errors, but each service can have custom codes, you should always check if response.Status == SimpleWebService.Status.Success for handling the result in onResult
*/

var SimpleWebService = SimpleWebService || {};

(function ($) {
    'use strict';
    
    var _extend = function (a, b) {
        var n;
        if (!a) {
            a = {};
        }
        for (n in b) {
            a[n] = b[n];
        }
        return a;
    }

    SimpleWebService.Client = function (url) {
        this.url = url;
    };

    SimpleWebService.Client.prototype.Request = function (method, data, onResult, onSuccess, onError) {

        if (typeof method === 'object') {
            method.url = this.url;
            SimpleWebService.Request(method);
        }
        else {
            SimpleWebService.Request(this.url, method, data, onResult, onSuccess, onError);
        }
    };

    SimpleWebService.Status = {
        Success: 0,
        Fail: -1
    };

    SimpleWebService.Request = function (url, method, data, onResult, onSuccess, onError) {

        var _defaults = {
            url: undefined,
            method: undefined,
            data: undefined,
            onResult: undefined,
            onSuccess: undefined,
            onError: undefined
        }, 
        cfg = {}, escapedData, r, 
        _parseResponse = function (status, statusText, textResponse) {
            
            var response = status == 200 ? JSON.parse(textResponse) :
                            { 
                                Status : SimpleWebService.Status.Fail,
                                Error: {
                                    Code: status,
                                    Message: statusText,
                                    Details: textResponse
                                }
                            };

            if (typeof cfg.onResult === 'function') {
                cfg.onResult(response);
            }
            else if (typeof cfg.onSuccess === 'function' && response.Status == SimpleWebService.Status.Success) {
                cfg.onSuccess(response.Result);
            }
            else if (typeof cfg.onError === 'function') {
                cfg.onError(response.Error);
            }
        };;

        if (typeof url === 'object') {
            cfg = _extend(_defaults, arguments[0]);
        }
        else if (typeof data === 'function' && typeof onResult === 'undefined') {
            cfg = _extend(_defaults, {
                url: arguments[0],
                method: arguments[1],
                data: {},
                onResult: arguments[2]
            });
        }
        else if (typeof data === 'function' && typeof onResult === 'function') {
            cfg = _extend(_defaults, {
                url: arguments[0],
                method: arguments[1],
                data: {},
                onSuccess: arguments[2],
                onError: arguments[3]
            });
        }
        else if (typeof data === 'object') {
            if (typeof onResult !== 'function') {
                cfg = _extend(_defaults, {
                    url: arguments[0],
                    method: arguments[1],
                    data: arguments[2]
                });
            }
            else if (typeof onSuccess === 'function') {
                cfg = _extend(_defaults, {
                    url: arguments[0],
                    method: arguments[1],
                    data: arguments[2],
                    onSuccess: arguments[3],
                    onError: arguments[4]
                });
            }
            else if (typeof arguments[2] === 'object') {
                cfg = _extend(_defaults, {
                    url: arguments[0],
                    method: arguments[1],
                    data: arguments[2],
                    onResult: arguments[3]
                });
            }
        }
        else {
            cfg = _extend(_defaults, {
                url: arguments[0],
                method: arguments[1]
            });
        }

        escapedData = encodeURIComponent(JSON.stringify(cfg.data));

        r = {
            m: cfg.method,
            e: escapedData,
            s: new Date().getTime()
        };
        
        try 
        {
            var xmlhttp = window.XMLHttpRequest ?
            // IE7+, Firefox, Chrome, Opera, Safari
            new XMLHttpRequest() :
            // IE6, IE5
            new ActiveXObject("Microsoft.XMLHTTP");

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    _parseResponse(xmlhttp.status, xmlhttp.statusText, xmlhttp.responseText);
                }
            }
            xmlhttp.open("POST", cfg.url, true);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xmlhttp.send("m=" + r.m + "&e=" + r.e + "&s=" + r.s);
        }
        catch(err)
        {
            _parseResponse(err.number, err.message, err.description);
        }
    };

    // Extends the jQuery object with $.ssRequest(opts);
    if ( $ !== undefined ) {
        $.ssRequest = function (url, method, data, onResult, onSucess, onError) {
            SimpleWebService.Request(url, method, data, onResult, onSuccess, onError);
        };
    }

}(typeof jQuery === 'object' ? jQuery : undefined));