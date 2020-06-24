using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Common.EntityDomain
{
    public interface IEntityTypedId<TId>
    {
        TId Id { get; set; }
    }
}
