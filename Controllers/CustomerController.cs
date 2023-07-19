using CustomersApi.DTOS;
using CustomersApi.Repositories;
using CustomersApi.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace CustomersApi.Controllers
{

    [ApiController] // Atributo, son clases que se enlazan con metodos o clases, quiere decir que esta clase ahora cuenta con funcionalidades de api
    // [Authorize] , atributo de autorizacion
    [Route("api/[controller]")] // ruta de este controlador
    public class CustomerController : Controller
    {

        private readonly CustomerDatabaseContext _customerDatabaseContext;
        private readonly IUpdateCustomerUseCase _updateCustomerUseCase;
        public CustomerController(CustomerDatabaseContext customerDatabaseContext, IUpdateCustomerUseCase updateCustomerUseCase) {
            _customerDatabaseContext = customerDatabaseContext;
            _updateCustomerUseCase = updateCustomerUseCase;
        }

        // [Authorize] , podemos aplicar los atributos en metodos, de esta manera definimos rutas privadas o publicas para la API.

        [HttpGet] // api/customer
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        public async Task<IActionResult> GetCustomers()
        {
            List<CustomerDTO> result = _customerDatabaseContext.Customers
                .Select(c => c.ToDto()).ToList();

            return new OkObjectResult(result);
        }

        [HttpGet("{id}")] // api/customer/1
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))] // indicamos que tipo de status code devolver (mas complejo que NodeJS + Express)
        [ProducesResponseType(StatusCodes.Status404NotFound)]//podemos definir aquellos por defecto en caso de error
        public async Task<IActionResult> GetCustomer(long id)
        {
            CustomerEntity result = await _customerDatabaseContext.Get(id);
            if(result == null)
            {
                dynamic res = new ExpandoObject(); // de esta manera customizamos la respuesta de ser necesario
                res.Mensaje = "todo no se encontró";
                res.Error= true;
                return new NotFoundObjectResult(res);
            }

            // dynamic res = new ExpandoObject(); // de esta manera customizamos la respuesta de ser necesario
            // res.Result = 200;
            // res.Mensaje = "todo ok por aqui";
            // res.Error= "false";

            return new OkObjectResult(result.ToDto());

            //throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var result = await _customerDatabaseContext.Delete(id);
            return new OkObjectResult(result);
            //throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerDTO))]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO customer)
        {

            CustomerEntity result = await _customerDatabaseContext.Add(customer);

            return new CreatedResult($"https://localhost:7186/api/customer/{result.Id}", result);

            //throw new NotImplementedException();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(CustomerDTO customer)
        {
            //throw new NotImplementedException();
            CustomerDTO? result = await _updateCustomerUseCase.Execute(customer);
            if(result == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(result);
        }

        // GET: CustomerController/Details/5
        /*
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/


    }
}
