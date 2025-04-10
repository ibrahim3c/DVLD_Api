using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using DVLD.Core.Validators;
using FluentValidation;
using System.Collections.Generic;

namespace DVLD.Core.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUOW uow;
        private readonly IValidator<UpdateApplicationDTO> updateApplicationDTOValidator;

        public ApplicationService(IUOW uow, IValidator<UpdateApplicationDTO> UpdateApplicationDTOValidator)
        {
            this.uow = uow;
            updateApplicationDTOValidator = UpdateApplicationDTOValidator;
        }
        #region ManageApplicationTypes
        public async Task<Result<int>> AddAppTypeAync(TypeDTO appTypeDTO)
        {
            var errors = ObjectValidator.Validate<TypeDTO>(appTypeDTO);
            if (await IsTitleTaken(appTypeDTO.Title))
            {
                errors.Add("this title is already exist");
            }
            if (errors.Any())
            {
                return Result<int>.Failure(errors);
            }

            var appType = new ApplicationType
            {
                Description = appTypeDTO.Description,
                Title = appTypeDTO.Title,
                TypeFee = appTypeDTO.TypeFee
            };
            await uow.appTypeRepository.AddAsync(appType);
            uow.Complete();
            return Result<int>.Success(appType.Id);
        }
        private async Task<bool> IsTitleTaken(string title)
        {
            return await uow.appTypeRepository.AnyAsync(x => x.Title == title);
        }
        public async Task<Result> DeleteAppTypeAsync(int id)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(id);
            if (appType == null)
            {
                return Result.Failure(["No Application Type Found!"]);
            }
            uow.appTypeRepository.Delete(appType);
            uow.Complete();
            return Result.Success();
        }
        public async Task<Result<IEnumerable<TypeDTO>>> GetAllAppTypesAsync()
        {
            if (!await uow.appTypeRepository.AnyAsync())
            {
                return Result<IEnumerable<TypeDTO>>.Failure(["No Application Types Found"]);
            }
            var appTypes = (await uow.appTypeRepository.GetAllAsync()).Select(a => new TypeDTO
            {
                Description = a.Description,
                Title = a.Title,
                TypeFee = a.TypeFee
            });
            return Result<IEnumerable<TypeDTO>>.Success(appTypes);
        }
        public async Task<Result<TypeDTO>> GetAppTypeByIdAsync(int id)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(id);
            if (appType == null)
            {
                return Result<TypeDTO>.Failure(["No Application Type Found!"]);
            }

            var appTypeDTO = new TypeDTO
            {
                Description = appType.Description,
                Title = appType.Title,
                TypeFee = appType.TypeFee
            };
            return Result<TypeDTO>.Success(appTypeDTO);

        }
        public async Task<Result> UpdateAppTypeAsync(int id, TypeDTO appTypeDTO)
        {
            var errors = ObjectValidator.Validate(appTypeDTO);

            var appType = await uow.appTypeRepository.GetByIdAsync(id);
            if (appType == null)
            {
                errors.Add("No Application Type Found!");
            }

            bool isTitleExists = await uow.appTypeRepository
                .AnyAsync(t => t.Id != id && t.Title == appTypeDTO.Title);

            if (isTitleExists)
            {
                errors.Add("An Application Type with this title already exists!");
            }

            if (errors.Any())
            {
                return Result.Failure(errors);
            }

            appType.Title = appTypeDTO.Title;
            appType.Description = appTypeDTO.Description;
            appType.TypeFee = appTypeDTO.TypeFee;

            uow.appTypeRepository.Update(appType);
            uow.Complete();

            return Result.Success();
        }

        #endregion

        #region ManageApps
        public async Task<Result<ApplicationDTO>> GetApplicationByIdAsync(int id)
        {
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id, ["Applicant", "AppType", "LicenseClass"]);
            if (app == null)
                return Result<ApplicationDTO>.Failure(["Application not Found"]);
            var fullName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim();


            var appDTO = new ApplicationDTO
            {
                AppId=app.AppID,
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
            };
            return Result<ApplicationDTO>.Success(appDTO);
        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a=>a.NationalNo==nationalNo))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applicant with this National Number"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.NationalNo == nationalNo, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });

            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a => a.ApplicantId == applicantId))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applicant with this National Number"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.ApplicantId == applicantId, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });
            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationAsync()
        {
            if (!await uow.ApplicationRepository.AnyAsync())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applications Found"]);

            var apps = (await uow.ApplicationRepository.GetAllAsync(["Applicant", "AppType", "LicenseClass"])).Select(app=> new ApplicationDTO
            {
                AppId = app.AppID,
                AppDate = app.AppDate,
                AppFee=app.AppFee,
                AppStatus = app.AppStatus,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
                ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim()
            });
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }
        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationWithStatusAsync(string status)
        {
             status = status.Trim('"');
            if (!AppStatuses.IsValidStatus(status))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["Invalid status"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.AppStatus == status, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });
            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);

        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationAsync(int appType)
        {
            if (!await uow.ApplicationRepository.AnyAsync())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applications Found"]);

            var apps = (await uow.ApplicationRepository.FindAllAsync(a=>a.AppTypeID==appType,["Applicant", "AppType", "LicenseClass"])).Select(app => new ApplicationDTO
            {
                AppId = app.AppID,
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
                ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
            });
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }
        public async Task<Result<ApplicationDTO>> GetApplicationByIdAsync(int id,int appType)
        {
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id && a.AppTypeID==appType, ["Applicant", "AppType", "LicenseClass"]);
            if (app == null)
                return Result<ApplicationDTO>.Failure(["Application not Found"]);
            var fullName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim();


            var appDTO = new ApplicationDTO
            {
                AppId = app.AppID,
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
            };
            return Result<ApplicationDTO>.Success(appDTO);
        }


        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicationWithStatusAsync(string status, int appType)
        {
            status = status.Trim('"');
            if (!AppStatuses.IsValidStatus(status))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["Invalid status"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.AppStatus == status && a.AppTypeID==appType, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });
            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByNationalNoAsync(string nationalNo, int appType)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a => a.NationalNo == nationalNo))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applicant with this National Number"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.NationalNo == nationalNo && a.AppTypeID==appType, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });

            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllApplicantApplicationsByIdAsync(int applicantId, int appType)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a => a.ApplicantId == applicantId))
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applicant with this National Number"]);
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.ApplicantId == applicantId && a.AppTypeID==appType, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new ApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo
                });
            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }

        public async Task<Result> DeleteApplicationAsync(int id)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == id
                 && a.AppStatus == AppStatuses.Completed))
                return Result.Failure(["this Application is Completed"]);

            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id);
            if (app == null)
                return Result.Failure(["Application not Found"]);
            uow.ApplicationRepository.Delete(app);
            uow.Complete();
            return Result.Success();
        }
        public async Task<Result> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO)
        {
            var validationResult = updateApplicationDTOValidator.Validate(updateApplicationDTO);
            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == id&&
                 a.AppStatus == AppStatuses.Completed))
                return Result.Failure(["this Application is Completed"]);


            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id);
            if (app == null)
                return Result.Failure(["Application not Found"]);

            app.AppStatus = updateApplicationDTO.AppStatus;
            app.AppDate = updateApplicationDTO.AppDate;
            var appType = await GetAppTypeByIdAsync(updateApplicationDTO.AppTypeId);
            if (appType == null)
                return Result.Failure(["InValid Application Type"]);

            app.AppTypeID = updateApplicationDTO.AppTypeId;
            app.AppFee = appType.Value!.TypeFee;

            uow.ApplicationRepository.Update(app);
            uow.Complete();
            return Result.Success();
        }
        public async Task<Result> ApproveTheApplicationAsync(int appId)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                && a.AppStatus == AppStatuses.Completed))
                return Result.Failure(["this Application is Completed"]);

            return await uow.ApplicationRepository.ChangeStatusAsync(appId, AppStatuses.Pending, AppStatuses.Approved);
        }
        public async Task<Result> RejectTheApplicationAsync(int appId)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                 && a.AppStatus == AppStatuses.Completed))
                return Result.Failure(["this Application is Completed"]);

            return await uow.ApplicationRepository.ChangeStatusAsync(appId, AppStatuses.Pending, AppStatuses.Rejected);
        }

        #endregion

        #region LocalAppLicense
        public async Task<Result<int>> ApplyForNewLocalDrivingLincense(int applicantId, int licenseClassId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(x => x.ApplicantId == applicantId))
                return Result<int>.Failure(["this applicant is not found!"]);

            if (!await uow.LicenseClassRepository.AnyAsync(x => x.Id == licenseClassId))
                return Result<int>.Failure(["this License Class is not found!"]);

            if (await uow.ApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.NewLocalDrivingLicense
                                                          && x.ApplicantId == applicantId
                                                          && x.LicenseClassId == licenseClassId
                                                          && x.AppStatus == AppStatuses.Pending))

                return Result<int>.Failure(["U already have a pending application for this license class."]);

            var driver = await uow.DriverRepository.FindAsync(l => l.applicantId == applicantId);

            if (driver is not null && await uow.LicenseRepository.AnyAsync(l=>l.LicenseClassId==licenseClassId
            && l.DriverId==driver.DriverId))
            {
                return Result<int>.Failure(["U already have a License from same license class."]);
            }

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.NewLocalDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new Application
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = (int)AppTypes.NewLocalDrivingLicense,
                LicenseClassId = licenseClassId,
            };
            await uow.ApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }
        public async Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesAsync()
        {
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.AppID == 1, ["Applicant", "AppType", "LicenseClass","TestAppointments.Test"]))
                .Select(x => new LocalAppLicenseDTO
                {
                    AppId=x.AppID,
                    AppDate = x.AppDate,
                    AppFee = x.AppFee,
                    ApplicantName = $"{x.Applicant.Fname ?? ""} {x.Applicant.Sname ?? ""} {x.Applicant.Tname ?? ""} {x.Applicant.Lname ?? ""}",
                    ApplicationType = x.AppType.Title,
                    AppStatus = x.AppStatus,
                    LicenseClass = x.LicenseClass!.Name,
                    NationalNumber = x.Applicant.NationalNo,
                    PassedTests=x.TestAppointments.Count(ta=>ta.Test !=null && ta.Test.TestResult)

                });
            if (!apps.Any())
                return Result<IEnumerable<LocalAppLicenseDTO>>.Failure(["No Applications Found"]);
            return Result<IEnumerable<LocalAppLicenseDTO>>.Success(apps);
        }

        public async Task<Result<LocalAppLicenseDTO>> GetLocalAppLicenseByIdAsync(int id)
        {
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id, ["Applicant", "AppType", "LicenseClass", "TestAppointments.Test"]);
            if (app == null)
                return Result<LocalAppLicenseDTO>.Failure(["Application not Found"]);
            var fullName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim();

            int passedTests = 0;
            if(app.TestAppointments.Any())
                passedTests = app.TestAppointments.Count(ta => ta.Test != null && ta.Test.TestResult);

            var appDTO = new LocalAppLicenseDTO
            {
                AppId = app.AppID,
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
                PassedTests= passedTests,
            };
            return Result<LocalAppLicenseDTO>.Success(appDTO);
        }

        public async Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesWithsByNationalNoAsync(string nationalNo)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a => a.NationalNo == nationalNo))
                return Result<IEnumerable<LocalAppLicenseDTO>>.Failure(["No Applicant with this National Number"]);

            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.NationalNo == nationalNo && a.AppTypeID==(int)AppTypes.NewLocalDrivingLicense, ["Applicant", "AppType", "LicenseClass", "TestAppointments.Test"])
                ).Select(app => new LocalAppLicenseDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo,
                    PassedTests = app.TestAppointments.Count(ta => ta.Test != null && ta.Test.TestResult)

                });

            if (!apps.Any())
                return Result<IEnumerable<LocalAppLicenseDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<LocalAppLicenseDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<LocalAppLicenseDTO>>> GetAllLocalAppLicensesByApplicantIdAsync(int applicantId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(a => a.ApplicantId == applicantId))
                return Result<IEnumerable<LocalAppLicenseDTO>>.Failure(["No Applicant Found"]);

            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.Applicant.ApplicantId == applicantId && a.AppTypeID == (int)AppTypes.NewLocalDrivingLicense, ["Applicant", "AppType", "LicenseClass", "TestAppointments.Test"])
                ).Select(app => new LocalAppLicenseDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo,
                    PassedTests = app.TestAppointments.Count(ta => ta.Test != null && ta.Test.TestResult)

                });

            if (!apps.Any())
                return Result<IEnumerable<LocalAppLicenseDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<LocalAppLicenseDTO>>.Success(apps);
        }



        #endregion

        #region RetakeTest
        public async Task<Result<int>> ApplyForRetakeTestApp(int applicantId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(x => x.ApplicantId == applicantId))
                return Result<int>.Failure(["this applicant is not found!"]);

           

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.RetakeTest);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new Application
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Approved,
                AppTypeID = (int)AppTypes.RetakeTest,
            };
            await uow.ApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }

        #endregion

        #region InternationalLicenseApp
        public async Task<Result<int>> ApplyForNewInternationalLicenseApplicationAsync(int applicantId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(x => x.ApplicantId == applicantId))
                return Result<int>.Failure(["this applicant is not found!"]);


            if (await uow.ApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.NewInternationalDrivingLicense
                                                          && x.ApplicantId == applicantId
                                                          && x.AppStatus == AppStatuses.Pending))
                return Result<int>.Failure(["U already have a pending application."]);


            var driver = await uow.DriverRepository.FindAsync(l => l.applicantId == applicantId, ["Applicant"]);
            if(driver is null)
                return Result<int>.Failure(["U must have a license from classThree-Ordinary."]);

            if (await uow.LicenseRepository.AnyAsync(l => l.Application.AppTypeID == (int)AppTypes.NewInternationalDrivingLicense
            && l.DriverId == driver.DriverId))
            {
                return Result<int>.Failure(["U already have an International License ."]);
            }


            var driverLicense = await uow.LicenseRepository.FindAsync(x => x.LicenseClassId == (int)LicenseClasses.ClassThree_Oridinary
                                                       && x.DriverId == driver.DriverId);
            if(driverLicense is null)
                return Result<int>.Failure(["U must have a license from classThree-Ordinary."]);

            if (!driverLicense.IsValid)
                return Result<int>.Failure(["U must have an acive license from classThree-Ordinary."]);


            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.NewInternationalDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new Application
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = (int)AppTypes.NewInternationalDrivingLicense,
            };
            await uow.ApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }
        #endregion

        #region RenewLicenseApplication
        public async Task<Result<int>> ApplyForRenewLicenseApplicationAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == licenseId , ["Application.Applicant", "LicenseClass"]);
            if (license is null)
                return Result<int>.Failure(["License not Found!"]);

            if (await uow.RenewLicenseApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.RenewDrivingLicense
                                              && x.ExpiredLicenseId == licenseId
                                              && x.AppStatus == AppStatuses.Pending))
                return Result<int>.Failure(["U already have a pending application."]);


            var isExpired = DateTime.UtcNow > license.IssueDate.AddYears(license.LicenseClass.ValidityPeriod);
            if (!isExpired)
                return Result<int>.Failure(["The license is not expired!"]);

            var applicantId = license.Application.ApplicantId;

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.RenewDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new RenewLicenseApplication
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = (int)AppTypes.RenewDrivingLicense,
                LicenseClassId = license.LicenseClassId,
                ExpiredLicenseId = licenseId
            };
            await uow.RenewLicenseApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }

        public async Task<Result<GetRenewLicenseApplicationDTO>> GetRewLicenseAppLicenseByIdAsync(int id,int appTypeId)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(appTypeId);
            if(appType == null)
                return Result<GetRenewLicenseApplicationDTO>.Failure(["This Application Type is not valid"]);

            var app = await uow.RenewLicenseApplicationRepository.FindAsync(a => a.AppID == id && a.AppTypeID== appTypeId, ["Applicant", "AppType", "LicenseClass"]);
            if (app == null)
                return Result<GetRenewLicenseApplicationDTO>.Failure(["Application not Found"]);
            var fullName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim();


            var appDTO = new GetRenewLicenseApplicationDTO
            {
                AppId = app.AppID,
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
                OldLicenseId = app.ExpiredLicenseId
            };
            return Result<GetRenewLicenseApplicationDTO>.Success(appDTO);
        }

        public async Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppLicensesAsync(int appTypeId)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(appTypeId);
            if (appType == null)
                return Result < IEnumerable < GetRenewLicenseApplicationDTO >>.Failure(["This Application Type is not valid"]);

            var apps = (await uow.RenewLicenseApplicationRepository.FindAllAsync(a => a.AppID == 1 &&a.AppTypeID==appTypeId, ["Applicant", "AppType", "LicenseClass"]))
                .Select(x => new GetRenewLicenseApplicationDTO
                {
                    AppId = x.AppID,
                    AppDate = x.AppDate,
                    AppFee = x.AppFee,
                    ApplicantName = $"{x.Applicant.Fname ?? ""} {x.Applicant.Sname ?? ""} {x.Applicant.Tname ?? ""} {x.Applicant.Lname ?? ""}",
                    ApplicationType = x.AppType.Title,
                    AppStatus = x.AppStatus,
                    LicenseClass = x.LicenseClass!.Name,
                    NationalNumber = x.Applicant.NationalNo,
                    OldLicenseId = x.ExpiredLicenseId
                });
            if (!apps.Any())
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["No Applications Found"]);
            return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppsWithsByNationalNoAsync(string nationalNo,int appTypeId)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(appTypeId);
            if (appType == null)
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["This Application Type is not valid"]);

            if (!await uow.ApplicantRepository.AnyAsync(a => a.NationalNo == nationalNo))
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["No Applicant with this National Number"]);

            var apps = (await uow.RenewLicenseApplicationRepository.FindAllAsync(a => a.Applicant.NationalNo == nationalNo && a.AppTypeID == appTypeId, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new GetRenewLicenseApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo,
                    OldLicenseId =app.ExpiredLicenseId
                });

            if (!apps.Any())
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Success(apps);
        }

        public async Task<Result<IEnumerable<GetRenewLicenseApplicationDTO>>> GetAllRewLicenseAppsByApplicantIdAsync(int applicantId,int appTypeId)
        {
            var appType = await uow.appTypeRepository.GetByIdAsync(appTypeId);
            if (appType == null)
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["This Application Type is not valid"]);

            if (!await uow.ApplicantRepository.AnyAsync(a => a.ApplicantId == applicantId))
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["No Applicant Found"]);

            var apps = (await uow.RenewLicenseApplicationRepository.FindAllAsync(a => a.Applicant.ApplicantId == applicantId && a.AppTypeID ==appTypeId, ["Applicant", "AppType", "LicenseClass"])
                ).Select(app => new GetRenewLicenseApplicationDTO
                {
                    AppId = app.AppID,
                    AppDate = app.AppDate,
                    AppFee = app.AppFee,
                    AppStatus = app.AppStatus,
                    ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
                    ApplicationType = app.AppType.Title,
                    LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                    NationalNumber = app.Applicant.NationalNo,
                    OldLicenseId = app.ExpiredLicenseId
                });

            if (!apps.Any())
                return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Failure(["No Application Found"]);
            return Result<IEnumerable<GetRenewLicenseApplicationDTO>>.Success(apps);
        }

        #endregion

        public async Task<Result<int>> ApplyForReplacementDamagedLicenseApplicationAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == licenseId && l.IsValid, ["Application.Applicant", "LicenseClass"]);
            if (license is null)
                return Result<int>.Failure(["No Active License Found!"]);

            if (await uow.RenewLicenseApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.ReplacementForDamagedDrivingLicense
                                              && x.ExpiredLicense == license
                                              && x.AppStatus == AppStatuses.Pending))
                return Result<int>.Failure(["U already have a pending application."]);

            var applicantId = license.Application.ApplicantId;

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.ReplacementForDamagedDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new RenewLicenseApplication
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = appType.Id,
                LicenseClassId = license.LicenseClassId,
                ExpiredLicenseId = licenseId
            };
            await uow.RenewLicenseApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }
        public async Task<Result<int>> ApplyForReplacementLostLicenseApplicationAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == licenseId && l.IsValid, ["Application.Applicant", "LicenseClass"]);
            if (license is null)
                return Result<int>.Failure(["No Active License Found!"]);

            if (await uow.RenewLicenseApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.ReplacementForLostDrivingLicense
                                  && x.ExpiredLicense == license
                                  && x.AppStatus == AppStatuses.Pending))
                return Result<int>.Failure(["U already have a pending application."]);

            var applicantId = license.Application.ApplicantId;

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.ReplacementForLostDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new RenewLicenseApplication
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = appType.Id,
                LicenseClassId = license.LicenseClassId,
                ExpiredLicenseId = licenseId
            };
            await uow.RenewLicenseApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }

        // release LicenseApp
        public async Task<Result<int>> ApplyForReleaseLicenseApplicationAsync(int licenseId)
        {
            var license = await uow.LicenseRepository.FindAsync(l => l.LicenseId == licenseId && l.IsDetained, ["Application.Applicant", "LicenseClass", "DetainedLicense"]);
            if (license is null)
                return Result<int>.Failure(["No Detained License Found!"]);

            if (await uow.DetainedLicenseRepository.FindAsync(x => x.LicenseId == licenseId
            && x.ReleaseApplication!.AppTypeID == (int)AppTypes.ReleaseDetainedDrivingLicense
             && x.ReleaseApplication.AppStatus == AppStatuses.Pending, ["ReleaseApplication"]) is not null)

            //if (await uow.ApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.ReleaseDetainedDrivingLicense
            //                      && x.DetainedLicense != null && x.DetainedLicense.LicenseId == licenseId
            //                      && x.AppStatus == AppStatuses.Pending))
                return Result<int>.Failure(["U already have a pending application."]);

            var applicantId = license.Application.ApplicantId;

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.ReleaseDetainedDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new Application
            {
                AppDate = DateTime.UtcNow,
                AppFee = appType.TypeFee,
                ApplicantId = applicantId,
                AppStatus = AppStatuses.Pending,
                AppTypeID = appType.Id,
                LicenseClassId = license.LicenseClassId
            };
            await uow.ApplicationRepository.AddAsync(application);
            uow.Complete();
            return Result<int>.Success(application.AppID);
        }

    }
}
