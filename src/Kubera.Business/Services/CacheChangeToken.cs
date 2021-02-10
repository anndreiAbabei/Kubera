using Microsoft.Extensions.Primitives;
using System;

namespace Kubera.Business.Services
{
    internal class CacheChangeToken : IChangeToken
    {
        private readonly IChangeToken _innerChangeToken;
        private bool _hasChanged;

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged 
        {
            get => _hasChanged || (_innerChangeToken?.HasChanged ?? false); 
            set => _hasChanged = value; 
        }

        public CacheChangeToken()
            : this(null)
        {

        }
        public CacheChangeToken(IChangeToken innerChangeToken)
        {
            _innerChangeToken = innerChangeToken;
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            throw new NotImplementedException();
        }
    }
}
