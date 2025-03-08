namespace DVLD.Core.IRepositories
{
    public interface IUOW:IDisposable
    {
        IApplicantRepository ApplicantRepository {get;}
        IAppTypeRepository  appTypeRepository { get;}
        ITestTypeRepository testTypeRepository { get;}
        int Complete();
    }
}
