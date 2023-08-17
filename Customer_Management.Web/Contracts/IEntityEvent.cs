namespace Customer_Management.Web.Contracts
{
    public interface IEntityEvent
    {
        public Guid Id { get; set; }
        public DateTime ActionDate { get; set; }
        public string Action { get; set; }
    }
}
