using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Entities
{
    public class CharacterMovie
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int MovieId { get; set; }
    }
}
