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
using Microscope.Application.Features.RemoteConfig.Commands;

namespace Microscope.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemoteConfigController : ControllerBase
    {
        private readonly MicroscopeDbContext _context;

        private readonly IMediator _mediator;


        public RemoteConfigController(MicroscopeDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        // GET: api/RemoteConfig
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RemoteConfig>>> GetRemoteConfigs()
        {
            return await _context.RemoteConfigs.ToListAsync();
        }

        // GET: api/RemoteConfig/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RemoteConfig>> GetRemoteConfig(Guid id)
        {
            var remoteConfig = await _context.RemoteConfigs.FindAsync(id);

            if (remoteConfig == null)
            {
                return NotFound();
            }

            return remoteConfig;
        }

        // PUT: api/RemoteConfig/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemoteConfig(Guid id, EditRemoteConfigCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await this._mediator.Send(command);

            return Ok();
        }

        // POST: api/RemoteConfig
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RemoteConfig>> PostRemoteConfig(AddRemoteConfigCommand command)
        {
            Guid idCreated = await this._mediator.Send(command);

            return CreatedAtAction("GetRemoteConfig", new { id = idCreated }, idCreated.ToString());
        }

        // DELETE: api/RemoteConfig/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRemoteConfig(Guid id)
        {
            var remoteConfig = await _context.RemoteConfigs.FindAsync(id);
            if (remoteConfig == null)
            {
                return NotFound();
            }

            _context.RemoteConfigs.Remove(remoteConfig);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
