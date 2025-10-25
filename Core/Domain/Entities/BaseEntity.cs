using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity<TKey> // Generic Class [BaseClass] Specify The PK Type
    {
        public TKey Id { get; set; }
    }
}
