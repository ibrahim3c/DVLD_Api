using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class RenewLicenseApplicationRepository : BaseRepository<RenewLicenseApplication>, IRenewLicenseApplicationRepository
    {
        public RenewLicenseApplicationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
