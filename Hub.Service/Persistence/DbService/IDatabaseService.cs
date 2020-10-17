using System.Collections.Generic;
using System.Threading.Tasks;
using Hub.Service.Application.Dto;

namespace Hub.Service.Persistence.DbService
{
    public interface IDatabaseService
    {
        Task<List<MyDto>> GetData();
    }
}
