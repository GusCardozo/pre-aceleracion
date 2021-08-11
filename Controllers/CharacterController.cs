using Challenge.Aceleracion.Context;
using Challenge.Aceleracion.Entities;
using Challenge.Aceleracion.ViewModels.Characters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Challenge.Aceleracion.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    //[Authorize(Roles = "Admin")]
    public class CharacterController : ControllerBase
    {
        private readonly DisneyContext _context;

        public CharacterController(DisneyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCharacters()
        {
            return Ok(_context.Characters
                .Select(x => new { Name = x.Name, Image = x.Image })
                .ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("characters")]
        public IActionResult GetCharacter(string name, int? age, int? movies)
        {
            List<CharacterResponseModel> characterModels = new();

            if (age != null)
            {
                var characters = _context.Characters.Include(x => x.Movies).Where(x => x.Name == name && x.Age == age).ToList();

                if (characters == null)
                {
                    return NotFound("The character does not exist");
                }

                characterModels = setResponse(characters);

                return Ok(characterModels);
            }

            if (movies != null)
            {
                var characters = (from c in _context.Characters
                                  join cm in _context.CharacterMovies on c.Id equals cm.CharacterId
                                  join m in _context.Movies on cm.MovieId equals m.Id
                                  where c.Name == name
                                  select c)
                                  .Include(x => x.Movies)
                                  .ToList();

                if (characters == null)
                {
                    return NotFound("The character does not exist");
                }

                characterModels = setResponse(characters);

                return Ok(characterModels);
            }

            var character = _context.Characters.Include(x => x.Movies).Where(x => x.Name == name).ToList();

            if (character == null)
            {
                return NotFound("The character does not exist");
            }

            characterModels = setResponse(character);

            return Ok(characterModels);
        }

        private List<CharacterResponseModel> setResponse(List<Character> characters)
        {
            List<CharacterResponseModel> characterModels = new();

            foreach (var character in characters)
            {
                characterModels.Add(new CharacterResponseModel()
                {
                    Id = character.Id,
                    Name = character.Name,
                    Image = character.Image,
                    Age = character.Age,
                    Weight = character.Weight,
                    History = character.History,
                    Movie = character.Movies
                });  
            }

            return characterModels;
        }

        [HttpPost]
        public IActionResult Post(CharacterRequestModel character)
        {
            var newChar = new Character
            {
                Image = character.Image,
                Name = character.Name,
                Age = character.Age,
                Weight = character.Weight,
                History = character.History
            };

            _context.Characters.Add(newChar);

            _context.SaveChanges();

            return Ok(new CharacterResponseModel 
            { 
                Id = newChar.Id,
                Image = newChar.Image,
                Name = newChar.Name,
                Age = newChar.Age,
                Weight = newChar.Weight,
                History = newChar.History
            });
        }

        [HttpPut]
        public IActionResult Put(CharacterRequestModel character)
        {
            var characterP = _context.Characters.FirstOrDefault(x => x.Id == character.Id);

            if (characterP == null)
            {
                return NotFound("The character does not exist");
            }

            characterP.Image = character.Image;
            characterP.Name = character.Name;
            characterP.Age = character.Age;
            characterP.Weight = character.Weight;
            characterP.History = character.History;

            _context.Characters.Update(characterP);

            _context.SaveChanges();

            return Ok(new CharacterResponseModel
            {
                Id = characterP.Id,
                Image = characterP.Image,
                Name = characterP.Name,
                Age = characterP.Age,
                Weight = characterP.Weight,
                History = characterP.History
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleteChar = _context.Characters.Find(id);

            if (deleteChar == null) return NotFound("The character does not exist");

            _context.Remove(deleteChar);

            _context.SaveChanges();

            return Ok();
        }
    }
}
