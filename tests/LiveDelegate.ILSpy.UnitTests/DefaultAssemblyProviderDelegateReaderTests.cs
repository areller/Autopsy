using FluentAssertions;
using LiveDelegate.ILSpy.Extensions;
using LiveDelegate.ILSpy.UnitTests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDelegate.ILSpy.UnitTests
{
    [TestClass]
    public class DefaultAssemblyProviderDelegateReaderTests
    {
        #region Test Objects

        class Foo
        {
            public int Value { get; set; }
        }

        #endregion

        delegate int MyDel1(int x);
        delegate int MyDel2(Foo x);
        delegate int MyDel3(ExternalFoo x);

        [TestMethod]
        public void ShouldReturnTreeOfSimpleDelegate()
        {
            var reader = DelegateReader.CreateWithDefaultAssemblyProvider();
            var tree = reader.Read((MyDel1)(x =>
            {
                if (x >= 0 && x < 5)
                    return 1;
                else if (x >= 5 && x < 10)
                    return 2;
                return 3;
            }));
            tree.Should().NotBeNull();
            tree.FirstMethodOrDefault().Should().NotBeNull();
            tree.Warnings().Count.Should().Be(0);
        }

        [TestMethod]
        public void ShouldReturnTreeOfDelegateWithInternalComplexTypes()
        {
            var reader = DelegateReader.CreateWithDefaultAssemblyProvider();
            var tree = reader.Read((MyDel2)(x =>
            {
                if (x.Value >= 0 && x.Value < 5)
                    return 1;
                else if (x.Value >= 5 && x.Value < 10)
                    return 2;
                return 3;
            }));
            tree.Should().NotBeNull();
            tree.FirstMethodOrDefault().Should().NotBeNull();
            tree.Warnings().Count.Should().Be(0);
        }

        [TestMethod]
        public void ShouldReturnTreeOfDelegateWithExternalComplexTypes()
        {
            var reader = DelegateReader.CreateWithDefaultAssemblyProvider();
            var tree = reader.Read((MyDel3)(x =>
            {
                if (x.Value >= 0 && x.Value < 5)
                    return 1;
                else if (x.Value >= 5 && x.Value < 10)
                    return 2;
                return 3;
            }));
            tree.Should().NotBeNull();
            tree.FirstMethodOrDefault().Should().NotBeNull();
            tree.Warnings().Count.Should().Be(0);
        }
    }
}