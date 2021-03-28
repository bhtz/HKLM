using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microscope.Infrastructure;
using Microscope.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Microscope.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemoteConfigController : ControllerBase
    {
        private readonly MicroscopeDbContext _context;

        public RemoteConfigController(MicroscopeDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> PutRemoteConfig(Guid id, RemoteConfig remoteConfig)
        {
            if (id != remoteConfig.Id)
            {
                return BadRequest();
            }

            _context.Entry(remoteConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RemoteConfigExists(id))
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

        // POST: api/RemoteConfig
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RemoteConfig>> PostRemoteConfig(RemoteConfig remoteConfig)
        {
            _context.RemoteConfigs.Add(remoteConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRemoteConfig", new { id = remoteConfig.Id }, remoteConfig);
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

        private bool RemoteConfigExists(Guid id)
        {
            return _context.RemoteConfigs.Any(e => e.Id == id);
        }
    }
}
