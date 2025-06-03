using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Models;
using static MySaleApp.Admin.UI.Interface.IRepositories;

namespace MySaleApp.Admin.UI.Repository
{
    public class PaymentRepository : EntityBaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MySaleAppContext context)
            : base(context)
        { }
    }
}