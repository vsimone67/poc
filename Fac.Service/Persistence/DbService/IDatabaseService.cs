using System.Collections.Generic;
using System.Threading.Tasks;
using Fac.Service.Application.Dto;

namespace Fac.Service.Persistence.DbService
{
    public interface IDatabaseService
    {
        Task<List<MyDto>> GetData();
    }
}
