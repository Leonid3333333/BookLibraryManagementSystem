using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibraryManagementSystem.Data;
using BookLibraryManagementSystem.Models;

namespace BookLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public MembersController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("GetMembers")]
        public ActionResult<IEnumerable<Member>> GetMembers() => _context.Members.ToList();

        [HttpGet("GetMember")]
        public ActionResult<Member> GetMember(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }

        [HttpPost("AddMember")]
        public ActionResult<Member> AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [HttpPut("UpdateMember")]
        public IActionResult EditMember(int id, Member updatedMember)
        {
            if (id != updatedMember.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedMember).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Members.Any(m => m.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteMember")]
        public IActionResult DeleteMember(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
