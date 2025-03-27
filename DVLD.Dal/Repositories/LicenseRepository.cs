using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    internal class LicenseRepository : BaseRepository<License>, ILicenseRepository
    {
        public LicenseRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
