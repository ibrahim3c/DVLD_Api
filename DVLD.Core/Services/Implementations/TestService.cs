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
        private readonly IApplicationService applicationService;

        public TestService(IUOW uow,IApplicationService applicationService)
        {
            this.uow = uow;
            this.applicationService = applicationService;
        }

        #region TestType
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


        #endregion


        #region Test And TestAppointment
        public async Task<Result<int>> ScheduleVisionTestAsync(int appId, int applicantId)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
            && a.AppStatus==AppStatuses.Completed))
                return Result<int>.Failure(["this Application is Completed"]);

            // yaraaab
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
            && a.ApplicantId == applicantId
            && a.AppStatus == AppStatuses.Approved))
                return Result<int>.Failure(["No Approved Application for this Applicant Found!"]);

            // Check if the applicant already has a scheduled test of the same type:
            // - If an appointment exists but the test has not been taken → "You already have an active test appointment."
            // - If the test was taken but failed → Schedule a retake test.
            // - If the test was taken and passed → "You have already passed this test."

            var retakeAppId = 0;
            var appointment = await uow.TestAppointmentRepository.FindAsync(t => t.ApplicationId == appId
                                                            && t.TestTypeId == (int)TestTypes.VisionTest, ["Test"]);

            if (appointment != null && appointment.Test == null)
            {
                //  If test is scheduled but not taken yet
                return Result<int>.Failure(["You already have an active test appointment for this application!"]);

            }
            //  If test was passed, no need to retake
            else if (appointment != null && appointment.Test.TestResult)
                return Result<int>.Failure(["You already passed this test."]);
            else if(appointment != null &&!appointment.Test.TestResult)
            {
                var result = await applicationService.ApplyForRetakeTestApp(applicantId);
                if (result.IsSuccess)
                    retakeAppId = result.Value;
                else
                    return Result<int>.Failure(result.Errors);
            }



                //add new appointment
                TestAppointment testAppointment = new()
                {
                    ApplicationId = appId,
                    IsLooked = false,
                    // i make the default is week after he Schedule
                    AppointmentDate = DateTime.UtcNow.AddDays(7),
                    PaidFee = (await uow.testTypeRepository.GetByIdAsync(1)).TypeFee,
                    TestTypeId = (int)TestTypes.VisionTest,
                    RetakeTestAppId = retakeAppId
                };

                await uow.TestAppointmentRepository.AddAsync(testAppointment);
                uow.Complete();
                return Result<int>.Success(testAppointment.Id);


        }

        public async Task<Result<int>> ScheduleWrittenTestAsync(int appId, int applicantId)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                && a.AppStatus == AppStatuses.Completed))
                return Result<int>.Failure(["this Application is Completed"]);

            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                                                        && a.ApplicantId == applicantId
                                                        && a.AppStatus == AppStatuses.Approved))
                return Result<int>.Failure(["No Approved Application for this Applicant Found!"]);

            // Check if the applicant already has a scheduled test of the same type:
            // - If an appointment exists but the test has not been taken → "You already have an active test appointment."
            // - If the test was taken but failed → Schedule a retake test.
            // - If the test was taken and passed → "You have already passed this test."

            var appointment = await uow.TestAppointmentRepository.FindAsync(t => t.ApplicationId == appId
                                                            && t.TestTypeId == (int)TestTypes.WrittenTest, ["Test"]);

            if (appointment != null && appointment.Test == null)
                //  If test is scheduled but not taken yet
                return Result<int>.Failure(["You already have an active test appointment for this application!"]);

            //  If test was passed, no need to retake
            else if (appointment != null && appointment.Test.TestResult)
                return Result<int>.Failure(["You already passed this test."]);


            //check if he passed the visionTest
            var hasPassedVissionTest = await uow.TestAppointmentRepository.FindAsync(x => x.ApplicationId == appId
            && x.TestTypeId == (int)TestTypes.VisionTest && x.IsLooked && x.Test != null && x.Test.TestResult, ["Test"]);
            if (hasPassedVissionTest == null)
                return Result<int>.Failure(["You must pass the Vision Test before scheduling the Written Test!"]);

            int retakeAppId = 0;
            var testType = await uow.testTypeRepository.GetByIdAsync((int)TestTypes.WrittenTest);
            if (testType == null)
                return Result<int>.Failure(["Test type not found!"]);


            else if (appointment != null && !appointment.Test.TestResult)
            {
                var result = await applicationService.ApplyForRetakeTestApp(applicantId);
                if (result.IsSuccess)
                    retakeAppId = result.Value;
                else
                    return Result<int>.Failure(result.Errors);
            }



            //add new appointment
            TestAppointment testAppointment = new()
            {
                ApplicationId = appId,
                IsLooked = false,
                // i make the default is week after he Schedule
                AppointmentDate = DateTime.UtcNow.AddDays(7),
                PaidFee = testType.TypeFee,
                TestTypeId = (int)TestTypes.WrittenTest,
                RetakeTestAppId = retakeAppId
            };

            await uow.TestAppointmentRepository.AddAsync(testAppointment);
            uow.Complete();
            return Result<int>.Success(testAppointment.Id);
        }

        public async Task<Result<int>> SchedulePracticalTestAsync(int appId, int applicantId)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                 && a.AppStatus == AppStatuses.Completed))
                return Result<int>.Failure(["this Application is Completed"]);

            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                                                        && a.ApplicantId == applicantId
                                                        && a.AppStatus == AppStatuses.Approved))
                return Result<int>.Failure(["No Approved Application for this Applicant Found!"]);

            // Check if the applicant already has a scheduled test of the same type:
            // - If an appointment exists but the test has not been taken → "You already have an active test appointment."
            // - If the test was taken but failed → Schedule a retake test.
            // - If the test was taken and passed → "You have already passed this test."

            var appointment = await uow.TestAppointmentRepository.FindAsync(t => t.ApplicationId == appId
                                                            && t.TestTypeId == (int)TestTypes.PracticalTest, ["Test"]);
            if (appointment != null && appointment.Test == null)
                //  If test is scheduled but not taken yet
                return Result<int>.Failure(["You already have an active test appointment for this application!"]);

            //  If test was passed, no need to retake
            else if (appointment != null && appointment.Test.TestResult)
                return Result<int>.Failure(["You already passed this test."]);

            //check if he passed the visionTest
            var hasPassedVissionTest = await uow.TestAppointmentRepository.FindAsync(x => x.ApplicationId == appId
            && x.TestTypeId == (int)TestTypes.VisionTest && x.IsLooked && x.Test != null && x.Test.TestResult, ["Test"]);
            if (hasPassedVissionTest == null)
                return Result<int>.Failure(["You must pass the Vision Test before scheduling the Practical Test!"]);

            //check if he passed the writtenTest
            var hasPassedWrittenTest = await uow.TestAppointmentRepository.FindAsync(x => x.ApplicationId == appId
            && x.TestTypeId == (int)TestTypes.WrittenTest && x.IsLooked && x.Test != null && x.Test.TestResult, ["Test"]);
            if (hasPassedWrittenTest == null)
                return Result<int>.Failure(["You must pass the Written Test before scheduling the Practical Test!"]);

            var retakeAppId = 0;
            var testType = await uow.testTypeRepository.GetByIdAsync((int)TestTypes.PracticalTest);
            if (testType == null)
                return Result<int>.Failure(["Test type not found!"]);


            else if (appointment != null && !appointment.Test.TestResult)
            {
                var result = await applicationService.ApplyForRetakeTestApp(applicantId);
                if (result.IsSuccess)
                    retakeAppId = result.Value;
                else
                    return Result<int>.Failure(result.Errors);
            }

            //add new appointment
            TestAppointment testAppointment = new()
            {
                ApplicationId = appId,
                IsLooked = false,
                // i make the default is week after he Schedule
                AppointmentDate = DateTime.Now.AddDays(7),
                PaidFee = testType.TypeFee,
                TestTypeId = (int)TestTypes.PracticalTest,
                RetakeTestAppId = retakeAppId,
            };

            await uow.TestAppointmentRepository.AddAsync(testAppointment);
            uow.Complete();
            return Result<int>.Success(testAppointment.Id);
        }

        public async Task<Result> EditTestAppointmentAsync(int testAppointmentId, EditTestAppointmentDTO editTestAppointmentDTO)
        {
            var testAppointment = await uow.TestAppointmentRepository.FindAsync(t => t.Id == testAppointmentId, ["Test"]);
            if (testAppointment is null)
                return Result.Failure(["no Test Appointment Found"]);
            if (testAppointment.IsLooked)
                return Result.Failure(["This Appoinment is already finished"]);

            testAppointment.Test.TestResult = editTestAppointmentDTO.TestResult;
            testAppointment.Test.Notes = editTestAppointmentDTO.Notes;
            testAppointment.AppointmentDate = editTestAppointmentDTO.AppointmentDate;

            uow.TestAppointmentRepository.Update(testAppointment);
            uow.Complete();
            return Result.Success();
        }

        public async Task<Result> CompleteTestAsync(CompleteTestDTO completeTestDTO)
        {
            var testAppointment = await uow.TestAppointmentRepository.FindAsync(a => a.Id == completeTestDTO.TestAppointmentId);
            if (testAppointment is null)
                return Result.Failure(["No Test Appointment Found"]);
            if (testAppointment.IsLooked)
                return Result.Failure(["u already completed this Test"]);

            testAppointment.IsLooked = true;
            Test test = new Test
            {
                TestAppointmentId = completeTestDTO.TestAppointmentId,
                TestResult = completeTestDTO.TestResult,
                Notes = completeTestDTO.Notes
            };

            await uow.TestRepository.AddAsync(test);
            uow.Complete();
            return Result.Success();
        }
        public async Task<Result> IsAllTestPassed(int applicantId, int appId)
        {
            var testTypes = new[]
            {
            TestTypes.VisionTest,
            TestTypes.WrittenTest,
            TestTypes.PracticalTest
            };

            foreach (var testType in testTypes)
            {
                var result = await IsTestPassed(applicantId, appId, testType);
                if (!result.IsSuccess)
                    return result; // Return failure immediately if any test is not passed
            }

            return Result.Success();
        }
        public async Task<Result> IsTestPassed(int applicantId, int appId, TestTypes testType)
        {
            if (!await uow.ApplicationRepository.AnyAsync(a => a.AppID == appId
                        && a.ApplicantId == applicantId
                        && a.AppStatus == AppStatuses.Pending))
                return Result.Failure(["No Application for this Applicant Found!"]);

            var appointment = await uow.TestAppointmentRepository.FindAsync(t => t.ApplicationId == appId
                                                            && t.TestTypeId == (int)testType
                                                            && t.IsLooked
                                                            && t.Test != null
                                                            && t.Test.TestResult, ["Test"]);

            if (appointment == null)
                return Result.Failure([$"You didn't pass the {testType}"]);

            return Result.Success();
        }


        #endregion

    }
}
