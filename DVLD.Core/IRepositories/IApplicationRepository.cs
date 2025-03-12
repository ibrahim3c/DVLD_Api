using DVLD.Core.Helpers;
using DVLD.Core.Models;

namespace DVLD.Core.IRepositories
{
    public interface IApplicationRepository:IBaseRepository<Application>
    {
        Task<Result> ChangeStatusAsync(int id , string FromStatus ,string ToStatus);
    }
}
