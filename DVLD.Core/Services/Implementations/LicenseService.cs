using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using FluentValidation;

namespace DVLD.Core.Services.Implementations
{
    public class LicenseService : ILicenseService
    {
        private readonly IUOW uow;
        private readonly IDriverServices driverServices;
        private readonly ITestService testService;
        private readonly IValidator<DetainedLicenseDTO> detainedLicenseDTOvalidator;
        private readonly IValidator<AddLicenseDTO> addLicenseDTOValidator;
        private readonly IValidator<AddInternationalLicenseDTO> addInternationalLicenseDTOValidator;
        private readonly IValidator<RenewLicenseApplicationDTO> renewLicenseApplicationDTOValidator;

        public LicenseService(IUOW uOW,IDriverServices driverServices
            ,ITestService testService
            , IValidator<DetainedLicenseDTO >validator
            ,IValidator<AddLicenseDTO>addLicenseDTOValidator
            ,IValidator<AddInternationalLicenseDTO> addInternationalLicenseDTOValidator
            ,IValidator<RenewLicenseApplicationDTO> renewLicenseApplicationDTOValidator)
        {
            uow = uOW;
            this.driverServices = driverServices;
            this.testService = testService;
            this.detainedLicenseDTOvalidator = validator;
            this.addLicenseDTOValidator = addLicenseDTOValidator;
            this.addInternationalLicenseDTOValidator = addInternationalLicenseDTOValidator;
            this.renewLicenseApplicationDTOValidator = renewLicenseApplicationDTOValidator;
        }
        public async Task<Result<IEnumerable<GetLicenseDTO>>> GetAllLicensesAsync()
        {

            var licenses = await uow.LicenseRepository.GetAllAsync(["Driver.Applicant", "LicenseClass"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetLicenseDTO>>.Failure(["No Licenses Found!"]);

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
                IsDetained = license.IsDetained
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);
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
                IsDetained = license.IsDetained
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
                IsDetained = license.IsDetained
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
                IsDetained = license.IsDetained
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
                IsDetained = license.IsDetained
            }).ToList();

            return Result<IEnumerable<GetLicenseDTO>>.Success(licensesDTO);

        }

