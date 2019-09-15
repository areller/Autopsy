using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;
using ICSharpCode.Decompiler.CSharp.Syntax;

namespace LiveDelegate.ILSpy
{
    class CachedDelegateReader : IDelegateReader
    {
        private IDelegateReader _actualReader;

        private ConcurrentDictionary<Delegate, Lazy<SyntaxTree>> _cache;

        public CachedDelegateReader(IDelegateReader actualReader)
        {
            _actualReader = actualReader;
            _cache = new ConcurrentDictionary<Delegate, Lazy<SyntaxTree>>();
        }

        public SyntaxTree Read(Delegate @delegate)
        {
            Contract.Assert(@delegate != null);

            try
            {
                return _cache.GetOrAdd(@delegate, key => new Lazy<SyntaxTree>(() => _actualReader.Read(key), LazyThreadSafetyMode.ExecutionAndPublication)).Value;
            }
            catch (Exception)
            {
                _cache.TryRemove(@delegate, out _);
                throw;
            }
        }
    }
}