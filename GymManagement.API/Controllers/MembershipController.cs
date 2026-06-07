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
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IMapper _mapper;

        public MembershipController(IMembershipService membershipService, IMapper mapper)
        {
            _membershipService = membershipService;
            _mapper = mapper;
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<MembershipResponseDTO>>> GetByMember(
            int memberId)
        {
            try
            {
                var memberships = await _membershipService.GetByMemberAsync(memberId);
                return Ok(_mapper.Map<IEnumerable<MembershipResponseDTO>>(memberships));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipResponseDTO>> GetById(int id)
        {
            var membership = await _membershipService.GetByIdAsync(id);
            if (membership == null)
                return NotFound(new { message = $"Membership with ID {id} not found." });
            return Ok(_mapper.Map<MembershipResponseDTO>(membership));
        }

        [HttpPost]
        public async Task<ActionResult<MembershipResponseDTO>> Create(MembershipRequestDTO dto)
        {
            try
            {
                var membership = _mapper.Map<Membership>(dto);
                var created = await _membershipService.CreateAsync(membership);
                var response = _mapper.Map<MembershipResponseDTO>(created);
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
        public async Task<ActionResult> Update(int id, MembershipRequestDTO dto)
        {
            try
            {
                var membership = _mapper.Map<Membership>(dto);
                await _membershipService.UpdateAsync(id, membership);
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
                await _membershipService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}