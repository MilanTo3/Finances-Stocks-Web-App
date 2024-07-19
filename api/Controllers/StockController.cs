using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.AppDbContext;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mapster;
using api.DTOs;
using api.Models;
using api.Interfaces;
using api.Repositories;

namespace api.Controllers
{
    [Route("api/stock")]
    public class StockController : ControllerBase
    {

        private readonly IStockRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public StockController(IRepositoryManager stockRepo)
        {
            _repo = stockRepo.stockRepo;
            _unitOfWork = stockRepo.unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute]int id){
            
            var s = await _repo.getById(id);
            if(s != null){
                var retval = s.Adapt<StockDTO>();
                return Ok(retval);
            }else{
                return BadRequest("Stock by that id doesn't exist.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetStocks(){

            var k = await _repo.getAll();
            var p = k.Adapt<IEnumerable<StockDTO>>();

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO dto){

            var stockmodel = dto.Adapt<Stock>();
            bool p = await _repo.Add(stockmodel);
            if(p){
                await _unitOfWork.Complete();
                return CreatedAtAction(nameof(GetStock), new { id = stockmodel.Id }, stockmodel.Adapt<StockDTO>());
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStockAsync([FromRoute] int id, [FromBody] UpdateStockDTO dto)
        {

            bool s = await _repo.Update(dto.Adapt<Stock>(), id);
            if(s){
                await _unitOfWork.Complete();
            }else{
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id){
            
            bool s = await _repo.Delete(id);
            if(!s){
                return NotFound();
            }

            await _unitOfWork.Complete();

            return NoContent();
        }

    }

}