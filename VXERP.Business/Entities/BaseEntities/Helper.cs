using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Business.Entities.BaseEntities
{
    /// <summary>
    /// Especial para decorar las propiedades que no quieren
    /// ser mostrados en el gridview
    /// </summary>
    public class InvisibleAttribute : Attribute
    {
    }

    /// <summary>
    /// Para buscar la relacion entre una propiedad de una entidad del modelo 
    /// y una entidad del viewModel
    /// </summary>
    public class NamePropertyEntityAttribute : Attribute
    {
        public NamePropertyEntityAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para usar en la creacion de los listbox y tome el campo que uno configuro como
    /// valores a mostrar en el combo, por ahora debe ser utilizado en el modelo
    /// que contiene el fk como id, relacionando el nombre de la propiedad que contiene
    /// ese modelo a ser mostrado en el listbox
    /// </summary>
    public class NamePropertylistBoxAttribute : Attribute
    {
        public NamePropertylistBoxAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para usar en los gridview de devexpress, y poder cambiar el nombre de la columna que
    /// se genera automaticamente luego del binding
    /// </summary>
    public class ColumnNameGridViewAttribute : Attribute
    {
        public ColumnNameGridViewAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para usar en los enums y genere un listbox con los valores
    /// </summary>
    public class PropertyEnumAttribute : Attribute
    {
        public PropertyEnumAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para que la propiedad de un modelo figure como html hidden
    /// </summary>
    public class HiddenPropertyAttribute : Attribute
    {
        public HiddenPropertyAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para columnas de tipo imagen en el gridview
    /// </summary>
    public class ColumnImageGridViewAttribute : Attribute
    {
        public ColumnImageGridViewAttribute()
        {
        }
    }

    /// <summary>
    /// Para los formularios que pueda armar automaticamente
    /// las propiedades de tipo file
    /// </summary>
    public class FilePropertyAttribute : Attribute
    {
        public FilePropertyAttribute()
        {
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Para las propiedades que no son requeridas pero deben mostrarse en el formulario
    /// </summary>
    public class NotRequiredAndVisibleAttribute : Attribute
    {
        public NotRequiredAndVisibleAttribute()
        {
        }

        public string Name { get; set; }
    }

    public class SkipPreventSpamActionFilterAttribute : Attribute
    {
    }

    public class NamePropertylistBoxRelationAttribute : Attribute
    {
        public NamePropertylistBoxRelationAttribute()
        {
        }

        public string Name { get; set; }
    }

    public class StyleCssAttribute : Attribute
    {
        public StyleCssAttribute()
        {
        }

        public string Class { get; set; }
    }
}
