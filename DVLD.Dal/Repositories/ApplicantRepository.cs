using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories
{
    public class ApplicantRepository :BaseRepository<Applicant>, IApplicantRepository
    {
        public ApplicantRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
