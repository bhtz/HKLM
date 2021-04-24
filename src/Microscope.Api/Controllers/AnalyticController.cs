using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microscope.Infrastructure;
using Microscope.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microscope.Application.Core.Queries.Analytic;

namespace Microscope.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticController : ControllerBase
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMediator _mediator;

        public AnalyticController(MicroscopeDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/Analytic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalyticQueryResult>>> GetAnalytics()
        {
            var results = await this._mediator.Send(new FilteredAnalyticQuery());
            return Ok(results);
        }

        // GET: api/Analytic/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Analytic>> GetAnalytic(Guid id)
        {
            var analytic = await _context.Analytics.FindAsync(id);

            if (analytic == null)
            {
                return NotFound();
            }

            return analytic;
        }

        // PUT: api/Analytic/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnalytic(Guid id, Analytic analytic)
        {
            if (id != analytic.Id)
            {
                return BadRequest();
            }

            _context.Entry(analytic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnalyticExists(id))
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

        // POST: api/Analytic
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Analytic>> PostAnalytic(Analytic analytic)
        {
            _context.Analytics.Add(analytic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnalytic", new { id = analytic.Id }, analytic.Id.ToString());
        }

        // DELETE: api/Analytic/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalytic(Guid id)
        {
            var analytic = await _context.Analytics.FindAsync(id);
            if (analytic == null)
            {
                return NotFound();
            }

            _context.Analytics.Remove(analytic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnalyticExists(Guid id)
        {
            return _context.Analytics.Any(e => e.Id == id);
        }
    }
}
