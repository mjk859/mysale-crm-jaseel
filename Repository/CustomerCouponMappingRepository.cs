using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using static MySaleApp.Admin.UI.Interface.IRepositories;

namespace MySaleApp.Admin.UI.Repository
{
    public class CustomerCouponMappingRepository : EntityBaseRepository<CustomerCouponMapping>, ICustomerCouponMappingRepository
    {
        public CustomerCouponMappingRepository(MySaleAppContext context)
            : base(context)
        { }
    }
}