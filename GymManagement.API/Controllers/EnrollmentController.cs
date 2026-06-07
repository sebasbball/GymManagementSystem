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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;

        public EnrollmentController(IEnrollmentService enrollmentService, IMapper mapper)
        {
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDTO>>> GetByMember(
            int memberId)
        {
            try
            {
                var enrollments = await _enrollmentService.GetByMemberAsync(memberId);
                return Ok(_mapper.Map<IEnumerable<EnrollmentResponseDTO>>(enrollments));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("class/{gymClassId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDTO>>> GetByClass(
            int gymClassId)
        {
            try
            {
                var enrollments = await _enrollmentService.GetByClassAsync(gymClassId);
                return Ok(_mapper.Map<IEnumerable<EnrollmentResponseDTO>>(enrollments));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDTO>> Enroll(EnrollmentRequestDTO dto)
        {
            try
            {
                var enrollment = _mapper.Map<Enrollment>(dto);
                var created = await _enrollmentService.EnrollAsync(enrollment);

                // Reload with full details for the response
                var withDetails = await _enrollmentService
                    .GetByMemberAsync(created.MemberId);
                var createdWithDetails = withDetails
                    .FirstOrDefault(e => e.Id == created.Id);

                return Ok(_mapper.Map<EnrollmentResponseDTO>(createdWithDetails));
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
        public async Task<ActionResult> Unenroll(int id)
        {
            try
            {
                await _enrollmentService.UnenrollAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}