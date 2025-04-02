using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;

namespace DVLD.Core.Services.Implementations
{
    public class LicenseService : ILicenseService
    {
        private readonly IUOW uow;
        private readonly IDriverServices driverServices;
        private readonly ITestService testService;

        public LicenseService(IUOW uOW,IDriverServices driverServices,ITestService testService)
        {
            uow = uOW;
            this.driverServices = driverServices;
            this.testService = testService;
        }

        public async Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByApplicantIdAsync(int applicantId)
        {
            var result = (await driverServices.GetDriverByApplicantIdAsync(applicantId));
            if (!result.IsSuccess)
                return Result<IEnumerable<GetLicenseDTO>>.Failure(result.Errors);
            var driverId = result.Value.DriverId;

            var licenses = await uow.LicenseRepository.FindAllAsync(l => l.DriverId == driverId,
                             ["Driver.Applicant", "LicenseClass"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetLicenseDTO>>.Failure(["No License Found!"]);

            var licensesDTO = licenses.Select(license => new GetLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                LicenseClass = license.LicenseClass?.Name ?? "Unknown",
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                IssueReason = license.IssueReason,
                IsValid = license.IsValid,
                Notes = license.Notes,
                PaidFees = license.PaidFees,
                IsDetained = false // TODO: Add logic if necessary
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);
        }

        public async Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByDriverIdAsync(int driverId)
        {

            var licenses = await uow.LicenseRepository.FindAllAsync(l => l.DriverId == driverId,
                             ["Driver.Applicant", "LicenseClass"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetLicenseDTO>>.Failure(["No License Found!"]);

            var licensesDTO = licenses.Select(license => new GetLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                LicenseClass = license.LicenseClass?.Name ?? "Unknown",
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                IssueReason = license.IssueReason,
                IsValid = license.IsValid,
                Notes = license.Notes,
                PaidFees = license.PaidFees,
                IsDetained = false // TODO: Add logic if necessary
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);
        }

        public async Task<Result<IEnumerable<GetLicenseDTO>>> GetLicenseByLicenseIdAsync(int licenseId)
        {
            var licenses = await uow.LicenseRepository.FindAllAsync(l => l.LicenseId == licenseId,
                ["Driver.Applicant", "LicenseClass"]);

            if (!licenses.Any())
                return Result<IEnumerable<GetLicenseDTO>>.Failure(["No License Found!"]);

            var licensesDTO = licenses.Select(license => new GetLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                LicenseClass = license.LicenseClass?.Name ?? "Unknown",
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                IssueReason = license.IssueReason,
                IsValid = license.IsValid,
                Notes = license.Notes,
                PaidFees = license.PaidFees,
                IsDetained = false // TODO: Add logic if necessary
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);
        }

        public async Task<Result<IEnumerable<GetLicenseDTO>>> GetLicensesByNationalNoAsync(string nationalNo)
        {
            var result=(await driverServices.GetDriverByApplicantNationalNoAsync(nationalNo));
            if (!result.IsSuccess)
                return Result<IEnumerable<GetLicenseDTO>>.Failure(result.Errors);
            var driverId = result.Value.DriverId;

            var licenses = await uow.LicenseRepository.FindAllAsync(l => l.DriverId == driverId,
                             ["Driver.Applicant", "LicenseClass"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetLicenseDTO>>.Failure(["No License Found!"]);

            var licensesDTO = licenses.Select(license => new GetLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                LicenseClass = license.LicenseClass?.Name ?? "Unknown",
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                IssueReason = license.IssueReason,
                IsValid = license.IsValid,
                Notes = license.Notes,
                PaidFees = license.PaidFees,
                IsDetained = false // TODO: Add logic if necessary
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);

        }

        public async Task<Result<int>> IssueLicenseFirstTimeAsync(AddLicenseDTO addLicenseDTO)
        {

            var applicant = await uow.ApplicantRepository.FindAsync(a => a.NationalNo == addLicenseDTO.NationalNo);
            if (applicant == null)
                return Result<int>.Failure(["No Applicant Found"]);


            var isAllTestsPassed =await testService.IsAllTestPassed(applicant.ApplicantId, addLicenseDTO.AppId);
          if (!isAllTestsPassed.IsSuccess)
                 return Result<int>.Failure(isAllTestsPassed.errors);


            var driver = await uow.DriverRepository.FindAsync(d=> d.applicantId == applicant.ApplicantId);
            if(driver !=null)
            {
                if (await uow.LicenseRepository.AnyAsync(l => l.LicenseClassId == addLicenseDTO.LicenseClassId && l.DriverId == driver.DriverId))
                {
                    return Result<int>.Failure(["U already have a License from same license class."]);
                }
            }

                var result=await driverServices.AddDriverAsync(applicant.ApplicantId);
                if(!result.IsSuccess)
                    return Result<int>.Failure(result.Errors);

            var license = new License
            {
                AppId = addLicenseDTO.AppId,
                IssueDate = DateTime.UtcNow,
                DriverId = result.Value,
                IssueReason = IssueReasons.FirstTime,
                LicenseClassId = addLicenseDTO.LicenseClassId,
                IsValid = true,
                Notes = addLicenseDTO.Notes,
                PaidFees = addLicenseDTO.PaidFees
            };

            await uow.LicenseRepository.AddAsync(license);

            await uow.ApplicationRepository.ChangeStatusAsync(addLicenseDTO.AppId, AppStatuses.Completed);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);

        }

        public async Task<Result> ValidateLicenseAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l=>l.LicenseId== licenseId, ["LicenseClass"]);
            if (license == null)
                return Result.Failure(["No License Found"]);

            if (!license.IsValid)
                return Result.Failure(["The License is not active"]);

            // Calculate license expiry date
            var expiryDate = license.IssueDate.AddYears(license.LicenseClass.ValidityPeriod);

            // Check if license has expired
            if (DateTime.UtcNow > expiryDate)
                return Result.Failure(["The License has expired"]);

            return Result.Success();
        }

        // InternationalLicense
        public async Task<Result<int>> IssueInternationalLicenseAsync(AddInternationalLicenseDTO addLicenseDTO)
        {
            var application = await uow.ApplicationRepository.FindAsync(a => a.AppID == addLicenseDTO.AppId && a.AppStatus==AppStatuses.Approved, ["Applicant"]);
            if (application == null)
                return Result<int>.Failure(["No Approved Application Found"]);


            var driver = await uow.DriverRepository.FindAsync(d => d.applicantId == application.ApplicantId);
            if (driver != null)
            {
                return Result<int>.Failure(["No Driver found for this application"]);
            }


            var license = new License
            {
                AppId = addLicenseDTO.AppId,
                IssueDate = DateTime.UtcNow,
                DriverId = driver.DriverId,
                IssueReason = IssueReasons.InternationalLicenseFirstTime,
                IsValid = true,
                Notes = addLicenseDTO.Notes,
                PaidFees = addLicenseDTO.PaidFees //Todo CHECK FOR THIS 
            };

            await uow.LicenseRepository.AddAsync(license);
            await uow.ApplicationRepository.ChangeStatusAsync(addLicenseDTO.AppId, AppStatuses.Completed);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);
        }

        public async Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByApplicantIdAsync(int applicantId)
        {
            var result = (await driverServices.GetDriverByApplicantIdAsync(applicantId));
            if (!result.IsSuccess)
                return Result <GetInternationalLicenseDTO>.Failure(result.Errors);
            var driverId = result.Value.DriverId;
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppTypeID == (int)AppTypes.NewInternationalDrivingLicense && a.ApplicantId == applicantId);
            var license = await uow.LicenseRepository.FindAsync(l => l.DriverId == driverId && l.AppId==app.AppID,
                             ["Driver.Applicant", "LicenseClass"]);
            if (license==null)
                return Result <GetInternationalLicenseDTO>.Failure(["No License Found!"]);


            var licenseDTO =  new GetInternationalLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                Notes = license.Notes,
                PaidFees = license.PaidFees,
            };

            return Result<GetInternationalLicenseDTO>.Success(licenseDTO);
        }

