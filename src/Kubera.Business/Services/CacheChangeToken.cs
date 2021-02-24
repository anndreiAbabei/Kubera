using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace Kubera.Business.Services
{
    internal class CacheChangeToken : IRegionChangeToken
    {
        private readonly IChangeToken[] _innerChangeTokens;
        private bool _hasChanged;

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged 
        {
            get => _hasChanged || (_innerChangeTokens?.Any(t => t.HasChanged) ?? false); 
            set => _hasChanged = value; 
        }

        public string Region { get; }

        internal CacheChangeToken(string region)
            : this(region, null)
        {

        }
        internal CacheChangeToken(string region, params IChangeToken[] innerChangeTokens)
        {
            Region = region;
            _innerChangeTokens = innerChangeTokens;
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            throw new NotImplementedException();
        }
    }
}
