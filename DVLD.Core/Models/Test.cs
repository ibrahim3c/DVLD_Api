namespace DVLD.Core.Models
{
    public class Test
    {
        public int Id {  get; set; }
        public int? TestAppointmentId {  get; set; }
        public bool TestResult {  get; set; }
        public string Notes {  get; set; }
        public TestAppointment? TestAppointment { get; set; }

    }
}
