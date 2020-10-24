using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FacCaseProcessor.Service.Application.Dto;

namespace FacCaseProcessor.Service.Persistence.DbService
{
    public class DatabaseService : IDatabaseService
    {
        public async Task<List<MyDto>> GetData()
        {
            await Task.FromResult(1);
            return new List<MyDto>();
        }
    }
}
