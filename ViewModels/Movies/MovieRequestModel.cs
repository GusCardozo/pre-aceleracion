using Challenge.Aceleracion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.ViewModels.Movies
{
    public class MovieRequestModel
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int Qualification { get; set; }
        public int GenresId { get; set; }
        public List<int> CharacterId { get; set; }
    }
}
