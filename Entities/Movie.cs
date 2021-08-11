using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Entities
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        [RegularExpression("[1-5]")]
        [MaxLength(1)]
        public int Qualification { get; set; }
        public int GenresId { get; set; }
        public Genre Genres { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
