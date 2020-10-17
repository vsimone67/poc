using System;
using System.Collections.Generic;
using MediatR;
using Hub.Service.Application.Dto;

namespace Hub.Service.Application.Commands
{
    public class MyCommand : IRequest<MyDto>
    {

    }
}
