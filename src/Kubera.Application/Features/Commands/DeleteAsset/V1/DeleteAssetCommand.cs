﻿using CSharpFunctionalExtensions;
using MediatR;
using System;

namespace Kubera.Application.Features.Commands.DeleteAsset.V1
{
    public class DeleteAssetCommand : IRequest<IResult>
    {
        public Guid Id { get; set; }
    }
}
