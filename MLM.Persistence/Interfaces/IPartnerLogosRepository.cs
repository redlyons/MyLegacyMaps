using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Models;
namespace MLM.Persistence.Interfaces
{
    public interface IPartnerLogosRepository
    {
        Task<ResourceResponse<List<PartnerLogo>>> GetPartnerLogosAsync();
        Task<ResourceResponse<List<PartnerLogo>>> AdminGetPartnerLogosAsync();
        Task<ResourceResponse<PartnerLogo>> AdminGetPartnerLogoAsync(int id);
        Task<ResourceResponse<PartnerLogo>> AdminSavePartnerLogoAsync(PartnerLogo logo);
        Task<ResourceResponse<PartnerLogo>> AdminCreatePartnerLogoAsync(PartnerLogo logo);
    }
}
