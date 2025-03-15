using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.SMTP
{
    public interface IEmailServices
    {
        Task SendResetPasswordEmailAsync(string email, string token);
    }
}
