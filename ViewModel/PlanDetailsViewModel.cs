using System;

namespace MySaleApp.Admin.UI.ViewModel
{
    public class PlanDetailsViewModel
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public string Description { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderLine { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string PlanName { get; set; }

    }
}
