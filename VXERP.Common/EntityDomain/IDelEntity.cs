using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Common.EntityDomain
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDelEntity
    {

        bool Estado { get; set; }
        System.DateTime FechaCreacion { get; set; }
        System.DateTime FechaActualizacion { get; set; }
        int? idUsuario { get; set; } 
    }
}
