using System.Text.Json.Serialization;

namespace api.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
