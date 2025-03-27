using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class DriverRepository : BaseRepository<Driver>, IDriverRepository
    {
        public DriverRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
