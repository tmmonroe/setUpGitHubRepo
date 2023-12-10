using System;
using System.Collections.Generic;

namespace TicketingApp.Models
{
    public class Filter
    {
        public Filter(string filterstring)
        {
            FilterString = filterstring ?? "all-all-all-all";
            string[] filters = FilterString.Split('-');
            TicketId = filters[0];
            StatusId = filters[1];
            int PointValueId;
            if (int.TryParse(filters[2], out int parsedPointValueId))
            {
                PointValueId = parsedPointValueId;
            }
            SprintId = filters[3];

            // Assuming SprintId is an integer in the filterstring
            int sprintId;
            if (int.TryParse(SprintId, out sprintId))
            {
                Sprint = new Sprint(sprintId, DateTime.MinValue, DateTime.MinValue);
            }
        }

        public string FilterString { get; }
        public string TicketId { get; }
        public string StatusId { get; }
        public int PointValueId { get; }
        public string SprintId { get; }
        public Sprint Sprint { get; private set; }

        public bool HasTicket => TicketId.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";
        public bool HasPointValue => PointValueId.ToString().ToLower() != "all";
        public bool HasSprint => SprintId.ToLower() != "all";

        public bool IsPast => Sprint != null && Sprint.EndDate < DateTime.Now;
        public bool IsFuture => Sprint != null && Sprint.StartDate > DateTime.Now;
        public bool IsToday => Sprint != null && Sprint.StartDate <= DateTime.Now && Sprint.EndDate >= DateTime.Now;
    }
}