using System.Collections.Generic;
using System.Threading.Tasks;
using FacCaseProcessor.Service.Application.Dto;

namespace FacCaseProcessor.Service.Persistence.DbService
{
    public interface IDatabaseService
    {
        Task<List<MyDto>> GetData();
    }
}
