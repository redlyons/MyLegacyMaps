using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Models;

namespace MLM.Persistence.Interfaces
{
    public interface IAdoptedMapsRepository
    {
        Task<ResourceResponse<List<AdoptedMap>>> GetAdoptedMapsByUserIdAsync(string userId);
        Task<ResourceResponse<List<AdoptedMap>>> GetPublicAdoptedMapsByUserIdAsync(string userId);
        Task<ResourceResponse<AdoptedMap>> GetAdoptedMapByIdAsync(int id);
        Task<ResourceResponse<AdoptedMap>> CreateAdoptedMapAsync(AdoptedMap map);
        Task<ResourceResponse<AdoptedMap>> SaveAdoptedMapAsync(AdoptedMap adoptedMap);
        Task<ResourceResponse<bool>> DeleteAdoptedMapAsync(AdoptedMap adoptedMap);
        Task<ResourceResponse<List<ShareStatusType>>> GetShareTypesAsync();
    }
}
