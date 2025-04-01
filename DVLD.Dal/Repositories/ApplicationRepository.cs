using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Dal.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Result> ChangeStatusAsync(int id, string FromStatus,string ToStatus)
        {
            if (!AppStatuses.IsValidStatus(FromStatus) && !AppStatuses.IsValidStatus(ToStatus))
                return Result.Failure(["Invalid status"]);

            var application = await _context.applications.SingleOrDefaultAsync(a=>a.AppID==id && a.AppStatus==FromStatus);
            if (application is null)
                return Result.Failure(["Application with this status was not found"]);

            if(FromStatus!=ToStatus)
                application.AppStatus = ToStatus;

            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> ChangeStatusAsync(int id, string ToStatus)
        {
            if (!AppStatuses.IsValidStatus(ToStatus))
                return Result.Failure(["Invalid status"]);

            var application = await _context.applications.SingleOrDefaultAsync(a => a.AppID == id);
            if (application is null)
                return Result.Failure(["No Application found"]);
                application.AppStatus = ToStatus;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
