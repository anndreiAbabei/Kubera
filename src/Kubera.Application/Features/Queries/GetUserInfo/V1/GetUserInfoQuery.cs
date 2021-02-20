using CSharpFunctionalExtensions;
using Kubera.Application.Common.Models;
using MediatR;

namespace Kubera.Application.Features.Queries.GetUserInfo.V1
{
    public class GetUserInfoQuery : IRequest<IResult<UserInfoModel>>
    {
    }
}
