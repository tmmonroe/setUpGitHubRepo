using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TicketingApp.Models;

namespace TicketingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TicketDb _context;
        public HomeController(TicketDb context) 
        {
            _context = context;
        }
        

        // A static list to store tickets (this is for demonstration purposes, and you might want to use a database in a real application)
        private static List<Ticket> tickets = new List<Ticket>();

        // GET: /Ticket/Index
        public IActionResult Index(string id)
        {
            var filter = new Filter(id);
            ViewBag.Filter = filter;
            ViewBag.Tickets = context.Tickets.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.PointValues = context.PointValues.ToList();
            ViewBag.Sprints = context.Sprints.ToList();

            IQueryable<Ticket> query = context.Tickets
                .Include(t => t.Status)
                .Include(t => t.PointValue)
                .Include(t => t.Sprint);

            if (filter.HasTicket)
            {
                query = query.Where(t => t.TicketId == filter.TicketId);
            }
            if (filter.HasStatus)
            {
                query = query.Where(t => t.StatusId == filter.StatusId);
            }
            if (filter.HasPointValue)
            {
                query = query.Where(t => t.PointValueId == filter.PointValueId);
            }
            if (filter.HasSprint)
            {
                if (filter.IsPast)
                    query = query.Where(t => t.Sprint.EndDate < DateTime.Now);
                else if (filter.IsFuture)
                    query = query.Where(t => t.Sprint.StartDate > DateTime.Now);
                else if (filter.IsToday)
                    query = query.Where(t => t.Sprint.StartDate <= DateTime.Now && t.Sprint.EndDate >= DateTime.Now);
            }

            var tasks = query.OrderBy(t => t.DueDate).ToList();

            // Pass the list of tickets to the view
            return View(tasks);
        }

        // GET: /Ticket/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Tickets = context.Tickets.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.PointValues = context.PointValues.ToList();
            ViewBag.Sprints = context.Sprints.ToList();
            var task = new Ticket { StatusId = "todo" };
            return View(task);
        }

        // POST: /Ticket/Create
        [HttpPost]
        public IActionResult Create(Ticket task)
        {
            if (ModelState.IsValid)
            {
                context.Tickets.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Tickets = context.Tickets.ToList();
                ViewBag.Statuses = context.Statuses.ToList();
                ViewBag.PointValues = context.PointValues.ToList();
                ViewBag.Sprints = context.Sprints.ToList();
                return View(task);
            }
        }
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, Ticket selected)
        {
            selected = context.Tickets.Find(selected.TicketId)!;
            if (selected != null)
            {
                selected.StatusId = "done";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult DeleteComplete(string id)
        {
            var toDelete = context.Tickets
                .Where(t => t.StatusId == "done").ToList();

            foreach (var task in toDelete)
            {
                context.Tickets.Remove(task);
            }
            context.SaveChanges();
            return RedirectToAction("Index", new { ID = id });
        }

    }
}
