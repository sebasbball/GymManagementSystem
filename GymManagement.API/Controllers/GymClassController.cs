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
    public class GymClassController : ControllerBase
    {
        private readonly IGymClassService _gymClassService;
        private readonly IMapper _mapper;

        public GymClassController(IGymClassService gymClassService, IMapper mapper)
        {
            _gymClassService = gymClassService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GymClassResponseDTO>>> GetAll()
        {
            var classes = await _gymClassService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<GymClassResponseDTO>>(classes));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GymClassResponseDTO>> GetById(int id)
        {
            var gymClass = await _gymClassService.GetByIdAsync(id);
            if (gymClass == null)
                return NotFound(new { message = $"Gym class with ID {id} not found." });
            return Ok(_mapper.Map<GymClassResponseDTO>(gymClass));
        }

        [HttpGet("trainer/{trainerId}")]
        public async Task<ActionResult<IEnumerable<GymClassResponseDTO>>> GetByTrainer(
            int trainerId)
        {
            try
            {
                var classes = await _gymClassService.GetByTrainerAsync(trainerId);
                return Ok(_mapper.Map<IEnumerable<GymClassResponseDTO>>(classes));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<GymClassResponseDTO>> Create(GymClassRequestDTO dto)
        {
            try
            {
                var gymClass = _mapper.Map<GymClass>(dto);
                var created = await _gymClassService.CreateAsync(gymClass);

                // Reload with trainer details for the response
                var withDetails = await _gymClassService.GetByIdAsync(created.Id);
                var response = _mapper.Map<GymClassResponseDTO>(withDetails);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, GymClassRequestDTO dto)
        {
            try
            {
                var gymClass = _mapper.Map<GymClass>(dto);
                await _gymClassService.UpdateAsync(id, gymClass);
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
                await _gymClassService.DeleteAsync(id);
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

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, UpdateClassStatusDTO dto)
        {
            try
            {
                await _gymClassService.UpdateStatusAsync(id, dto.Status);
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
    }
}