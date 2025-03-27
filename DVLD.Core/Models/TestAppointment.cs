namespace DVLD.Core.Models
{
    public class TestAppointment
    {
        public int Id {  get; set; }
        public DateTime AppointmentDate {  get; set; }
        public decimal PaidFee {  get; set; }
        public bool IsLooked {  get; set; }
        public int ApplicationId {  get; set; }
        public int? RetakeTestAppId {  get; set; }
        public int TestTypeId {  get; set; }

        public Application Application { get; set; }
        public TestType TestType { get; set; }
        public Test Test { get; set; }
    }
}
