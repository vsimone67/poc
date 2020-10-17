using System;
using System.Collections.Generic;
using MediatR;
using Hub.Service.Application.Dto;

namespace Hub.Service.Application.Queries
{
    public class MyQuery : IRequest<List<MyDto>>
    {

    }
}
