using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class LicenseClassRepository : BaseRepository<LicenseClass>, ILicenseClassRepository
    {
        public LicenseClassRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
