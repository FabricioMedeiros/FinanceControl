using Microsoft.AspNetCore.Identity;

namespace FinanceControl.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
