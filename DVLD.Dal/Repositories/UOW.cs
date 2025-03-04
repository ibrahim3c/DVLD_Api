using DVLD.Core.IRepositories;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories;

public class UOW : IUOW
{
    private readonly AppDbContext appDbContext;

    public UOW(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    public int Complete()
    {
        return appDbContext.SaveChanges();
    }
}
