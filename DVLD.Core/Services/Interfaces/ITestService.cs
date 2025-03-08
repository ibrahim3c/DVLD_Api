using DVLD.Core.DTOs;
using DVLD.Core.Helpers;

namespace DVLD.Core.Services.Interfaces
{
    public interface ITestService
    {
        Task<Result<IEnumerable<TypeDTO>>> GetAllTestTypesAsync();
        Task<Result<TypeDTO>> GetTestTypeByIdAsync(int id);
        Task<Result> UpdateTestTypeAsync(int id, TypeDTO testTypeDTO);
        Task<Result> DeleteTestTypeAsync(int id);
        Task<Result<int>> AddTestTypeAync(TypeDTO testTypeDTO);
    }
}
