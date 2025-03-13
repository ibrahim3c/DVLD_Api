using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class TestAppointementRepository : BaseRepository<TestAppointment>, ITestAppointmentRepository
    {
        public TestAppointementRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
