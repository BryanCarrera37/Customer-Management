using Customer_Management.Web.Classes.Exception;
using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.DAO;
using Customer_Management.Web.DTO.Company;
using Customer_Management.Web.Helpers;
using Customer_Management.Web.StaticValues;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Customer_Management.Web.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyDAO _companyDao;
        private readonly ICustomerDAO _customerDao;
        private readonly IEventRegisterDAO _eventRegisterDao;
        private readonly IToastNotification _toastNotification;

        public CompaniesController(IToastNotification toastNotification)
        {
            _companyDao = new CompanyDAO();
            _customerDao = new CustomerDAO();
            _eventRegisterDao = new EventsForCompaniesDAO();
            _toastNotification = toastNotification;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var companies = await _companyDao.GetAll();
            return companies.Count != 0 ? 
                View(companies) :
                Problem("No hay compañías registradas por el momento");
        }

        // GET: Companies/Details/5
        public async Task<IActionResult?> Details(Guid? id)
        {
            if (id == null) { return NotFound(); }

            try
            {
                return View(await GetCompanyWithTheirCustomers(id.Value));
            }
            catch (CustomHttpException ex)
            {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return RedirectToAction(nameof(Index));
            }
            catch { return RedirectToAction(nameof(Index)); }
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] AvailableCompanyDTO company)
        {
            if (!ModelState.IsValid) {
                ToastNotificationHelper.ShowToastWhenValuesFormAreIncorrect(_toastNotification);
                return View();
            }

            try
            {
                var companySearchedByName = await _companyDao.GetByName(company.Name);
                if (companySearchedByName != null)
                {
                    await TryToRegenerateCompany(companySearchedByName.Id);
                    return RedirectToAction(nameof(Index));
                }

                await TryToCreateCompany(company.Name);
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex)
            {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View();
            }
            catch { throw; }
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await GetActionResultWhenLookingForEntity(id);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] AvailableCompanyDTO company)
        {
            if (id != company.Id) { return NotFound(); }
            if(!ModelState.IsValid) {
                ToastNotificationHelper.ShowToastWhenValuesFormAreIncorrect(_toastNotification);
                return View(company);
            }

            try
            {
                await _companyDao.Update(company.Name, id);
                await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Modifying);
                _toastNotification.AddSuccessToastMessage("Compañía modificada satisfactoriamente");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex) {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View();
            }
            catch { throw; }
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await GetActionResultWhenLookingForEntity(id);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _companyDao.Delete(id);
                await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Deleting);
                _toastNotification.AddSuccessToastMessage("La compañía especificada ha sido eliminada satisfactoriamente");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex) {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View();
            }
            catch { throw; }
        }

        private async Task<IActionResult> GetActionResultWhenLookingForEntity(Guid? id)
        {
            if (id == null) { return NotFound(); }

            try
            {
                var company = await _companyDao.Get(id.Value);
                return View(company);
            }
            catch { return NotFound(); }
        }

        private async Task TryToRegenerateCompany(Guid id)
        {
            await _companyDao.Regenerate(id);
            await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Regenerating);
            _toastNotification.AddInfoToastMessage("La compañía ha sido activada o regenerada");
        }

        private async Task TryToCreateCompany(string companyName)
        {
            var possibleCreation = await _companyDao.Save(companyName);
            await _eventRegisterDao.RegisterAction(possibleCreation.Id, ActionForEntityEvent.Creating);
            _toastNotification.AddSuccessToastMessage("Compañía registrada satisfactoriamente");
        }

        private async Task<CompanyWithCustomersDTO> GetCompanyWithTheirCustomers(Guid id)
        {
            try
            {
                var company = await _companyDao.Get(id);
                var customers = await _customerDao.GetCustomersLinkedToCompany(company.Id);
                if (customers == null ||  customers.Count == 0)
                {
                    throw new CustomHttpException(StatusCodes.Status404NotFound, new CustomHttpExceptionBody("Por el momento la compañía NO cuenta con clientes"));
                }

                return new CompanyWithCustomersDTO
                {
                    Id = company.Id,
                    Name = company.Name,
                    CreatedAt = company.CreatedAt,
                    Customers = customers,
                };
            }
            catch { throw; }
        }
    }
}
