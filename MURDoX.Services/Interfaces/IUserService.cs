using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Interfaces
{
    public interface IUserService
    {
        Task UpdateOrAddXpToUser(string user, int amount);
    }
}
