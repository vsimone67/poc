using System;
using System.Collections.Generic;
using MediatR;
using Mib.Service.Application.Dto;

namespace Mib.Service.Application.Queries
{
    public class MyQuery : IRequest<List<MyDto>>
    {

    }
}
