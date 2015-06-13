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
        Task<ResourceResponse<Map>> AdminCreateMapAsync(Map map);
        Task<ResourceResponse<Map>> AdminSaveMapAsync(Map adoptedMap);
        Task<ResourceResponse<List<Map>>> AdminGetMapsAsync(int mapTypeId = 0);
        Task<ResourceResponse<Map>> AdminGetMapAsync(int id);
        Task<ResourceResponse<List<MapType>>> AdminGetMapTypesAsync();
        Task<ResourceResponse<MapType>> AdminGetMapTypeAsync(int id);
        Task<ResourceResponse<MapType>> AdminSaveMapTypeAsync(MapType mapType);
        Task<ResourceResponse<MapType>> AdminCreateMapTypeAsync(MapType map);
    }
}
