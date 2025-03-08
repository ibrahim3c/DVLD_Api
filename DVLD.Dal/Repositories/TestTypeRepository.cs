using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class TestTypeRepository : BaseRepository<TestType>, ITestTypeRepository
    {
        public TestTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
