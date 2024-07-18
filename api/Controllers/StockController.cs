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

namespace api.Controllers
{
    [Route("api/stock")]
    public class StockController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStock([FromRoute]int id){
            
            var k = await _context.Stocks.FindAsync(id);
            if(k != null){
                var retval = k.Adapt<StockDTO>();
                return Ok(retval);
            }else{
                return BadRequest("Stock by that id doesn't exist.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetStocks(){

            var k = (await _context.Stocks.ToListAsync()).Adapt<IEnumerable<StockDTO>>();

            return Ok(k);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO dto){

            var stockmodel = dto.Adapt<Stock>();
            _context.Stocks.Add(stockmodel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStock), new { id = stockmodel.Id }, stockmodel.Adapt<StockDTO>());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDTO dto){
            
            var stockmodel = await _context.Stocks.FindAsync(id);

            if(stockmodel == null){
                return NotFound();
            }

            
            stockmodel.Symbol = dto.Symbol;
            stockmodel.CompanyName = dto.CompanyName;
            stockmodel.Purchase = dto.Purchase;
            stockmodel.LastDiv = dto.LastDiv;
            stockmodel.Industry = dto.Industry;
            stockmodel.MarketCap = dto.MarketCap;

            _context.SaveChanges();

            return Ok(stockmodel.Adapt<StockDTO>());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id){
            
            var stockmodel = await _context.Stocks.FindAsync(id);
            if(stockmodel == null){
                return NotFound();
            }

            _context.Stocks.Remove(stockmodel);
            _context.SaveChanges();

            return NoContent();
        }

    }

}