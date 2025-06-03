using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using static MySaleApp.Admin.UI.Interface.IRepositories;

namespace MySaleApp.Admin.UI.Repository
{
    public class CouponRepository : EntityBaseRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(MySaleAppContext context)
            : base(context)
        { }
    }
}