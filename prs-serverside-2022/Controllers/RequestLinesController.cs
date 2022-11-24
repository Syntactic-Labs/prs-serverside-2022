using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prs_serverside_2022.Models;

namespace prs_serverside_2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestLinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestLinesController(AppDbContext context)
        {
            _context = context;
        }
        
        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestsLine()
        {
            return await _context.RequestLines.ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine is null) return NotFound();

            return requestLine;
        }

        // PUT: api/RequestLines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id) return BadRequest();

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateRequestTotal(requestLine.RequestId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/RequestLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestLine.RequestId);

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine is null) return NotFound();

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestLine.RequestId);
            return NoContent();
        }

        private async Task RecalculateRequestTotal(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if (request is null) throw new ArgumentException("Invalid requestId");

            request.Total = (from rl in _context.RequestLines
                             join p in _context.Products
                             on rl.ProductId equals p.Id
                             where rl.RequestId == requestId
                             select new
                             {
                                 LineTotal = rl.Quantity * p.Price
                             }).Sum(x => x.LineTotal);

            request.Status = "EDIT";
            var reqCtrl = new RequestsController(_context);
            await reqCtrl.PutRequest(request.Id, request);
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
