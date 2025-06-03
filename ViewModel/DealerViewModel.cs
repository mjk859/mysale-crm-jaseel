namespace MySaleApp.Admin.UI.ViewModel
{
    public class DealerViewModel
    {
        public string Name { get; set; }
        public string DealerCode { get; set; }
        public string ReferralCode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
