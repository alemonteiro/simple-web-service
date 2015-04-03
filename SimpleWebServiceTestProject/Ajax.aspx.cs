using System;
using System.Collections.Generic;
using System.Linq;
using Alcode.SimpleWebService;

namespace Alcode.SimpleWebServiceTestProject
{
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

    public class TestPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<TestJob> Jobs { get; set; }

        public string Summary
        {
            get
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.AppendFormat("Name: {0}; Age: {1};<br/>", Name, Age);
                str.AppendFormat(Jobs == null ? " No Jobs; " : 
                    string.Join("<br/>",
                        Jobs.Select( j =>
                                string.Format("{0} from {0:yyyy-MM-dd} to {1:yyyy-MM-dd}", 
                                    j.Title, j.DateStart, j.DateEnd)).ToArray()));
                
                return str.ToString();
            }
        }
    }

    public class TestJob
    {
        public string Title { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}