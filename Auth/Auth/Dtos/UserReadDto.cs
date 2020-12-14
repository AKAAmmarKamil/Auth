using System;
namespace Auth
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Usertype { get; set; }
        public string Province { get; set; }
    }
}
