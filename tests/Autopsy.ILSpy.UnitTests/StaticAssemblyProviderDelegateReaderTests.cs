using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Autopsy.ILSpy.Extensions;
using System.Reflection;
using Autopsy.ILSpy.UnitTests.TestObjects;
using Autopsy.ILSpy.Exceptions;

namespace Autopsy.ILSpy.UnitTests
{
    [TestClass]
    public class StaticAssemblyProviderDelegateReaderTests
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

        private int GetValue(ExternalFoo f) => f.Value;

        private IList<Assembly> GetCoreAssemblies() => AssemblySet.Core.Concat(new[] { typeof(Foo).Assembly }).ToList();

        [TestMethod]
        public void ShouldReturnTreeOfSimpleDelegate()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(GetCoreAssemblies(), false, false);
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
        [ExpectedException(typeof(AssemblyNotFoundException))]
        public void ShouldReturnTreeOfDelegateWithInternalComplexTypes_NotReferenced()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(AssemblySet.Core, false, false);
            var tree = reader.Read((MyDel2)(x =>
            {
                if (x.Value >= 0 && x.Value < 5)
                    return 1;
                else if (x.Value >= 5 && x.Value < 10)
                    return 2;
                return 3;
            }));
        }

        [TestMethod]
        public void ShouldReturnTreeOfDelegateWithInternalComplexTypes_NotReferenced_AddOnPrepare()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(AssemblySet.Core, true, false);
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
        public void ShouldReturnTreeOfDelegateWithInternalComplexTypes_Referenced()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(GetCoreAssemblies(), false, false);
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
        public void ShouldReturnTreeOfDelegateWithExternalComplexTypes_NotReferenced()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(GetCoreAssemblies().Concat(new[] { typeof(ExternalFoo).Assembly }).ToList(), false, false);
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

        [TestMethod]
        public void ShouldReturnTreeOfDelegateWithExternalComplexTypes_Referenced()
        {
            var reader = DelegateReader.CreateWithSetOfAssemblies(GetCoreAssemblies().Concat(new[] { typeof(ExternalFoo).Assembly }).ToList(), false, false);
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