using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using FluentValidation;

namespace DVLD.Core.Services.Implementations
{
    public class ApplicantService : IApplicantService
    {
        private readonly IUOW uOW;
        private readonly IFileService fileService;
        private readonly IValidator<ApplicantDTO>  ApplicantDTOValidator;

        public ApplicantService(IUOW uOW, IFileService fileService, IValidator<ApplicantDTO> applicantDTOValidator)
        {
            this.uOW = uOW;
            this.fileService = fileService;
            this.ApplicantDTOValidator=applicantDTOValidator;
        }
        public async Task<Result<int>> AddApplicant(ApplicantDTO addedApplicant)
        {
            var validationResult = ApplicantDTOValidator.Validate(addedApplicant);
            if (!validationResult.IsValid)
            {
                return Result<int>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }


            var applicant = new Applicant()
            {
                NationalNo = addedApplicant.NationalNo,
                Fname = addedApplicant.Fname,
                Sname = addedApplicant.Sname,
                Tname = addedApplicant.Tname,
                Lname = addedApplicant.Lname,
                CountryId = addedApplicant.CountryId,
                Gender = addedApplicant.Gender,
                BirthDate = addedApplicant.BirthDate,
                Address = addedApplicant.Address
            };
            if (addedApplicant.Image != null)
            {
                var imagepath = await fileService.UploadFileAsync(addedApplicant.Image, FileSettings.ImagePath);
                applicant.ImagePath = imagepath;
            }
            await uOW.ApplicantRepository.AddAsync(applicant);
            uOW.Complete();
            return Result<int>.Success(applicant.ApplicantId);
        }

        public async Task<Result> DeleteApplicantByIdAsync(int id)
        {
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.ApplicantId == id);
            if (applicant == null)
                return Result.Failure(["No Applicant Found!"]);

            var path = applicant.ImagePath;

            // Delete from DB
            uOW.ApplicantRepository.Delete(applicant);
            var result = uOW.Complete();

            if (result > 0)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    await fileService.DeleteFileAsync(path);
                }
                return Result.Success();
            }

            return Result.Failure(["Failed to delete the applicant."]);
        }

        public async Task<Result> DeleteApplicantByNationalNoAsync(string nationalNo)
        {
            if (await isNationalNoTakenAsync(nationalNo))
            {
                return Result.Failure(["National Number already exists!"]);
            }
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.NationalNo ==nationalNo);
            if (applicant == null)
                return Result.Failure(["No Applicant Found!"]);

            var path = applicant.ImagePath;

            // Delete from DB
            uOW.ApplicantRepository.Delete(applicant);
            var result = uOW.Complete();

            if (result > 0)
            { 
                if (!string.IsNullOrEmpty(path))
                {
                    await fileService.DeleteFileAsync(path);
                }
               return Result.Success();
            }

            return Result.Failure(["Failed to delete the applicant."]);
        }

        public async Task<Result<List<Applicant>>> GetApplicantsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                return Result<List<Applicant>>.Failure(["Page Number must be greater than 0."]);

            if (pageSize <= 0)
                return Result<List<Applicant>>.Failure(["Page Size must be greater than 0."]);

            var applicants = (await uOW.ApplicantRepository.PaginateAsync(pageSize, (pageNumber - 1) * pageSize)).ToList();

            if (!applicants.Any())
                return Result<List<Applicant>>.Failure(["No Applicants Found."]);

            return Result<List<Applicant>>.Success(applicants);
        }

        public async Task<Result<string>> GetApplicantUserIdbyIdAsync(int id)
        {
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.ApplicantId == id);
            if (applicant == null)
            {
                return Result<string>.Failure(new List<string> { "No Applicant with this id" });
            }
            return Result<string>.Success(applicant.UserId);
        }

        public async Task<Result<string>> GetApplicantUserIdbyNationalNoAsync(string nationalNo)
        {
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.NationalNo == nationalNo);
            if (applicant == null)
            {
                return Result<string>.Failure(new List<string> { "No Applicant with this id" });
            }
            return Result<string>.Success(applicant.UserId);
        }

        public async Task<Result<Applicant>> GetByIdAsync(int id)
        {
            var applicant = await uOW.ApplicantRepository.GetByIdAsync(id);
            if (applicant == null)
            {
                return Result<Applicant>.Failure(new List<string> { "There is no applicant with this id" });
            }
            return Result<Applicant>.Success(applicant);
        }

        public async Task<Result<Applicant>> GetByNationalNoAsync(string nationalNo)
        {
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.NationalNo == nationalNo);
            if (applicant == null)
            {
                return Result<Applicant>.Failure(new List<string> { "there is no applicant with this nationalNo" });
            }
            return Result<Applicant>.Success(applicant);
        }

        public async Task<Result> IsNationalNoTakenAsync(string nationalNo)
        {
            var result=await isNationalNoTakenAsync(nationalNo);
            if (result)
                return Result.Failure(["This National Number already taken!"]);
            return Result.Success();
        }
        private async Task<bool> isNationalNoTakenAsync(string nationalNo)
        {
            if (await uOW.ApplicantRepository.FindAsync(x => x.NationalNo == nationalNo) == null)
            {
                return false;
            }
            return true ;
        }

        public async Task<Result> UpdateApplicantAsync(int id, ApplicantDTO updatedApplicant)
        {
            var applicant = await uOW.ApplicantRepository.FindAsync(x => x.ApplicantId == id);
            if (applicant == null)
            {
                return Result.Failure(new List<string> { "there is no applicant with this id" });
            }
            applicant.NationalNo = updatedApplicant.NationalNo;
            applicant.Fname = updatedApplicant.Fname;
            applicant.Sname = updatedApplicant.Sname;
            applicant.Tname = updatedApplicant.Tname;
            applicant.Lname = updatedApplicant.Lname;
            applicant.CountryId = updatedApplicant.CountryId;
            applicant.Gender = updatedApplicant.Gender;
            applicant.BirthDate = updatedApplicant.BirthDate;
            applicant.Address = updatedApplicant.Address;

            if (updatedApplicant.Image != null) // If a new image is uploaded
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(applicant.ImagePath))
                {
                    await fileService.DeleteFileAsync(applicant.ImagePath);
                }

                // Upload new image
                var newImagePath = await fileService.UploadFileAsync(updatedApplicant.Image,FileSettings.ImagePath);
                applicant.ImagePath = newImagePath;
            }

            uOW.ApplicantRepository.Update(applicant);
            uOW.Complete();
            return Result.Success();
        }

        public async Task<Result<string>> GetFullNameAsync(int id)
        {
            var applicant = await uOW.ApplicantRepository.GetByIdAsync(id);
            if (applicant == null)
            {
                return Result<string>.Failure(new List<string> { "There is no applicant with this id" });
            }
            var fullName=applicant.Fname +" "+ applicant.Sname+" "+applicant.Tname+" "+applicant.Lname;
            return Result<string>.Success(fullName);
        }
    }
}
