using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.IdentityJwtRefreshExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// Метод получения списка заказов.
        /// Доступ для ролей "User".
        /// </summary>
        /// <returns>Список заказов.</returns>
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public IActionResult GetOrders()
        {
            List<string> orders = ["order1", "order2", "order3"];

            return Ok(orders);
        }
    }
}
