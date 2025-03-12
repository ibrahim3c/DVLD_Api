using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using Microsoft.VisualBasic;

namespace DVLD.Core.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUOW uow;

        public ApplicationService(IUOW uow)
        {
            this.uow = uow;
        }
        // applicationType
        public async Task<Result<int>> AddAppTypeAync( TypeDTO appTypeDTO)
        {
            var errors = ObjectValidator.Validate<TypeDTO>(appTypeDTO);
            if (await IsTitleTaken(appTypeDTO.Title))
            {
                errors.Add( "this title is already exist");
            }
            if (errors.Any()) {
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
            if( ! await uow.appTypeRepository.AnyAsync())
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

        // application
        public async Task<Result<ApplicationDTO>> GetApplicationByIdAsync(int id)
        {
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id, ["Applicant", "AppType", "LicenseClass"]);
            if (app == null)
                return Result<ApplicationDTO>.Failure(["Application not Found"]);
            var fullName= app.Applicant.Fname??"" + " " + app.Applicant.Sname??"" + " " + app.Applicant.Tname??"" + " " + app.Applicant.Lname??"";
            var appDTO = new ApplicationDTO
            {
                AppDate = app.AppDate,
                AppFee = app.AppFee,
                AppStatus = app.AppStatus,
                ApplicantName = fullName,
                ApplicationType = app.AppType.Title,
                LicenseClass = app.LicenseClass!=null?app.LicenseClass.Name:"no License"
            };
            return Result<ApplicationDTO>.Success(appDTO);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var app = await uow.ApplicationRepository.FindAsync(a => a.AppID == id);
            if (app == null)
                return Result.Failure(["Application not Found"]);
            uow.ApplicationRepository.Delete(app);
            uow.Complete();
            return Result.Success();
        }

        // NewLocalDrivingLicense
        public async Task<Result<int>>ApplyForNewLocalDrivingLincense(int applicantId,int licenseClassId)
        {
            if (!await uow.ApplicantRepository.AnyAsync(x => x.ApplicantId == applicantId))
                return Result<int>.Failure(["this applicant is not found!"]);

            if (!await uow.LicenseClassRepository.AnyAsync(x => x.Id == licenseClassId))
                return Result<int>.Failure(["this License Class is not found!"]);

            if (await uow.ApplicationRepository.AnyAsync(x => x.AppTypeID == (int)AppTypes.NewLocalDrivingLicense
                                                          && x.ApplicantId==applicantId
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
        public async Task<Result> ApproveTheApplicationAsync(int appId)
        {
            return await uow.ApplicationRepository.ChangeStatusAsync(appId, AppStatuses.Pending, AppStatuses.Approved);            
        }
        public async Task<Result> RejectTheApplicationAsync(int appId)
        {
            return await uow.ApplicationRepository.ChangeStatusAsync(appId, AppStatuses.Pending, AppStatuses.Rejected);
        }
    }
}
