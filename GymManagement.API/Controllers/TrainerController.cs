using AutoMapper;
using GymManagement.API.DTOs.Request;
using GymManagement.API.DTOs.Response;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;
        private readonly IMapper _mapper;

        public TrainerController(ITrainerService trainerService, IMapper mapper)
        {
            _trainerService = trainerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerResponseDTO>>> GetAll()
        {
            var trainers = await _trainerService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TrainerResponseDTO>>(trainers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerResponseDTO>> GetById(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);
            if (trainer == null)
                return NotFound(new { message = $"Trainer with ID {id} not found." });
            return Ok(_mapper.Map<TrainerResponseDTO>(trainer));
        }

        [HttpPost]
        public async Task<ActionResult<TrainerResponseDTO>> Create(TrainerRequestDTO dto)
        {
            try
            {
                var trainer = _mapper.Map<Trainer>(dto);
                var created = await _trainerService.CreateAsync(trainer);
                var response = _mapper.Map<TrainerResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TrainerRequestDTO dto)
        {
            try
            {
                var trainer = _mapper.Map<Trainer>(dto);
                await _trainerService.UpdateAsync(id, trainer);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _trainerService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}