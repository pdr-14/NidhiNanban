namespace Nidhinanban.Models;

public class ViewCustomer
{
        public string? customerid { get; set; }
        public string? customername { get; set; }
        public string? cutomerphonenumber { get; set; }
        public string? customeraddress { get; set; }
}

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}
