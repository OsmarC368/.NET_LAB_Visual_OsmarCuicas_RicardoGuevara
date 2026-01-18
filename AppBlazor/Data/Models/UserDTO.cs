using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlazor.Data.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El Email es Requerido!")]
        [StringLength(225, ErrorMessage = "El Email es Demasiado Corto", MinimumLength =10)]
        public string email {get; set;} = string.Empty;

        [Required(ErrorMessage = "La Contraseña es Requerida!")]
        [StringLength(225, ErrorMessage = "La Contraseña es Debe Contener al Menos 5 Caracteres", MinimumLength =5)]
        public string password {get; set;} = string.Empty;

        [Required(ErrorMessage = "El Nombre es Requerido!")]
        [StringLength(225, ErrorMessage = "El Nombre es Demasiado Corto", MinimumLength =2)]
        public string name {get; set;} = string.Empty;

        [Required(ErrorMessage = "El Apellido es Requerido!")]
        [StringLength(225, ErrorMessage = "El Apellido es Demasiado Corto", MinimumLength =2)]
        public string lastname {get; set;} = string.Empty;
        public char userTypeID {get; set;} = '1';
    }
}