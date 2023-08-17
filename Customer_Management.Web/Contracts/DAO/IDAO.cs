
namespace Customer_Management.Web.Contracts;

public interface IDAO<Result, Creation, Modification> where Result : class where Creation : class where Modification : class
{
    Task<List<Result>> GetAll();
    Task<Result> Get(Guid id);
    Task<Result> Save(Creation creationValue);
    Task<Result> Update(Modification modificationValue, Guid id);
    Task Delete(Guid id);
}
