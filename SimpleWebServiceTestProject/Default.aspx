<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Alcode.SimpleWebServiceTestProject.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simple Service Test Page</title>
    
    
    <script type="text/javascript" src="js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="js/json2.js"></script>
    <script type="text/javascript" src="js/simple-service-0.1.js"></script>

    <script type="text/javascript">
        
        var getBId = function(id) { return document.getElementById(id); },
            ssr = new SimpleWebService.Client("Ajax.aspx"),
            tests = [
                { name: 'Hello', method: 'Hello', data: {} },
                { name: 'Hello (Name=John)', method: 'Hello', data: { Name: 'John'} },
                { name: 'Add Numbers (x=10,y=50)', method: 'AddNumbers', data: { x: 10, y: 50} },
                { name: 'Get Comples Objects', method: 'GetJobs', data: {} },
                { name: 'Send Complex Objects', method: 'RecievePerson', data: {
                    Name: 'John',
                    Age: 20,
                    Jobs: [
                        { Title: 'Bus Driver', DateStart: new Date(2011, 4, 5), DateEnd: new Date(2012, 4, 5) },
                        { Title: 'Bus Driver', DateStart: new Date(2013, 4, 5), DateEnd: new Date(2015, 1, 5) }
                    ]
                }
                }
            ];

                var runTest = function (t) {

                    if (typeof t == 'undefined') throw new Error('Invalid argument!');

                    t = typeof t == 'object' ? t : tests[t];

                    var d = document.createElement('div');
                    document.getElementById('resultDiv').appendChild(d);
                    d.className = "test-run";
                    d.innerHTML = 'Requesting <b>' + t.method + '</b> with ' + JSON.stringify(t.data);

                    var opts = {
                        method: t.method,
                        data: t.data
                    }
                    if (form1.chkOnResult.checked) {
                        opts.onResult = function (response) {
                            d.innerHTML += '<br/>Result: ' + JSON.stringify(response);
                        };
                    }
                    else {
                        opts.onSuccess = function (result) {
                            d.innerHTML += '<br/>Succes: ' + JSON.stringify(result);
                        };
                        opts.onError = function (error) {
                            d.innerHTML += '<br/>Error: ' + JSON.stringify(error);
                        };
                    }

                    ssr.Request(opts);
                }

                var runAllTests = function () {

                    for (var i = 0, il = tests.length; i < il; i++) {
                        runTest(i);
                        //(function (t) { runTest(t); })(tests[i]);
                    }
                };

                window.onload = function () {
                    var slc = getBId('slcTests'), tmp = '';
                    for (var i = 0, il = tests.length, opt; i < il; i++) {
                        opt = document.createElement('option');
                        opt.value = i;
                        opt.innerHTML = tests[i].name;
                        slc.appendChild(opt);

                    }
                    getBId('btnTest').onclick = function () {
                        runTest(slc.selectedIndex);
                    };
                    getBId('btnTestAll').onclick = function () {
                        runAllTests();
                    };
                };

    </script>

    <style type="text/css">
        div.test-run 
        {
            border: 1px solid black;
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <select id="slcTests" size="1"></select><button type="button" id="btnTest">Test It!</button>

        <input type="checkbox" id="chkOnResult" value="1" />
        <label for="chkOnResult">Use onResult(instead of onSuccess/onError)</label>
        
        <br />
        <button type="button" id="btnTestAll">Run all tests!!!</button>
        <div id="resultDiv">
            
        </div>
    </form>
</body>
</html>
