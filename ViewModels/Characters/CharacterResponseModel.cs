using Challenge.Aceleracion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.ViewModels.Characters
{
    public class CharacterResponseModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public string History { get; set; }
        public ICollection<Movie> Movie { get; set; }
    }
}