        public async Task<Result<int>> IssueLicenseFirstTimeAsync(AddLicenseDTO addLicenseDTO)
        {
            var validationResult = addLicenseDTOValidator.Validate(addLicenseDTO);

            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

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

                var licenseClass=await uow.LicenseClassRepository.GetByIdAsync(addLicenseDTO.LicenseClassId);
            if (licenseClass == null)
                return Result<int>.Failure(["License class is not found."]);

            var license = new License
            {
                AppId = addLicenseDTO.AppId,
                IssueDate = DateTime.UtcNow,
                DriverId = result.Value,
                IssueReason = IssueReasons.FirstTime,
                LicenseClassId = addLicenseDTO.LicenseClassId,
                Notes = addLicenseDTO.Notes,
                PaidFees = addLicenseDTO.PaidFees,
                ExpirationDate=DateTime.UtcNow.AddYears(licenseClass?.ValidityPeriod ?? 0)
            };

            await uow.LicenseRepository.AddAsync(license);

            await uow.ApplicationRepository.ChangeStatusAsync(addLicenseDTO.AppId, AppStatuses.Completed);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);

        }

        // InternationalLicense
        public async Task<Result<int>> IssueInternationalLicenseAsync(AddInternationalLicenseDTO addLicenseDTO)
        {
            var validationResult=addInternationalLicenseDTOValidator.Validate(addLicenseDTO);
            if (!validationResult.IsValid)
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

            var application = await uow.ApplicationRepository.FindAsync(a => a.AppID == addLicenseDTO.AppId && a.AppStatus==AppStatuses.Approved, ["Applicant"]);
            if (application == null)
                return Result<int>.Failure(["No Approved Application Found"]);


            var driver = await uow.DriverRepository.FindAsync(d => d.applicantId == application.ApplicantId);
            if (driver != null)
            {
                return Result<int>.Failure(["No Driver found for this application"]);
            }


            //TODO:Determine the validityPeriod of the International License default 10 yeas
            var license = new License
            {
                AppId = addLicenseDTO.AppId,
                IssueDate = DateTime.UtcNow,
                DriverId = driver.DriverId,
                IssueReason = IssueReasons.InternationalLicenseFirstTime,
                Notes = addLicenseDTO.Notes,
                PaidFees = addLicenseDTO.PaidFees ,//Todo CHECK FOR THIS 
                ExpirationDate = DateTime.UtcNow.AddYears(10)
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
                DriverId=driverId
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
                DriverId = driverId

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
                DriverId = license.DriverId
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
                DriverId=driverId
            };

            return Result<GetInternationalLicenseDTO>.Success(licenseDTO);
        }

        // renewLicense
        public async Task<Result<int>> RenewLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO)
        {
            var validationResult = renewLicenseApplicationDTOValidator.Validate(renewLicenseApplicationDTO);

            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var application = await uow.RenewLicenseApplicationRepository.FindAsync(a => a.AppID == renewLicenseApplicationDTO.ApplicationId&& a.AppStatus == AppStatuses.Approved, ["ExpiredLicense"]);
            if (application == null)
                return Result<int>.Failure(["No Approved Application Found"]);

            // in activate this license
            var Exlicense = application.ExpiredLicense;
            //Exlicense.IsValid = false;

            var licenseClass = await uow.LicenseClassRepository.GetByIdAsync(Exlicense.LicenseClassId);
            if (licenseClass == null)
                return Result<int>.Failure(["License class is not found."]);


            var license = new License
            {
                AppId = application.AppID,
                IssueDate = DateTime.UtcNow,
                DriverId = Exlicense.DriverId,
                IssueReason = IssueReasons.Renewal,
                LicenseClassId = Exlicense.LicenseClassId,
                Notes = renewLicenseApplicationDTO.Notes,
                PaidFees = renewLicenseApplicationDTO.PaidFees,
                ExpirationDate = DateTime.UtcNow.AddYears(licenseClass?.ValidityPeriod ?? 0)
            };



            await uow.LicenseRepository.AddAsync(license);
            await uow.ApplicationRepository.ChangeStatusAsync(application.AppID, AppStatuses.Completed);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);

        }
        public async Task<Result<int>> ReplaceForDamagedLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO)
        {
            var validationResult = renewLicenseApplicationDTOValidator.Validate(renewLicenseApplicationDTO);

            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var application = await uow.RenewLicenseApplicationRepository.FindAsync(a => a.AppID == renewLicenseApplicationDTO.ApplicationId && a.AppStatus == AppStatuses.Approved 
            &&a.AppTypeID==(int)AppTypes.ReplacementForDamagedDrivingLicense, ["ExpiredLicense"]);
            if (application == null)
                return Result<int>.Failure(["No Approved Application Found"]);

            // in activate this license
            var Exlicense = application.ExpiredLicense;
            Exlicense!.IsDamaged = true;
            //Exlicense.IsValid = false;

            var licenseClass = await uow.LicenseClassRepository.GetByIdAsync(Exlicense.LicenseClassId);
            if (licenseClass == null)
                return Result<int>.Failure(["License class is not found."]);


            var license = new License
            {
                AppId = application.AppID,
                IssueDate = DateTime.UtcNow,
                DriverId = Exlicense.DriverId,
                IssueReason = IssueReasons.ReplacementDamaged,
                LicenseClassId = Exlicense.LicenseClassId,
                Notes = renewLicenseApplicationDTO.Notes,
                PaidFees = renewLicenseApplicationDTO.PaidFees,
                ExpirationDate = DateTime.UtcNow.AddYears(licenseClass?.ValidityPeriod ?? 0)
            };



            await uow.LicenseRepository.AddAsync(license);
            await uow.ApplicationRepository.ChangeStatusAsync(application.AppID, AppStatuses.Completed);
            uow.LicenseRepository.Update(Exlicense);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);
        }
        public async Task<Result<int>> ReplaceForLostLicenseAsync(RenewLicenseApplicationDTO renewLicenseApplicationDTO)
        {
            var validationResult = renewLicenseApplicationDTOValidator.Validate(renewLicenseApplicationDTO);

            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var application = await uow.RenewLicenseApplicationRepository.FindAsync(a => a.AppID == renewLicenseApplicationDTO.ApplicationId && a.AppStatus == AppStatuses.Approved
                       && a.AppTypeID == (int)AppTypes.ReplacementForLostDrivingLicense, ["ExpiredLicense"]);
            if (application == null)
                return Result<int>.Failure(["No Approved Application Found"]);

            // in activate this license
            var Exlicense = application.ExpiredLicense;
            Exlicense!.IsLost = true;
            //Exlicense.IsValid = false;

            var licenseClass = await uow.LicenseClassRepository.GetByIdAsync(Exlicense.LicenseClassId);
            if (licenseClass == null)
                return Result<int>.Failure(["License class is not found."]);


            var license = new License
            {
                AppId = application.AppID,
                IssueDate = DateTime.UtcNow,
                DriverId = Exlicense.DriverId,
                IssueReason = IssueReasons.ReplacementLost,
                LicenseClassId = Exlicense.LicenseClassId,
                Notes = renewLicenseApplicationDTO.Notes,
                PaidFees = renewLicenseApplicationDTO.PaidFees,
                ExpirationDate = DateTime.UtcNow.AddYears(licenseClass?.ValidityPeriod ?? 0)
            };



            await uow.LicenseRepository.AddAsync(license);
            await uow.ApplicationRepository.ChangeStatusAsync(application.AppID, AppStatuses.Completed);
            uow.LicenseRepository.Update(Exlicense);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);
        }


        //DetainedLicene
        public async Task<Result<int>> DetainLicenseAsync(DetainedLicenseDTO detainedLicenseDTO)
        {
            var validationResult = detainedLicenseDTOvalidator.Validate(detainedLicenseDTO);

            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == detainedLicenseDTO.LicenseId && l.IsValid);
            if(license is null)    
                return Result<int>.Failure(["No Active License Found"]);

            if(license.IsDetained)
                return Result<int>.Failure(["This License is already Detianed"]);

            license.IsDetained = true;

            var detainedLicense = new DetainedLicense
            {
                DetainedDate = DateTime.UtcNow,
                LicenseId = license.LicenseId,
                FineFees = detainedLicenseDTO.FineFees,
                Notes = detainedLicenseDTO.Notes,
                Reason = detainedLicenseDTO.Reason
            };
               uow.LicenseRepository.Update(license);
            await uow.DetainedLicenseRepository.AddAsync(detainedLicense);
            uow.Complete();

            return Result<int>.Success(detainedLicense.DetainedLicenseId);
        }
        public async Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesAsync()
        {
            var licenses = await uow.DetainedLicenseRepository.GetAllAsync(["License.Driver.Applicant", "ReleaseApplication"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetDetainedLicenseDTO>>.Failure(["No Licenses Found!"]);

            var licensesDTO = licenses.Select(license => new GetDetainedLicenseDTO
            {
                DetainedLicenseId = license.DetainedLicenseId,
                LicenseId=license.LicenseId,
                DriverName = string.Join(" ",
                                        new[] {
                                            license.License.Driver?.Applicant?.Fname,
                                            license.License.Driver?.Applicant?.Sname,
                                            license.License.Driver?.Applicant?.Tname,
                                            license.License.Driver?.Applicant?.Lname
                                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                                        ),
                FineFees = license.FineFees,
                IsReleased=license.IsReleased,
                Notes=license.Notes,
                Reason=license.Reason,
                ReleaseApplicationId=license.ReleaseApplicationId,
                ReleaseDate=license.ReleasedDate
            }).ToList();
            return Result<IEnumerable<GetDetainedLicenseDTO>>.Success(licensesDTO);
        }
        public async Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesByNationalNoAsync(string nationalNo)
        {
            var licenses = await uow.DetainedLicenseRepository.FindAllAsync(l=>l.License.Driver.Applicant.NationalNo==nationalNo,["License.Driver.Applicant", "ReleaseApplication"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetDetainedLicenseDTO>>.Failure(["No Licenses Found!"]);

            var licensesDTO = licenses.Select(license => new GetDetainedLicenseDTO
            {
                DetainedLicenseId = license.DetainedLicenseId,
                LicenseId = license.LicenseId,
                DriverName = string.Join(" ",
                                        new[] {
                                            license.License.Driver?.Applicant?.Fname,
                                            license.License.Driver?.Applicant?.Sname,
                                            license.License.Driver?.Applicant?.Tname,
                                            license.License.Driver?.Applicant?.Lname
                                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                                        ),
                FineFees = license.FineFees,
                IsReleased = license.IsReleased,
                Notes = license.Notes,
                Reason = license.Reason,
                ReleaseApplicationId = license.ReleaseApplicationId,
                ReleaseDate = license.ReleasedDate
            }).ToList();
            return Result<IEnumerable<GetDetainedLicenseDTO>>.Success(licensesDTO);
        }
        public async Task<Result<IEnumerable<GetDetainedLicenseDTO>>> GetAllDetainedLicensesByApplicantIdAsync(int applicantId)
        {
            var licenses = await uow.DetainedLicenseRepository.FindAllAsync(l => l.License.Driver.applicantId == applicantId, ["License.Driver.Applicant", "ReleaseApplication"]);
            if (!licenses.Any())
                return Result<IEnumerable<GetDetainedLicenseDTO>>.Failure(["No Licenses Found!"]);

            var licensesDTO = licenses.Select(license => new GetDetainedLicenseDTO
            {
                DetainedLicenseId = license.DetainedLicenseId,
                LicenseId = license.LicenseId,
                DriverName = string.Join(" ",
                                        new[] {
                                            license.License.Driver?.Applicant?.Fname,
                                            license.License.Driver?.Applicant?.Sname,
                                            license.License.Driver?.Applicant?.Tname,
                                            license.License.Driver?.Applicant?.Lname
                                        }.Where(x => !string.IsNullOrWhiteSpace(x))
                                        ),
                FineFees = license.FineFees,
                IsReleased = license.IsReleased,
                Notes = license.Notes,
                Reason = license.Reason,
                ReleaseApplicationId = license.ReleaseApplicationId,
                ReleaseDate = license.ReleasedDate
            }).ToList();
            return Result<IEnumerable<GetDetainedLicenseDTO>>.Success(licensesDTO);
        }

        public async Task<Result<int>> ReleaseLicenseAsync(int applicantionId)
        {
            var DetainedApp = await uow.ApplicationRepository.FindAsync(l => l.AppID == applicantionId &&l.AppStatus == AppStatuses.Approved && l.AppTypeID == (int)AppTypes.ReleaseDetainedDrivingLicense, ["DetainedLicense"]);
            if(DetainedApp is null)
                return Result<int>.Failure(["No Approved Application Found"]);
            var detainedLicense = DetainedApp.DetainedLicense;
            if (detainedLicense is null)
                return Result<int>.Failure(["No Detained License Found"]);
            var license = await uow.LicenseRepository.GetByIdAsync(detainedLicense.LicenseId);

            if(license is null)
                return Result<int>.Failure(["No License Found"]);

            detainedLicense.ReleaseApplicationId = applicantionId;
            detainedLicense.IsReleased = true;
            detainedLicense.ReleasedDate = DateTime.UtcNow;

            license.IsDetained = false;
            DetainedApp.AppStatus = AppStatuses.Completed;
           

             uow.DetainedLicenseRepository.Update(detainedLicense);
             uow.LicenseRepository.Update(license);
             uow.ApplicationRepository.Update(DetainedApp);
            uow.Complete();

            return Result<int>.Success(license.LicenseId);
        }
        }
}
