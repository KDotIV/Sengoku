using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using Sengoku.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Controllers
{
    [Route("api/clips")]
    [ApiController]
    public class ClipController : Controller
    {
        private readonly ClipRepository _clipRepository;
        public ClipController(ClipRepository clipRepository)
        {
            _clipRepository = clipRepository;
        }

        [HttpGet]
        [Route("GetAllClips")]
        public async Task<ActionResult> GetAllClipsAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var clips = await _clipRepository.GetAllClips(limit, page, sort);
            return Ok(clips);
        }

        [HttpGet]
        [Route("GetClip/id/{clipId}")]
        public async Task<ActionResult> GetClipByIDAsync(string clipId)
        {
            var clip = await _clipRepository.GetClipById(clipId);
            return Ok(clip);
        }

        [HttpGet]
        [Route("GetClip/name/{clipName}")]
        public async Task<ActionResult> GetClipByNameAsync(string clipName)
        {
            var clip = await _clipRepository.GetClipByName(clipName);
            return Ok(clip);
        }

        [HttpGet]
        [Route("GetClip/game/{gameName}")]
        public async Task<ActionResult> GetClipsByGameAsync(string gameName)
        {
            var clips = await _clipRepository.GetClipByGame(gameName);
            return Ok(clips);
        }

        [HttpPost]
        [Route("CreateClip")]
        public async Task<ActionResult> CreateClipAsync([FromBody] Clip newClip)
        {
            var response = await _clipRepository.AddClip(newClip.url, newClip.name, newClip.game);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteClip/{clipId}")]
        public async Task<ActionResult> DeleteClipAsync(string clipId)
        {
            var response = await _clipRepository.DeleteClip(clipId);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }

        [HttpPut]
        [Route("UpdateClip/url/{clipId}")]
        public async Task<ActionResult> UpdateClipByUrlAsync(string clipId, [FromBody] Clip updatedClip)
        {
            var response = await _clipRepository.UpdateClipUrl(clipId, updatedClip.url);
            if (!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }
            return Ok(response);
        }
    }
}
