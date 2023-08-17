using Customer_Management.Web.Classes.Exception;
using Customer_Management.Web.Contracts.DAO;
using Customer_Management.Web.DAO;
using Customer_Management.Web.DTO.Customer;
using Customer_Management.Web.Helpers;
using Customer_Management.Web.Models.Entities;
using Customer_Management.Web.StaticValues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace Customer_Management.Web.Controllers
{

    public class CustomersController : Controller
    {
        private readonly ICustomerDAO _customerDao;
        private readonly ICompanyDAO _companyDao;
        private readonly IEventRegisterDAO _eventRegisterDao;

        private readonly IToastNotification _toastNotification;

        public CustomersController(IToastNotification toastNotification)
        {
            _customerDao = new CustomerDAO();
            _companyDao = new CompanyDAO();
            _eventRegisterDao = new EventsForCustomersDAO();
            _toastNotification = toastNotification;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _customerDao.GetAll());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            return await GetTheProperlyActionResultWhenLookingForTheEntity(id);
        }

        // GET: Customers/Create
        public async Task<IActionResult> Create()
        {
            await SetCompaniesToTheViewBag();
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,CompanyId")] CustomerDTO customer)
        {
            if (!ModelState.IsValid)
            {
                await SetCompaniesToTheViewBag();
                ToastNotificationHelper.ShowToastWhenValuesFormAreIncorrect(_toastNotification);
                return View(customer);
            }

            try
            {
                var company = await _companyDao.Get(customer.CompanyId);
                customer.CompanyName = company.Name;
                var possibleCustomer = await _customerDao.GetByEmail(customer.Email);
                if (possibleCustomer != null)
                {
                    await TryToRegenerateCustomer(customer, possibleCustomer.Id);
                    return RedirectToAction(nameof(Index));
                }

                await TryToCreateCustomer(customer);
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex)
            {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View(customer);
            }
            catch { throw; }
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            return await GetTheProperlyActionResultWhenLookingForTheEntity(id);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,Email,CompanyId")] CustomerModificationDTO modificationProps)
        {
            if (!ModelState.IsValid)
            {
                await SetCompaniesToTheViewBag();
                ToastNotificationHelper.ShowToastWhenValuesFormAreIncorrect(_toastNotification);
                return View(await _customerDao.Get(id));
            }

            try
            {
                await TryToModifyTheCustomer(modificationProps, id);
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex)
            {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View(await _customerDao.Get(id));
            }
            catch { throw; };
        }

            // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            return await GetTheProperlyActionResultWhenLookingForTheEntity(id);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _customerDao.Delete(id);
                await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Deleting);
                _toastNotification.AddSuccessToastMessage("El cliente especificado ha sido eliminado satisfactoriamente");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomHttpException ex)
            {
                ToastNotificationHelper.HandleExceptionAndShowToast(_toastNotification, ex);
                return View();
            }
            catch { throw; }
        }

        private async Task SetCompaniesToTheViewBag()
        {
            ViewData[DefinedValuesForCompaniesSelectList.keyForCompaniesSelectList] = await GetSelectListForTheCompanies();
        }

        private async Task<SelectList> GetSelectListForTheCompanies()
        {
            return new SelectList(await _companyDao.GetAll(), DefinedValuesForCompaniesSelectList.dataValueFieldName, DefinedValuesForCompaniesSelectList.dataTextFieldName);
        }

        private async Task<IActionResult> GetTheProperlyActionResultWhenLookingForTheEntity(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerDao.Get(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            await SetCompaniesToTheViewBag();
            return View(customer);
        }

        private async Task TryToRegenerateCustomer(CustomerDTO customer, Guid id)
        {
            await _customerDao.Regenerate(GetRegenerationDtoFromNormalDto(customer), id);
            await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Regenerating);
            _toastNotification.AddInfoToastMessage("El cliente ha sido activado o regenerado");
        }

        private async Task TryToCreateCustomer(CustomerDTO customer)
        {
            var result = await _customerDao.Save(customer);
            await _eventRegisterDao.RegisterAction(result.Id, ActionForEntityEvent.Creating);
            _toastNotification.AddSuccessToastMessage("Cliente registrado satisfactoriamente");
        }

        private async Task TryToModifyTheCustomer(CustomerModificationDTO modificationProps, Guid id)
        {
            var company = await _companyDao.Get(modificationProps.CompanyId);
            modificationProps.CompanyName = company.Name;
            await _customerDao.Update(modificationProps, id);
            await _eventRegisterDao.RegisterAction(id, ActionForEntityEvent.Modifying);
            _toastNotification.AddSuccessToastMessage("Cliente modificado satisfactoriamente");
        }

        private static CustomerRegenerationDTO GetRegenerationDtoFromNormalDto(CustomerDTO customer) => new CustomerRegenerationDTO
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            CompanyId = customer.CompanyId,
            CompanyName = customer.CompanyName
        };
    }
}
