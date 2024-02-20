namespace WebApi.DTOs.Clients
{
    public class ClientListDTO
    {
        public List<ClientDTO> Clients { get; set; }

        public int Total { get; set; }
    }
}
