using System;

namespace LiveDelegate.ILSpy.UnitTests.TestObjects
{
    public class ExternalFoo
    {
        public int Value { get; set; }

        public object GetNew() => new ExternalFoo();
    }
}
