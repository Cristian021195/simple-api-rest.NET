using CustomersApi.DTOS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomersApi.Repositories
{
    // Es importante crear esta nueva capa/contexto el cual sera responsable de conectarse a la base de datos, 
    //DBContext viene de EntityFramework , 
    public class CustomerDatabaseContext: DbContext // extiende nuestra clase de DBcontext
    {
        public CustomerDatabaseContext(DbContextOptions<CustomerDatabaseContext> options) : base(options)
        {//Especificamos la configuracion definida en nuestro program, (contenedor de dependencias)  

        }
        
        /* necesitamos crear una entidad que hace un Mapeo 1 a 1 a nuestro objeto en la base de datos, de casualidad es igual a nuestro DTO
        Pero no siempre es lo mismo, aveces nuestras consultas tienen JOINS, y por ende debemos moldear un nuevo objeto/entidad que se ajuste a dichos campos
         */
        public DbSet<CustomerEntity> Customers { get; set; } // es lo que bindea / hace referencia a nuestras tablas en la base de datos
        /* Es recomendable que accedamos a la bbdd desde esta capa de acceso a datos, si necesitamos traer info hacemos:  */
        public async Task<CustomerEntity?> Get(long id)
        {
            try
            {
                return await Customers.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                return null;//throw new Exception("Sin coincidencias");
            }
            
        }

        public async Task<bool> Actualizar(CustomerEntity customerEntity)
        {
            try
            {
                Customers.Update(customerEntity);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception("Sin coincidencias");
            }

        }

        public async Task<bool> Delete(long id)
        {
            CustomerEntity entity = await Get(id);
            Customers.Remove(entity);
            SaveChanges();
            return true;
        }

        public async Task<CustomerEntity> Add(CreateCustomerDTO customer)
        {
            CustomerEntity entity = new CustomerEntity()
            {//esto seria el mapeo de una CreateCustomerDTO a CustomerEntity - solo consiste en tomar datos y ponerlos en la entidad
                Id = null,
                Address = customer.Address,
                Email = customer.Email,
                FirstName = customer.FirstName,
                Phone = customer.Phone,
                LastName = customer.LastName,
            };

            EntityEntry<CustomerEntity> response = await Customers.AddAsync(entity);
            await SaveChangesAsync();
            //return Get(respoonse.Entity.Id ?? throw new Exception("Error al guardar"));
            return await Get(response.Entity.Id ?? throw new Exception("no se ha podido guardar"));
        }
        /* ahora debemos añadir nuestra bbdd a nuestro contenedor de dependencias, con inyeccion de dependencias, (dentro de SOLID)
         definimos una clase  con su INTERFAZ y luego inyectarla o aplicarla en un servicio,clase,etc */
        /*public CustomerEntity GetSincrono(long id) // metodo sincrono, ya no se usa tanto, pero depende del caso de uso
        {
            return Customer.First(x => x.Id == id);
        }*/
    }

    public class CustomerEntity // no siempre es 1 a 1 con la API    
    { // por ejemplo si decidimos devolver en el endpoint apellido y nombre junto, lo modificamos aqui, por ello no usamos el objeto DTO u otra clase de una carpeta /Classes
        
        public long? Id { get; set; } // ? indica posiblemente nulo
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public CustomerDTO ToDto() // hacemos nuestro mapper, puede ser aqui o en una clase estatica
        {
            return new CustomerDTO()
            {
                Address = Address,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Phone  = Phone,
                Id = Id ?? throw new Exception("SIN ID")
            };
        }

    }
}
