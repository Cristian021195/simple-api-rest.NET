using CustomersApi.DTOS;
using CustomersApi.Repositories;

namespace CustomersApi.UseCases
{
    public interface IUpdateCustomerUseCase
    {//creamos la interfaz para este uso en concreto, es un contrato que va a tener la clase
        Task<CustomerDTO?> Execute(CustomerDTO customer);
    }
    public class UpdateCustomerUseCase: IUpdateCustomerUseCase
    {
        /* Como llamamos a la BD, la especificamos como en el controlador. Inyectamos la BD, 
          */

        private readonly CustomerDatabaseContext _customerDatabaseContext;

        public UpdateCustomerUseCase(CustomerDatabaseContext customerDatabaseContext)
        {
            _customerDatabaseContext = customerDatabaseContext;
        }
        public async Task<CustomerDTO?> Execute(CustomerDTO customer)
        {
            
            var entity = await _customerDatabaseContext.Get(customer.Id);

            if (entity == null)
                return null;

            entity.FirstName = customer.FirstName;
            entity.LastName = customer.LastName;
            entity.Email = customer.Email;
            entity.Phone = customer.Phone;
            entity.Address = customer.Address;

            await _customerDatabaseContext.Actualizar(entity);
            return entity.ToDto();

        }
    }
}
