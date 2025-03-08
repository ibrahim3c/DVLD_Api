using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class AppTypeRepository : BaseRepository<ApplicationType>, IAppTypeRepository
    {
        public AppTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
