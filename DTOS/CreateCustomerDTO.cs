using System.ComponentModel.DataAnnotations;

namespace CustomersApi.DTOS
{
    public class CreateCustomerDTO
    {
        //private long _id; es lo mismo que abajo

        // modificadores de acceso, public, private, static, internal, protected, etc. accesso desde dentro o fuera de la clase
        // .NET nos da Model Binding en los objetos que llegan a nuestra API, permitiendonos agregar logica en el objeto, por ej campos obligatorios
        // Es similar a express-validator de NodeJS, trabaja como un middleware, ya que la respuesta del endpoint no se dará y la reemplaza la excepción con ErrorMessage
        //[MaxLength(100)], y otras validaciones que se nos puedan ocurrir
        [Required (ErrorMessage ="El nombre propio debe especificarse")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "El apellido debe especificarse")]
        public string LastName { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "el email no es correcto")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        /*Todo esto es nuestro DTO: es un objeto que se usa para transferir informacion/paquete de dato de una capa de un dominio a otra capa diferente,
         * puede que llegue desde dentro de la api, o que sea externo a la api y debemos mapearlo, etc. son objetos que transportan informacion
         */

        /*
            public long ID {
                get {
                    return _id;
                },
                set
                {
                    _id = value;
                }
            }
        */
    }
}
