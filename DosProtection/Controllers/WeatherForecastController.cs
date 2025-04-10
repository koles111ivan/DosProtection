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
                message = "�������� ������!",
                time = DateTime.Now.ToString("HH:mm:ss"),
                instructions = "������� ����� 10 �������� �� 30 ������ ��� ����� ������"
            });
        }
    }
}