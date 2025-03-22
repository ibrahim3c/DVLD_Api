using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;

namespace DVLD.Core.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUOW uow;

        public ApplicationService(IUOW uow)
        {
            this.uow = uow;
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
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo
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
                AppDate=app.AppDate,
                AppFee=app.AppFee,
                AppStatus = app.AppStatus,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass != null ? app.LicenseClass.Name : "no License",
                NationalNumber = app.Applicant.NationalNo,
                ApplicantName = $"{app.Applicant.Fname ?? ""} {app.Applicant.Sname ?? ""} {app.Applicant.Tname ?? ""} {app.Applicant.Lname ?? ""}".Trim(),
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
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id);
            if (app == null)
                return Result.Failure(["Application not Found"]);
            uow.ApplicationRepository.Delete(app);
            uow.Complete();
            return Result.Success();
        }
        public async Task<Result> UpdateApplicationAsync(int id, UpdateApplicationDTO updateApplicationDTO)
        {
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
            return await uow.ApplicationRepository.ChangeStatusAsync(appId, AppStatuses.Pending, AppStatuses.Approved);
        }
        public async Task<Result> RejectTheApplicationAsync(int appId)
        {
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

            //TODO:check if he already has license with same class

            // Get the application fee
            var appType = await uow.appTypeRepository.GetByIdAsync((int)AppTypes.NewLocalDrivingLicense);
            if (appType == null)
                return Result<int>.Failure(["Application type not found!"]);

            var application = new Application
            {
                AppDate = DateTime.Now,
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

        public async Task<Result<IEnumerable<ApplicationDTO>>> GetAllLocalApplicationsLicense()
        {
            var apps = (await uow.ApplicationRepository.FindAllAsync(a => a.AppID == 1, ["Applicant", "AppType", "LicenseClass"]))
                .Select(x => new ApplicationDTO
                {
                    AppDate = x.AppDate,
                    AppFee = x.AppFee,
                    ApplicantName = $"{x.Applicant.Fname ?? ""} {x.Applicant.Sname ?? ""} {x.Applicant.Tname ?? ""} {x.Applicant.Lname ?? ""}",
                    ApplicationType = x.AppType.Title,
                    AppStatus = x.AppStatus,
                    LicenseClass = x.LicenseClass!.Name,
                    NationalNumber = x.Applicant.NationalNo
                });
            if (!apps.Any())
                return Result<IEnumerable<ApplicationDTO>>.Failure(["No Applications Found"]);
            return Result<IEnumerable<ApplicationDTO>>.Success(apps);
        }



        #endregion

    }
}
