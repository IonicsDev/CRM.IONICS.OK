using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CRM.Common.EntityDomain
{
    [Serializable]
    public abstract class EntityTypedId<TId> : IEntityTypedId<TId>
    {
        [Key]
        public virtual TId Id { get; set; }
    }
}
