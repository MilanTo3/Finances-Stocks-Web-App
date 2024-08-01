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

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : Controller
    {

        private readonly UserManager<AppUser> um;
        private readonly StockRepository stockRepository;
        private readonly PortfolioRepository portfolioRepository;
        
        RepositoryManager repositoryManager;
        public PortfolioController(IRepositoryManager rm, UserManager<AppUser> userManager){
            stockRepository = (StockRepository?)rm.stockRepo;
            um = userManager;
            portfolioRepository = (PortfolioRepository?)rm.portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPortfolios(){

            var username = User.GetUsername();
            var appUser = await um.FindByNameAsync(username);
            var userPortfolio = await portfolioRepository.getUserPortfolio(appUser);

            return Ok(userPortfolio);
        }
    }
}