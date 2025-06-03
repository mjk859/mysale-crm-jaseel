using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using static MySaleApp.Admin.UI.Interface.IRepositories;

namespace MySaleApp.Admin.UI.Repository
{
    public class PlanDetailsRepository : EntityBaseRepository<PlanDetails>, IPlanDetailsRepository
    {
        public PlanDetailsRepository(MySaleAppContext context)
            : base(context)
        { }
    }
}