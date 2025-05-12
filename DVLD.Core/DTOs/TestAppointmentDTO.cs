namespace DVLD.Core.DTOs
{
    public class TestAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFee { get; set; }
        public bool IsLooked { get; set; }
        public string TestType { get; set; }
        public string ApplicantName {  get; set; }
        public string ApplicantNationalNo {  get; set; }
    }
}
