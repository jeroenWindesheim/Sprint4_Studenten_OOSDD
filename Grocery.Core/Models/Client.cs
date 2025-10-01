[Flags]
public enum Role
{
    None = 0,
    Admin = 1
}

namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Metb;
        public Client(int id, string name, string emailAddress, string password, Role metb = Role.None) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            Metb = metb;
        }
    }
}
