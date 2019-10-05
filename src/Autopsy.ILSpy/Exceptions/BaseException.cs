using System;
using System.Collections;

namespace Autopsy.ILSpy.Exceptions
{
    public abstract class BaseException : Exception
    {
        public override IDictionary Data { get; }

        protected BaseException(string message, IDictionary data)
            : base(message)
        {
            Data = data;
        }
    }
}