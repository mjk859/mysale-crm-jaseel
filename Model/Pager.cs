using System;
namespace MySaleApp.Admin.UI.Model
{
    public class Pager
    {
        //public Pager()
        //{
            
        //}

        public Pager(int recsCount, int pg, int pageSize)
        {
            PageSize = pageSize;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageSize { get; set; }

    }
}
