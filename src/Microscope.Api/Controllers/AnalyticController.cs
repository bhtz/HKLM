using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microscope.Infrastructure;
using Microscope.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microscope.Application.Features.Analytic.Commands;
using Microscope.Application.Features.Analytic.Queries;

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
        public async Task<IActionResult> PutAnalytic(Guid id, EditAnalyticCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await this._mediator.Send(command);


            return Ok();
        }

        // POST: api/Analytic
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Analytic>> PostAnalytic(AddAnalyticCommand command)
        {
            Guid idCreated = await this._mediator.Send(command);
            return CreatedAtAction("GetAnalytic", new { id = idCreated }, idCreated.ToString());
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
