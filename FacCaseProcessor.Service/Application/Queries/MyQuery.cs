using System;
using System.Collections.Generic;
using MediatR;
using FacCaseProcessor.Service.Application.Dto;

namespace FacCaseProcessor.Service.Application.Queries
{
    public class MyQuery : IRequest<List<MyDto>>
    {

    }
}
