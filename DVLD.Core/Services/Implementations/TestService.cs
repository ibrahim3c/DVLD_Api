using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;

namespace DVLD.Core.Services.Implementations
{
    public class TestService:ITestService
    {
        private readonly IUOW uow;

        public TestService(IUOW uow)
        {
            this.uow = uow;
        }

        public async Task<Result<int>> AddTestTypeAync(TypeDTO testTypeDTO)
        {
            var errors = ObjectValidator.Validate<TypeDTO>(testTypeDTO);
            if (await IsTitleTaken(testTypeDTO.Title))
            {
                errors.Add("this title is already exist");
            }
            if (errors.Any())
            {
                return Result<int>.Failure(errors);
            }

            var appType = new ApplicationType
            {
                Description = testTypeDTO.Description,
                Title = testTypeDTO.Title,
                TypeFee = testTypeDTO.TypeFee
            };
            await uow.appTypeRepository.AddAsync(appType);
            uow.Complete();
            return Result<int>.Success(appType.Id);
        }

        private async Task<bool> IsTitleTaken(string title)
        {
            return await uow.appTypeRepository.AnyAsync(x => x.Title == title);
        }

        public async Task<Result> DeleteTestTypeAsync(int id)
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

        public async Task<Result<IEnumerable<TypeDTO>>> GetAllTestTypesAsync()
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

        public async Task<Result<TypeDTO>> GetTestTypeByIdAsync(int id)
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

        public async Task<Result> UpdateTestTypeAsync(int id, TypeDTO testTypeDTO)
        {
            var errors = ObjectValidator.Validate(testTypeDTO);

            var TestType = await uow.testTypeRepository.GetByIdAsync(id);
            if (TestType == null)
            {
                errors.Add("No Testlication Type Found!");
            }

            bool isTitleExists = await uow.testTypeRepository
                .AnyAsync(t => t.Id != id && t.Title == testTypeDTO.Title);

            if (isTitleExists)
            {
                errors.Add("An Testlication Type with this title already exists!");
            }

            if (errors.Any())
            {
                return Result.Failure(errors);
            }

            TestType.Title = testTypeDTO.Title;
            TestType.Description = testTypeDTO.Description;
            TestType.TypeFee = testTypeDTO.TypeFee;

            uow.testTypeRepository.Update(TestType);
            uow.Complete();

            return Result.Success();
        }
    }
}
