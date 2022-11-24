using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prs_serverside_2022.Models;

namespace prs_serverside_2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(r => r.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.Include(r => r.User).Include(r => r.RequestLines)
                                                    .ThenInclude(rl => rl.Product).SingleOrDefaultAsync(r => r.Id == id);

            return (request != null) ? request : NotFound();
        }
        [HttpGet("reviews/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int userId)
        {
            return await _context.Requests
                                        .Include(r => r.User)
                                        .Where(r => r.Status.Equals("REVIEW") && r.UserId != userId)
                                        .ToListAsync();
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id) return BadRequest();

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }
        [HttpPut("review")]
        public async Task<IActionResult> SetReview(Request request)
        {
            request.Status = request.Total <= 50 ? "Approved" : "Review";
            //_context.Entry(request).State = EntityState.Modified;  Used to alter data with PUT
            //await _context.SaveChangesAsync();
            //return Ok();
            return await PutRequest(request.Id, request);
        }
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> SetApproved(int id, Request request)
        {
            var req = await _context.Requests.FindAsync(request.Id);
            if (req is null) return NotFound();
            req.Status = "APPROVED";
            req.RejectionReason = null;
            return await PutRequest(req.Id, req);
        }
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> SetRejected(int id, Request request)
        {
            var req = await _context.Requests.FindAsync(request.Id);
            if (req is null) return NotFound();
            req.Status = "REJECTED";
            req.RejectionReason = request.RejectionReason;
            return await PutRequest(req.Id, req);
        }


        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request is null) return NotFound();

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
