using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Configuration.Interfaces
{
    public interface ITypeInitializer
    {
        Task Init(ApplicationContext context);
    }
}
