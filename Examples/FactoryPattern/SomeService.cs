using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.FactoryPattern
{
    public class SomeService : ISomeService
    {
        private readonly string _someData;

        public SomeService(string someData)
        {
            _someData = someData;
        }

        public SomeService()
            : this("Default")
        { }

        public void DoSomething()
        {
            Debug.WriteLine(string.Format(
                "[{0}]SomeService.DoSomething()", _someData));
        }
    }
}
