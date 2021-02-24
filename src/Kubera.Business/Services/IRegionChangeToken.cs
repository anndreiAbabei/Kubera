using Microsoft.Extensions.Primitives;

namespace Kubera.Business.Services
{
    internal interface IRegionChangeToken : IChangeToken
    {
        string Region { get; }
    }
}