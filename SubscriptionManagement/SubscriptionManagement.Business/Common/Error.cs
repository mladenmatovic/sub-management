using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Common
{
    public sealed record Error(string Code, string? Message = null);
}
