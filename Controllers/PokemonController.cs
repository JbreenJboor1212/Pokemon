using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon.Data;
using Pokemon.Dto;
using Pokemon.Interfaces;
using Pokemon.Model;
using Pokemon.Repository;
using System.Collections;

namespace Pokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(
            IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200 , Type = typeof(IEnumerable<Pokemont>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetAllPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }


        [HttpGet("{pokeId}")]
        [ProducesResponseType(200 , Type = typeof(Pokemont))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemont))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {

            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }


        [HttpPost]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)] // Bad Request
        [ProducesResponseType(422)] // Unprocessable Entity
        [ProducesResponseType(500)] // Internal Server Error
        public IActionResult CreatePokemont([FromQuery] int ownerId , [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreateDto)
        {

            if (pokemonCreateDto == null)
            {
                ModelState.AddModelError("", "Invalid Pokemon data");
                return BadRequest(ModelState);
            }

            var existingPokemon = _pokemonRepository.GetAllPokemons()
                .FirstOrDefault(p => string.Equals(p.Name, pokemonCreateDto.Name, StringComparison.OrdinalIgnoreCase));

            if (existingPokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemont>(pokemonCreateDto);

            if (pokemonMap == null)
            {
                ModelState.AddModelError("", "Mapping resulted in a null Pokemon object");
                return StatusCode(500, ModelState);
            }

            try
            {
                if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError("", dbEx.InnerException?.Message ?? dbEx.Message);
                return StatusCode(500, ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred");
                return StatusCode(500, ModelState);
            }
        }



        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updatePokemon(
            int pokeId,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDto updatedPokemon)
        {
            //400
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            //400
            if (pokeId != updatedPokemon.Id)
                return BadRequest(ModelState);



            //404
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            //404
            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemont>(updatedPokemon);

            //204
            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }


        [HttpDelete("{poekId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult deletePokemon(int poekId)
        {

            //404
            if (!_pokemonRepository.PokemonExists(poekId))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfAPokemon(poekId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(poekId);

            //404
            if (!ModelState.IsValid)
                return BadRequest();

            //204
            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting Reviews");
                return StatusCode(500, ModelState);
            }

            //204
            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
