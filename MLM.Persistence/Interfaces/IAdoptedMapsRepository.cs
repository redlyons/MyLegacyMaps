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
        Task<ResourceResponse<AdoptedMap>> FindByAdoptedMapIdAsync(int id);
        Task<ResourceResponse<AdoptedMap>> AddAdoptedMapAsync(AdoptedMap map);
        Task<ResourceResponse<AdoptedMap>> SaveAdoptedMapAsync(AdoptedMap adoptedMap);
        Task<ResourceResponse<bool>> DeleteAdoptedMapAsync(AdoptedMap adoptedMap);
        Task<ResourceResponse<List<ShareStatusType>>> GetShareTypesAsync();
    }
}
