using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Dto;
using Pokemon.Interfaces;
using Pokemon.Model;
using Pokemon.Repository;

namespace Pokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController
            (IOwnerRepository ownerRepository ,
            ICountryRepository countryRepository ,
            IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }


        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (_ownerRepository.OwnersExist(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        //[HttpGet("pokemon/{pokeId}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        //[ProducesResponseType(400)]
        //public IActionResult GetOwnerOfAPokemon(int pokeId)
        //{
        //
        //    var owners = _mapper
        //        .Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokeId));
        //
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //
        //    return Ok(owners);
        //}


        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemont>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonOfAOwner(int ownerId)
        {
            if (!_ownerRepository.OwnersExist(ownerId))
                return NotFound();

            var pokemons = _mapper
                .Map<List<PokemonDto>>(_ownerRepository.GetPokemonOfAOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createOwner([FromQuery] int countryId , [FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var ownerExist = _ownerRepository.GetOwners()
                .Where(c => c.LastName.Trim().ToLower() == ownerCreate.LastName.TrimEnd().ToLower());

            if (ownerExist == null)
            {
                ModelState.AddModelError("", "Owner Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult updateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            //400
            if (updatedOwner == null)
                return BadRequest(ModelState);

            //400
            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);



            //404
            if (!_ownerRepository.OwnersExist(ownerId))
                return NotFound();

            //404
            if (!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            //204
            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }


        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {

            //404
            if (!_ownerRepository.OwnersExist(ownerId))
                return NotFound();


            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            //404
            if (!ModelState.IsValid)
                return BadRequest();

            //204
            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
