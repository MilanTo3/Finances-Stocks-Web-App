using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/comment")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IRepositoryManager stockRepo)
        {
            _repo = stockRepo.commentRepo;
            _unitOfWork = stockRepo.unitOfWork;
        }

        [HttpGet("{id}")]
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

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateComment([FromRoute] int id, [FromBody] CommentDTO dto){

            var stockmodel = dto.Adapt<Comment>();
            stockmodel.StockId = id;
            bool p = await _repo.Add(stockmodel);
            if(p){
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(GetComment), new { id = stockmodel.Id }, stockmodel.Adapt<CommentDTO>());
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int id, [FromBody] CommentDTO dto)
        {

            bool s = await _repo.Update(dto.Adapt<Comment>(), id);
            if(s){
                await _unitOfWork.Complete();
            }else{```
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
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