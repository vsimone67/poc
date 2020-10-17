using System.Collections.Generic;
using System.Threading.Tasks;
using Mib.Service.Application.Dto;

namespace Mib.Service.Persistence.DbService
{
    public interface IDatabaseService
    {
        Task<List<MyDto>> GetData();
    }
}
