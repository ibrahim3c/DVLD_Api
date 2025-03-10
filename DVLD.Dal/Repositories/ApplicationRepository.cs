using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Dal.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DVLD.Dal.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Result> ChangeStatusAsync(int id, string status)
        {
            if (!AppStatuses.IsValidStatus(status))
                return Result.Failure(["Invalid status"]);

            var application = await _context.applications.FindAsync(id);
            if (application is null)
                return Result.Failure(["Application not found"]);

            application.AppStatus = status;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
