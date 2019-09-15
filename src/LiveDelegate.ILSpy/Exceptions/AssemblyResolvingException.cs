using System.Collections.Generic;

namespace LiveDelegate.ILSpy.Exceptions
{
    public class AssemblyResolvingException : BaseException
    {
        public AssemblyResolvingException(string fullname)
            : base($"Assembly '{fullname}' could not be resolved", new Dictionary<string, object>()
            {
                { "fullname", fullname }
            })
        { }
    }
}
