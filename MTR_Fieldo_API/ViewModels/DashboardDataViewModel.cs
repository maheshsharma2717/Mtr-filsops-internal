using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.ViewModels
{
    public class DashboardDataViewModel
    {
        public int TotalCustomer { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalPendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OngoingTasks { get; set; }
        public int CanceledTasks { get; set; }
        public List<TaskViewModel> RecentTasks { get; set; }
    }
    public class UserDashboardDataViewModel
    {
        public int PendingTaksCount { get; set; }
        public int InprogressTaskCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public int TotalTasks { get; set; }
        public double AverageRating { get; set; }
        public List<UserReviewViewModel2> Reviews { get; set; }
        public object MyEarning { get; set; }
        public bool? IsOnline { get; set; }

    }
    public class DashboardViewModel
    {
        public int NumberOfServicesRequested { get; set; }
        public double TotalMoneySpent { get; set; }
        public List<UserReviewViewModel2> Reviews { get; set; }
        public List<Fieldo_RequestCategory> Categories { get; set; }
        public double AverageRating { get; set; }
    }

}
