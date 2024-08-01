using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _repo;
        private readonly IStockRepository _srepo;
        private readonly IFMPService _fmpService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(IRepositoryManager stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _repo = stockRepo.commentRepo;
            _unitOfWork = stockRepo.unitOfWork;
            _userManager = userManager;
            _srepo = stockRepo.stockRepo;
            _fmpService = fmpService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComment([FromRoute]int id){

            var s = await _repo.getById(id);
            if(s != null){
                var retval = s.Adapt<CommentDTO>();
                return Ok(retval);
            }else{
                return BadRequest("Stock by that id doesn't exist.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(){

            var k = await _repo.getAll();
            var p = k.Adapt<IEnumerable<CommentDTO>>();

            return Ok(p);
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> CreateComment([FromRoute] string symbol, [FromBody] CommentDTO dto){

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _srepo.getBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists");
                }
                else
                {
                    await _srepo.Add(stock);
                }
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = dto.Adapt<Comment>();
            commentModel.AppUserId = appUser.Id;
            await _repo.Add(commentModel);
            return CreatedAtAction(nameof(GetComment), new { id = commentModel.Id }, commentModel.Adapt<CommentDTO>());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int id, [FromBody] CommentDTO dto)
        {

            if(!ModelState.IsValid){
                return BadRequest();
            }

            bool s = await _repo.Update(dto.Adapt<Comment>(), id);
            if(s){
                await _unitOfWork.Complete();
            }else{
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id){

            bool s = await _repo.Delete(id);
            if(!s){
                return NotFound();
            }

            await _unitOfWork.Complete();

            return NoContent();
        }

        
    }
}