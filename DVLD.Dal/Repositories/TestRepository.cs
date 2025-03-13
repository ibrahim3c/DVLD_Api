using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class TestRepository : BaseRepository<Test>, ITestRepository
    {
        public TestRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
