using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Models;

namespace DVLD.Core.Services.Interfaces
{
    public interface ITestService
    {
        #region TestType
        Task<Result<Test>> GetTestResultByTestAppoinmentId(int testAppoinmentId);
        Task<Result<IEnumerable<TypeDTO>>> GetAllTestTypesAsync();
        Task<Result<TypeDTO>> GetTestTypeByIdAsync(int id);
        Task<Result> UpdateTestTypeAsync(int id, TypeDTO testTypeDTO);
        Task<Result> DeleteTestTypeAsync(int id);
        Task<Result<int>> AddTestTypeAync(TypeDTO testTypeDTO);

        #endregion
        #region Test and TeatAppointment
        // Tests
        Task<Result<int>> ScheduleVisionTestAsync(int appId, int applicantId);
        Task<Result<int>> ScheduleWrittenTestAsync(int appId, int applicantId);
        Task<Result<int>> SchedulePracticalTestAsync(int appId, int applicantId);
        Task<Result> EditTestAppointmentAsync(int testAppointmentId, EditTestAppointmentDTO editTestAppointmentDTO);
        Task<Result> CompleteTestAsync(CompleteTestDTO completeTestDTO);
        Task<Result> IsAllTestPassed(int applicantId,int appId);
        Task<Result> IsTestPassed(int applicantId, int appId, TestTypes testType);
        Task<Result<TestAppointmentDTO>> GetTestAppointmentById(int testAppointmentId);
        Task<Result<List<TestAppointmentDTO>>> GetAllTestAppoinments();
        Task<Result<List<TestAppointmentDTO>>> GetAllTestAppoinmentByApplicantId(int applicantId);

        #endregion
    }
}
