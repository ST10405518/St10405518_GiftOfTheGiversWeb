using St10405518_GiftOfTheGiversWeb.Models;

namespace St10405518_GiftOfTheGiversWeb.Models.ViewModels
{
    public class VolunteerDashboardViewModel
    {
        public int TotalVolunteers { get; set; }
        public int ActiveTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<Volunteer> RecentRegistrations { get; set; } = new List<Volunteer>();
    }

    public class AssignVolunteerViewModel
    {
        public VolunteerTask? Task { get; set; }
        public List<Volunteer> AvailableVolunteers { get; set; } = new List<Volunteer>();
    }
}