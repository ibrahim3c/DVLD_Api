using DVLD.Core.IRepositories;
using DVLD.Dal.Data;

namespace DVLD.Dal.Repositories;

public class UOW : IUOW
{
    private readonly AppDbContext appDbContext;

    public UOW(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
        ApplicantRepository=new ApplicantRepository(appDbContext);
    }

    public IApplicantRepository ApplicantRepository {  get; private set; }


    public int Complete()
    {
        return appDbContext.SaveChanges();
    }

    public void Dispose()
    {
        appDbContext.Dispose();
    }
}
