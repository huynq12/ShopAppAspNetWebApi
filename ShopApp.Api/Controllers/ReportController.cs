using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Api.Interfaces;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("/report/topSoldProducts")]
        public async Task<IActionResult> GetTopSoldProducts()
        {
           
                
            var result = await _reportRepository.GetTopProducts();
            return Ok(result);
        }

        [HttpGet("/report/topCategories")]
        public async Task<IActionResult> GetTopCategories()
        {
           

            var result = await _reportRepository.GetTopCategories();
            return Ok(result);
        }

        [HttpGet("/report/topReviewProducts")]
        public async Task<IActionResult> GetTopRatingProduct()
        {
            var result = await _reportRepository.GetTopReviewProducts();
            return Ok(result);
        }

    }
}
