namespace CustomersApi.DTOS
{
    public class CustomerDTO
    {
        //private long _id; es lo mismo que abajo

        // modificadores de acceso, public, private, static, internal, protected, etc. accesso desde dentro o fuera de la clase
        // .NET nos da Model Binding en los objetos que llegan a nuestra API, permitiendonos agregar logica en el objeto, por ej campos obligatorios
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
