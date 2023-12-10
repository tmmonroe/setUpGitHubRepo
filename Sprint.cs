namespace TicketingApp.Models
{
    public class Sprint
    {
        public int SprintNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Sprint(int sprintNumber, DateTime startDate, DateTime endDate)
        {
            SprintNumber = sprintNumber;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
