using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class DetainedLicenseRepository : BaseRepository<DetainedLicense>, IDetainedLicenseRepository
    {
        public DetainedLicenseRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
