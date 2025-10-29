using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDataSeeding
    {
        void SeedData(); // implementation in the presistence layer (the place that implement the domain layer)
        Task SeedIdentityDataAsync();
    }
}
