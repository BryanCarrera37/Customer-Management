using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.Models.Entities;

namespace Customer_Management.Web.DAO
{
    public class EventsForCustomersDAO : BaseDAO, IEventRegisterDAO
    {
        public EventsForCustomersDAO()
        {
            InitializeDbContext();
            InitializeLogger();
        }

        public async Task RegisterAction(Guid entityId, string action)
        {
            try
            {
                var entityEvent = GetEventToRegisterIt(entityId, action);
                dbContext!.EventsForCustomers.Add(entityEvent);
                await SaveChangesOrThrowExceptionIsNecessary();
            }
            catch {
                logger!.LogError($"Could not register the action '{action}' for the customer with the ID {entityId}");
                throw;
            }
        }

        protected override void InitializeLogger()
        {
            logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger<EventsForCustomersDAO>();
        }

        private static EventsForCustomers GetEventToRegisterIt(Guid customerId, string action) => new EventsForCustomers
        {
            CustomerId = customerId,
            Action = action
        };
    }
}
