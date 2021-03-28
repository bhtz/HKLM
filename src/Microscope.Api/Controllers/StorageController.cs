using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microscope.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microscope.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <summary>
        /// Save blob in container
        /// </summary>
        /// <param name="file"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{containerName}")]
        public async Task<IActionResult> SaveBlob([FromForm] IFormFile file, [FromRoute] string containerName)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    await this._storageService.SaveBlobAsync(containerName, file.FileName, ms);
                    return Ok();
                }
            }
            else
            {
                return BadRequest("Bad request : empty file");
            }
        }

        /// <summary>
        /// Download blob from container
        /// </summary>
        /// <param name="containerName">container name</param>
        /// <param name="blobName">blob name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{containerName}/{blobName}")]
        public async Task<FileContentResult> GetBlob([FromRoute] string containerName, [FromRoute] string blobName)
        {
            var stream = await this._storageService.GetBlobAsync(containerName, blobName);
            stream.Position = 0;

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return File(ms.ToArray(), "application/octet-stream", blobName);
            }
        }

        /// <summary>
        /// List blobs in container
        /// </summary>
        /// <param name="containerName">container name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{containerName}")]
        public async Task<IEnumerable<string>> ListBlob([FromRoute] string containerName)
        {
            var blobs = await this._storageService.ListBlobsAsync(containerName);
            return blobs;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> ListContainers()
        {
            var containers = await this._storageService.ListContainersAsync();
            return containers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateContainer([FromBody] string containerName)
        {
            await this._storageService.CreateContainerAsync(containerName);
            return Ok();
        }

        /// <summary>
        /// Delete blob in container
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{containerName}/{blobName}")]
        public async Task<IActionResult> DeleteBlob([FromRoute] string containerName, [FromRoute] string blobName)
        {
            try
            {
                await this._storageService.DeleteBlobAsync(containerName, blobName);
                return Ok();   
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}