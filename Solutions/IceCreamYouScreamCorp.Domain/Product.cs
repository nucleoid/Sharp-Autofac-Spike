using System.ComponentModel.DataAnnotations;

namespace IceCreamYouScreamCorp.Domain
{
    using SharpArch.Domain.DomainModel;

    public class Product : Entity
    {
        [Required(ErrorMessage = "Must have a clever name!")]
        public virtual string Name { get; set; }
    }
}