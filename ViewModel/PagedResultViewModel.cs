using MySaleApp.Admin.UI.Models;

namespace MySaleApp.Admin.UI.ViewModel
{
    public class PagedResultViewModel
    {
        public List<CustomerPlanViewModel> Customers { get; set; }
        public List<CustomerUserViewModel> Users { get; set; }
        public List<Plan> Plans { get; set; }
        public List<SystemUserRoleViewModel> SystemUsers { get; set; }
        public List<PlanDetailsViewModel> PlanDetails { get; set; }
        public List<SystemUserRole> UserRoles { get; set; }
        public List<Dealer> Dealers { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }

}
