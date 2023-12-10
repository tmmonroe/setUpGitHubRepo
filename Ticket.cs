namespace TicketingApp.Models
{
    public class Ticket
    {
        public string TicketId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public PointValue PointValue { get; set; }
        public Sprint Sprint { get; set; }
        public Status Status { get; set; }

        // Foreign key properties
        public int PointValueId { get; set; }
        public int SprintId { get; set; }
        public string StatusId { get; set; }

        // Add DueDate property
        public DateTime? DueDate { get; set; }

    }
}
