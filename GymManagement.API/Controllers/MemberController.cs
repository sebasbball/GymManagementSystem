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
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public MemberController(IMemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetAll()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<MemberResponseDTO>>(members));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponseDTO>> GetById(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                return NotFound(new { message = $"Member with ID {id} not found." });
            return Ok(_mapper.Map<MemberResponseDTO>(member));
        }

        [HttpPost]
        public async Task<ActionResult<MemberResponseDTO>> Create(MemberRequestDTO dto)
        {
            try
            {
                var member = _mapper.Map<Member>(dto);
                var created = await _memberService.CreateAsync(member);
                var response = _mapper.Map<MemberResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MemberRequestDTO dto)
        {
            try
            {
                var member = _mapper.Map<Member>(dto);
                await _memberService.UpdateAsync(id, member);
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
                await _memberService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}