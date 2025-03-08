using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DVLD.Core.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUOW uow;

        public ApplicationService(IUOW uow)
        {
            this.uow = uow;
        }

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

    }
}
