namespace EcAPI.Entities
{
    public class User
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? UserGroupId { get; set; }
        public string? CreationDateTime { get; set; }
        public string? LastUpdateDateTime { get; set; }
    }
}
