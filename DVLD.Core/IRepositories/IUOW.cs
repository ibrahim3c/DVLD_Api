﻿namespace DVLD.Core.IRepositories
{
    public interface IUOW:IDisposable
    {
        IApplicantRepository ApplicantRepository {get;}
        IAppTypeRepository  appTypeRepository { get;}
        ITestTypeRepository testTypeRepository { get;}
        IApplicationRepository ApplicationRepository { get;}
        ILicenseClassRepository LicenseClassRepository { get;}
        ITestAppointmentRepository TestAppointmentRepository { get;}
        ITestRepository TestRepository { get;}
        IDriverRepository DriverRepository { get;}
        ILicenseRepository LicenseRepository { get;}
        IRenewLicenseApplicationRepository RenewLicenseApplicationRepository { get; }
        IDetainedLicenseRepository DetainedLicenseRepository { get; }
        int Complete();
    }
}
