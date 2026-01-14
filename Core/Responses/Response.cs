namespace Core.Responses
{
    public class Response<Entity>
    {
        public bool Ok { get; set; }
        public string? Mensaje { get; set; }
        public Entity? Datos { get; set; }
    }
}