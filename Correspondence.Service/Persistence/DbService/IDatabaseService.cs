using System.Collections.Generic;
using System.Threading.Tasks;
using Correspondence.Service.Application.Dto;

namespace Correspondence.Service.Persistence.DbService
{
    public interface IDatabaseService
    {
        Task<List<MyDto>> GetData();
    }
}
