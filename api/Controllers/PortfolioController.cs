using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.AppDbContext;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Extensions;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : Controller
    {

        private readonly UserManager<AppUser> um;
        private readonly StockRepository stockRepository;
        private readonly PortfolioRepository portfolioRepository;
        private readonly UnitOfWork uw;
        
        RepositoryManager repositoryManager;
        public PortfolioController(IRepositoryManager rm, UserManager<AppUser> userManager){
            stockRepository = (StockRepository?)rm.stockRepo;
            um = userManager;
            portfolioRepository = (PortfolioRepository?)rm.portfolioRepo;
            uw = (UnitOfWork?)rm.unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPortfolios(){

            var username = User.GetUsername();
            var appUser = await um.FindByNameAsync(username);
            var userPortfolio = await portfolioRepository.getUserPortfolio(appUser);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol){

            var username = User.GetUsername();
            var appUser = await um.FindByNameAsync(username);
            var stock = await stockRepository.getBySymbolAsync(symbol);

            if(stock == null){ return BadRequest(); }

            Portfolio port = new Portfolio(){StockId = stock.Id, AppUserId = appUser.Id, AppUser = appUser, Stock = stock};
            bool k = await portfolioRepository.Add(port);
            if(k){
                await uw.Complete();
                return Ok(port);
            }

            return StatusCode(500, "Could not create portfolio.");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol){

            var username = User.GetUsername();
            var appUser = await um.FindByNameAsync(username);

            var userPortfolio = await portfolioRepository.getUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count() == 1){

                var success = await portfolioRepository.DeletePortfolio(appUser, symbol);
                if(success != null){
                    return Ok();
                }else{
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        
    }
}