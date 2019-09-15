﻿using System.Collections.Generic;
using System.Reflection;

namespace LiveDelegate.ILSpy.Exceptions
{
    public class AssemblyNotFoundException : BaseException
    {
        public AssemblyNotFoundException(Assembly assembly)
            : base($"Assembly '{assembly.FullName}' was not found", new Dictionary<string, object>()
            {
                { "assembly", assembly }
            })
        { }
    }
}