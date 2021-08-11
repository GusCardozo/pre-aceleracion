using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Entities
{
    public class Character
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(3)]
        public int Age { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public string History { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
