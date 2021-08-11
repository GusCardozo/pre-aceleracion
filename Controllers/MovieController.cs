using Challenge.Aceleracion.Context;
using Challenge.Aceleracion.Entities;
using Challenge.Aceleracion.ViewModels.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Aceleracion.Controllers
{
    [ApiController]
    [Route ("api/[Controller]")]
    public class MovieController : ControllerBase
    {
        private readonly DisneyContext _context;

        public MovieController(DisneyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            return Ok(_context.Movies
                .Select(x => new { Title = x.Title, Image = x.Image, CreationDate = x.CreationDate })
                .ToList());
        }

        [HttpGet]
        [Route("movies")]
        public IActionResult GetMovie(string title, int? genre, string order)
        {
            List<MovieResponseModel> movieModels = new();

            if (genre != null)
            {
                var movie = _context.Movies.Include(x => x.Characters).Include(x => x.Genres).Where(x => x.Title == title && x.GenresId == genre).ToList();

                if (movie == null)
                {
                    return NotFound("The movie does not exist");
                }

                movieModels = setResponse(movie);

            }
            else
            {
                var movie = _context.Movies.Include(x => x.Characters).Include(x => x.Genres).Where(x => x.Title == title).ToList();

                if (movie == null)
                {
                    return NotFound("The movie does not exist");
                }

                movieModels = setResponse(movie);
            }
            
            if (order == "ASC")
            {
                movieModels.OrderBy(x => x.CreationDate);
            }
            else
            {
                movieModels.OrderByDescending(x => x.CreationDate);
            }

            return Ok(movieModels);
        }

        private List<MovieResponseModel> setResponse(List<Movie> movies)
        {
            List<MovieResponseModel> movieModels = new();

            foreach (var movie in movies)
            {
                movieModels.Add(new MovieResponseModel()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Image = movie.Image,
                    CreationDate = movie.CreationDate,
                    Qualification = movie.Qualification,
                    GenresId = movie.GenresId,
                    Character = movie.Characters
                });
            }

            return movieModels;
        }

        [HttpPost]
        public IActionResult Post(MovieRequestModel movie)
        {
            var newMovie = new Movie
            {
                Image = movie.Image,
                Title = movie.Title,
                CreationDate = movie.CreationDate,
                Qualification = movie.Qualification,
            };

            if (movie.GenresId != 0)
            {
                var genre = _context.Genres.FirstOrDefault(x => x.Id == movie.GenresId);

                if (genre != null)
                {
                    if (newMovie.Genres == null) newMovie.Genres = new Genre();
                    newMovie.Genres = genre;
                }
            }

            _context.Movies.Add(newMovie);

            _context.SaveChanges();

            newMovie.Characters = new List<Character>();

            foreach (var characterId in movie.CharacterId)
            {
                if (characterId != 0)
                {
                    newMovie.Characters.Add(_context.Characters.Find(characterId));

                    CharacterMovie characterMovie = new();

                    characterMovie.CharacterId = characterId;
                    characterMovie.MovieId = newMovie.Id;

                    _context.CharacterMovies.Add(characterMovie);

                    _context.SaveChanges();
                }
            }

            return Ok(new MovieResponseModel
            {
                Id = newMovie.Id,
                Image = newMovie.Image,
                Title = newMovie.Title,
                CreationDate = newMovie.CreationDate,
                Qualification = newMovie.Qualification,
                GenresId = newMovie.Genres.Id,
                Character = newMovie.Characters
            });
        }

        [HttpPut]
        public IActionResult Put(Movie movie)
        {
            var mov = _context.Movies.FirstOrDefault(x => x.Id == movie.Id);

            if (mov == null)
            {
                return NotFound("The movie does not exist");
            }

            mov.Image = movie.Image;
            mov.Title = movie.Title;
            mov.CreationDate = movie.CreationDate;
            mov.Qualification = movie.Qualification;

            _context.Movies.Update(mov);

            _context.SaveChanges();

            return Ok(_context.Movies.ToList());
        }

        [HttpDelete]
        public IActionResult Delete(Movie movie)
        {
            _context.Remove(movie);

            _context.SaveChanges();

            return Ok(_context.Movies.ToList());
        }
    }
}
