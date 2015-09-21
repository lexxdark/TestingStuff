using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class MultiInvoke
    {

        private List<Action> _actions;

        public void InvokeAll()
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (Action action in _actions)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new MultiplExceptionWrapper("InvokeAll", exceptions);
            }
        }

    }

    public class MultiplExceptionWrapper : Exception
    {
        public List<Exception> Exceptions { get; }

        public MultiplExceptionWrapper(string source, List<Exception> exceptions)
            : base("Multiple errors occured in " + source, exceptions.First())
        {
            Exceptions = exceptions;
        }
    }
}
