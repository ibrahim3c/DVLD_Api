using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;

namespace DVLD.Core.Services.Implementations
{
    public class DriverService : IDriverServices
    {
        private readonly IApplicantService applicantService;
        private readonly IUOW uOW;

        public DriverService(IApplicantService applicantService,IUOW uOW)
        {
            this.applicantService = applicantService;
            this.uOW = uOW;
        }

        public async Task<Result<int>> AddDriverAsync(int applicantId)
        {
            var result= await applicantService.GetByIdAsync(applicantId);
            if (result == null)
                return Result<int>.Failure(["No Applicant Found"]);
            if(await uOW.DriverRepository.AnyAsync(d=>d.applicantId==applicantId))
                return Result<int>.Failure(["This Driver is already exist"]);
            var driver = new Driver
            {
                applicantId = applicantId
            };
            await uOW.DriverRepository.AddAsync(driver);
            uOW.Complete();

            return Result<int>.Success(driver.DriverId);
        }

        public async Task< Result<IEnumerable<GetApplicantDTO>>> GetAllDriversAsync()
        {
            var drivers = await uOW.DriverRepository.GetAllAsync(["Applicant"]);
            if (!drivers.Any())
                return Result<IEnumerable<GetApplicantDTO>>.Failure(["No Drivers Found!"]);
            var driversDTO = drivers.Select(d => new GetApplicantDTO
            {
                DriverId=d.DriverId,
                applicantId=d.applicantId,
                Address = d.Applicant.Address,
                BirthDate = d.Applicant.BirthDate,
                CountryId = d.Applicant.CountryId,
                FullName = d.Applicant.Fname + " " + d.Applicant.Sname + " " + d.Applicant.Tname + " " + d.Applicant.Lname,
                Gender = d.Applicant.Gender,
                NationalNo = d.Applicant.NationalNo
            }).ToList();

            return Result<IEnumerable<GetApplicantDTO>>.Success(driversDTO);

        }

        public async Task<Result<GetApplicantDTO>> GetDriverByApplicantIdAsync(int applicantId)
        {
            var d = await uOW.DriverRepository.FindAsync(a => a.applicantId == applicantId, ["Applicant"]);
            if (d is null)
                return Result<GetApplicantDTO>.Failure(["No Driver Found"]);

            var driversDTO = new GetApplicantDTO
            {
                DriverId = d.DriverId,
                applicantId = d.applicantId,
                Address = d.Applicant.Address,
                BirthDate = d.Applicant.BirthDate,
                CountryId = d.Applicant.CountryId,
                FullName = d.Applicant.Fname + " " + d.Applicant.Sname + " " + d.Applicant.Tname + " " + d.Applicant.Lname,
                Gender = d.Applicant.Gender,
                NationalNo = d.Applicant.NationalNo
            };

            return Result<GetApplicantDTO>.Success(driversDTO);
        }

        public async Task<Result<GetApplicantDTO>> GetDriverByApplicantNationalNoAsync(string nationalNo)
        {
            var applicant = uOW.ApplicantRepository.FindAsync(a=>a.NationalNo==nationalNo);
            if (applicant is null)
                return Result<GetApplicantDTO>.Failure(["No Applicant Found"]);

            var d = await uOW.DriverRepository.FindAsync(a => a.applicantId == applicant.Id, ["Applicant"]);
            if (d is null)
                return Result<GetApplicantDTO>.Failure(["No Driver Found"]);

            var driversDTO = new GetApplicantDTO
            {
                DriverId = d.DriverId,
                applicantId = d.applicantId,
                Address = d.Applicant.Address,
                BirthDate = d.Applicant.BirthDate,
                CountryId = d.Applicant.CountryId,
                FullName = d.Applicant.Fname + " " + d.Applicant.Sname + " " + d.Applicant.Tname + " " + d.Applicant.Lname,
                Gender = d.Applicant.Gender,
                NationalNo = d.Applicant.NationalNo
            };

            return Result<GetApplicantDTO>.Success(driversDTO);
        }

        public async Task<Result<GetApplicantDTO>> GetDriversByIdAsync(int id)
        {
            var d = await uOW.DriverRepository.FindAsync(a => a.DriverId == id, ["Applicant"]);
            if (d is null)
                return Result<GetApplicantDTO>.Failure(["No Driver Found"]);

            var driversDTO = new GetApplicantDTO
            {
                applicantId=d.applicantId,
                DriverId = d.DriverId,
                Address = d.Applicant.Address,
                BirthDate = d.Applicant.BirthDate,
                CountryId = d.Applicant.CountryId,
                FullName = d.Applicant.Fname + " " + d.Applicant.Sname + " " + d.Applicant.Tname + " " + d.Applicant.Lname,
                Gender = d.Applicant.Gender,
                NationalNo = d.Applicant.NationalNo
            };

            return Result<GetApplicantDTO>.Success(driversDTO);
        }

        public async Task<Result<bool>> IsApplicantDriver(int applicantId)
        {
             var result=await uOW.DriverRepository.AnyAsync(a=>a.applicantId == applicantId);
            if (!result)
                return Result<bool>.Failure(["No Driver Found"]);
            return Result<bool>.Success(true);    
                
        }
    }
}
