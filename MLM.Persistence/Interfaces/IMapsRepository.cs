using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Models;

namespace MLM.Persistence.Interfaces
{
    public interface IMapsRepository
    {
        Task<ResourceResponse<List<Map>>> GetMapsAsync(int mapTypeId = 0);
        Task<ResourceResponse<Map>> GetMapAsync(int id);
        Task<ResourceResponse<List<MapType>>> GetMapTypesAsync();
        Task<ResourceResponse<Map>> CreateMapAsync(Map map);
        Task<ResourceResponse<Map>> SaveMapAsync(Map adoptedMap);
        Task<ResourceResponse<List<Map>>> AdminGetMapsAsync(int mapTypeId = 0);
        Task<ResourceResponse<Map>> AdminGetMapAsync(int id);
    }
}
