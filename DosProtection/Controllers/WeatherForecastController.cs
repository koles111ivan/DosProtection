using Microsoft.AspNetCore.Mvc;

namespace DosProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Успешный запрос!",
                time = DateTime.Now.ToString("HH:mm:ss"),
                instructions = "Делайте более 10 запросов за 30 секунд для теста защиты"
            });
        }
    }
}