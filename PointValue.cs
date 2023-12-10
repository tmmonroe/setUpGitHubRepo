namespace TicketingApp.Models
{
    public class PointValue
    {
        public int PointValueId { get; set; }

        public string Name { get; set; } = string.Empty;

        public PointValue()
        {
            PointValueId = 0; 
        }

    }
}
