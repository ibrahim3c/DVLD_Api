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
        appTypeRepository=new AppTypeRepository(appDbContext);
        testTypeRepository=new TestTypeRepository(appDbContext);
        ApplicationRepository = new ApplicationRepository(appDbContext);
        LicenseClassRepository = new LicenseClassRepository(appDbContext);
        TestAppointmentRepository = new TestAppointementRepository(appDbContext);
        TestRepository=new TestRepository(appDbContext);

    }

    public IApplicantRepository ApplicantRepository {  get; private set; }
    public IAppTypeRepository  appTypeRepository {  get; private set; }
    public ITestTypeRepository testTypeRepository { get; private set; }
    public IApplicationRepository ApplicationRepository { get; private set; }
    public ILicenseClassRepository LicenseClassRepository { get; private set; }
    public ITestAppointmentRepository TestAppointmentRepository { get; private set; }
    public ITestRepository TestRepository { get; private set; }


    public int Complete()
    {
        return appDbContext.SaveChanges();
    }

    public void Dispose()
    {
        appDbContext.Dispose();
    }
}
