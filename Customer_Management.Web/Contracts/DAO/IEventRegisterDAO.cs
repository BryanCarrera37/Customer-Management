namespace Customer_Management.Web.Contracts.DAO
{
    public interface IEventRegisterDAO
    {
        Task RegisterAction(Guid entityId, string action);
    }
}
