using AspNetCore.Identity.Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleMvc.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
