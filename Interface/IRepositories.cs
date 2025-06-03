using MySaleApp.Admin.UI.Models;

namespace MySaleApp.Admin.UI.Interface
{
    public class IRepositories
    {
        public interface ICustomerRepository : IEntityBaseRepository<Customer>
        { }

        public interface IUserRepository : IEntityBaseRepository<User>
        { }

        public interface IPlanRepository : IEntityBaseRepository<Plan>
        { }

        public interface IPaymentRepository : IEntityBaseRepository<Payment>
        { }

        public interface IPlanDetailsRepository : IEntityBaseRepository<PlanDetails>
        { }

        public interface ICouponRepository : IEntityBaseRepository<Coupon>
        { }

        public interface ICustomerCouponMappingRepository : IEntityBaseRepository<CustomerCouponMapping>
        { }
    }
}