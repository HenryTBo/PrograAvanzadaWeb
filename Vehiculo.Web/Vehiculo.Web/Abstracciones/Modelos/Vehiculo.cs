using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class VehiculoBase
    {
        [Required(ErrorMessage ="La propiedad placa es requeridad")]
        [RegularExpression(@"[A-Za-z]{3}-[0-9]{3}",ErrorMessage ="El formato debe ser ###=ABC")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "La propiedad color es requeridad")]
        [StringLength(40,ErrorMessage ="La propiedad color debe ser mayor a 4 caracteres y menor a 40",MinimumLength = 4)]
        public string Color { get; set; }
        [Required(ErrorMessage = "La propiedad anio es requeridad")]
        [RegularExpression(@"(19|20)\d\d",ErrorMessage = "El formato del anio no es valido")]
        public int Anio { get; set; }
        [Required(ErrorMessage = "La propiedad precio es requeridad")]
        public Decimal Precio { get; set; }
        [Required(ErrorMessage = "La propiedad correo es requeridad")]
        [EmailAddress]
        [DisplayName("Correo del propietario")]
        public string CorreoPropietario { get; set; }
        [Required(ErrorMessage = "La propiedad telefono es requeridad")]
        [Phone]
        public string TelefonoPropietario { get; set; }

    }
    public class VehiculoRequest : VehiculoBase
    {
        public Guid IdModelo { get; set; }
    }
    public class VehiculoResponse : VehiculoBase
    {
        public Guid Id {  get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
    }
    public class VehiculoDetalle : VehiculoResponse
    {
        public bool RevisionValida { get; set; }
        public bool RegistroValido { get; set; }
    }
}
