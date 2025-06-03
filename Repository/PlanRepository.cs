using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using static MySaleApp.Admin.UI.Interface.IRepositories;

namespace MySaleApp.Admin.UI.Repository
{
    public class PlanRepository : EntityBaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(MySaleAppContext context)
            : base(context)
        { }
    }
}