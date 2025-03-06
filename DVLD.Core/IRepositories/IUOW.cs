namespace DVLD.Core.IRepositories
{
    public interface IUOW:IDisposable
    {
        IApplicantRepository ApplicantRepository {get;}
        int Complete();
    }
}
