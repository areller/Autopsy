namespace LiveDelegate.ILSpy.UnitTests.TestObjects
{
    public class ExternalBar
    {
        public int GetValue(ExternalFoo f)
        {
            return f.Value;
        }
    }
}
