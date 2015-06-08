using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Models;

namespace MLM.Persistence.Interfaces
{
    public interface IFlagsRepository
    {
        Task<ResourceResponse<Flag>> FindFlagByIdAsync(int id);
        Task<ResourceResponse<Flag>> AddFlagAsync(Flag flag);
        Task<ResourceResponse<Flag>> SaveFlagAsync(Flag flag);
        Task<ResourceResponse<bool>> DeleteFlagAsync(Flag flag);
    }
}
