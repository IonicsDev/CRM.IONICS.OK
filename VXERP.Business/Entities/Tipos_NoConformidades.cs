﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.Entities
{
    [Table("Tipos_NoConformidades")]
    public class Tipos_NoConformidades : BaseEntity
    {
        public Tipos_NoConformidades()
        { 
            
        }

        [Required(ErrorMessage = "El Tipo es requerido")]
        [Display(Name = "Descripcion")]
        [ColumnNameGridView(Name = "Descripcion")]
        public string Descripcion { get; set; }


        [NotMappedAttribute]
        [Display(Name = "Activo")]
        [ColumnNameGridView(Name = "Activo")]
        public string DescripcionActivo
        {
            get
            {
                return (Estado ? "Activo" : "Inactivo");
            }
        }



    }
}
