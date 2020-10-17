using System;
using System.Collections.Generic;
using MediatR;
using Fac.Service.Application.Dto;

namespace Fac.Service.Application.Queries
{
    public class MyQuery : IRequest<List<MyDto>>
    {

    }
}
