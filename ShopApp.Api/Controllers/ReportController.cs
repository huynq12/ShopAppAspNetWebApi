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
        public async Task<IActionResult> GetTopSoldProducts(int num)
        {
            if (num == 0)
                return BadRequest();
                
            var result = await _reportRepository.GetTopProducts(num);
            return Ok(result);
        }

        [HttpGet("/report/topCategories")]
        public async Task<IActionResult> GetTopCategories(int num)
        {
            if (num == 0)
                return BadRequest();

            var result = await _reportRepository.GetTopCategories(num);
            return Ok(result);
        }

        [HttpGet("/report/topReviewProducts")]
        public async Task<IActionResult> GetTopRatingProduct(int num)
        {
            if (num == 0)
                return BadRequest();

            var result = await _reportRepository.GetTopReviewProducts(num);
            return Ok(result);
        }

    }
}
