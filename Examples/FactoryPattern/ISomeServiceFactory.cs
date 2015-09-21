using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.FactoryPattern
{
    public interface ISomeServiceFactory
    {
        // Singleton pattner
        ISomeService SomeService { get; }
        // In case we require an instance for each user
        ISomeService GetNewInstance();
        // In case the service requires parameters to run
        ISomeService GetSomeServiceWithData(string someData);
    }

    public class SomeServiceFactory : ISomeServiceFactory
    {
        private ISomeService _instance = new SomeService();

        public ISomeService SomeService { get { return _instance; } }
        public ISomeService GetNewInstance()
        {
            return new SomeService();
        }

        public ISomeService GetSomeServiceWithData(string someData)
        {
            return new SomeService(someData);
        }
    }
}