        public async Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByDriverIdAsync(int driverId)
        {
            var driver = await uow.DriverRepository.FindAsync(d => d.DriverId == driverId);
            if (driver is null)
                return Result<GetInternationalLicenseDTO>.Failure(["No Driver Found"]);

            var app = await uow.ApplicationRepository.FindAsync(a => a.AppTypeID == (int)AppTypes.NewInternationalDrivingLicense && a.ApplicantId == driver.applicantId);
            var license = await uow.LicenseRepository.FindAsync(l => l.DriverId == driverId && l.AppId == app.AppID,
                             ["Driver.Applicant", "LicenseClass"]);
            if (license == null)
                return Result<GetInternationalLicenseDTO>.Failure(["No License Found!"]);


            var licenseDTO = new GetInternationalLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                Notes = license.Notes,
                PaidFees = license.PaidFees,
            };

            return Result<GetInternationalLicenseDTO>.Success(licenseDTO);
        }

        public async Task<Result<GetInternationalLicenseDTO>> GetInternationalLicenseByLicenseIdAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == licenseId,
                             ["Driver.Applicant", "LicenseClass"]);

            if (license == null && license.Application.AppTypeID==(int)AppTypes.NewInternationalDrivingLicense)
                return Result<GetInternationalLicenseDTO>.Failure(["No License Found!"]);


            var licenseDTO = new GetInternationalLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                Notes = license.Notes,
                PaidFees = license.PaidFees,
            };

            return Result<GetInternationalLicenseDTO>.Success(licenseDTO);
        }

        public async Task<Result<GetInternationalLicenseDTO>> GetInternationalLicensesByNationalNoAsync(string nationalNo)
        {
            var result = (await driverServices.GetDriverByApplicantNationalNoAsync(nationalNo));
            if (!result.IsSuccess)
                return Result<GetInternationalLicenseDTO>.Failure(result.Errors);
            var driverId = result.Value.DriverId;
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppTypeID == (int)AppTypes.NewInternationalDrivingLicense && a.ApplicantId ==result.Value.applicantId);
            var license = await uow.LicenseRepository.FindAsync(l => l.DriverId == driverId && l.AppId == app.AppID,
                             ["Driver.Applicant", "LicenseClass"]);
            if (license == null)
                return Result<GetInternationalLicenseDTO>.Failure(["No License Found!"]);


            var licenseDTO = new GetInternationalLicenseDTO
            {
                LicenseId = license.LicenseId,
                IssueDate = license.IssueDate.ToString("d"),
                ExpirationDate = license.IssueDate.AddYears(license.LicenseClass?.ValidityPeriod ?? 0).ToString("d"), // Default to 5 years if null
                DriverName = $"{license.Driver?.Applicant?.Fname} {license.Driver?.Applicant?.Sname} {license.Driver?.Applicant?.Tname} {license.Driver?.Applicant?.Lname}".Trim(),
                DriverDateOfBirth = license.Driver?.Applicant?.BirthDate.ToString("d"),
                Notes = license.Notes,
                PaidFees = license.PaidFees,
            };

            return Result<GetInternationalLicenseDTO>.Success(licenseDTO);
        }
    }
}
