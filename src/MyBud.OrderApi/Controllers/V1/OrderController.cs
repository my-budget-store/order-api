using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MyBud.OrdersApi.Interfaces;
using MyBud.OrdersApi.Models.Request;
using System.Net;
using System.Security.Claims;

namespace MyBud.OrdersApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly Guid _userId;

        public OrderController(IOrderRepository OrderRepository,
            IHttpContextAccessor httpContext)
        {
            _OrderRepository = OrderRepository;
            _httpContext = httpContext;
            _userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        /// <summary>
        /// Get list of all Order
        /// </summary>
        /// <returns>List of all Orders</returns>
        [ProducesResponseType(typeof(IEnumerable<Models.Core.OrderEntity>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {

            var Orders = await _OrderRepository.GetOrders();

            return Ok(Orders);
        }

        /// <summary>
        /// Get details of a specific Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Details of a specific Order</returns>
        [ProducesResponseType(typeof(Models.Core.OrderEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var product = await _OrderRepository.GetOrderById(id);

            return product != null
                ? Ok(product)
                : NotFound();
        }

        /// <summary>
        /// Search Orders
        /// </summary>
        /// <param name="value"></param>
        /// <returns>List of matching Order</returns>
        [ProducesResponseType(typeof(IEnumerable<Models.Core.OrderEntity>), (int)HttpStatusCode.OK)]
        [HttpGet("Search")]
        public async Task<IActionResult> SearchOrders(int year)
        {
            var Order = await _OrderRepository.SearchOrders(year);

            return Order != null
                ? Ok(Order)
                : NotFound();
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="Order"></param>
        /// <returns>Result of Order creation</returns>
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder(List<OrderItem> cartItems)
        {
            //TODO: validate model
            var createdOrder = await _OrderRepository.CreateOrder(cartItems, _userId);

            return createdOrder != null
                ? Created(new Uri(Request.GetEncodedUrl()), true)
                : BadRequest();
        }

        ///// <summary>
        ///// Update specific Order
        ///// </summary>
        ///// <param name="Order"></param>
        ///// <returns>Updated Order</returns>
        //[HttpPut]
        //[Authorize]
        //[ProducesResponseType(typeof(OrderEntity), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> UpdateOrder(OrderEntity Order)
        //{
        //    //ToDo: Validate input model
        //    var isExistingOrder = await _OrderRepository.GetOrderById(Order.OrderId) != null;

        //    if (isExistingOrder)
        //    {
        //        var updatedOrder = await _OrderRepository.UpdateOrder(Order);

        //        return Ok(updatedOrder);
        //    }

        //    return NotFound(Order);
        //}

        ///// <summary>
        ///// Delete specific Order
        ///// </summary>
        ///// <param name="OrderId"></param>
        ///// <returns>Result of Order deletion</returns>
        //[ProducesResponseType(typeof(OrderEntity), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[HttpDelete("{id}")]
        //[Authorize]
        //public async Task<IActionResult> DeleteOrder(int OrderId)
        //{
        //    var Order = await _OrderRepository.GetOrderById(OrderId);
        //    if (Order == null)
        //    {
        //        return NotFound();
        //    }

        //    var isDeleted = await _OrderRepository.DeleteOrder(Order);

        //    return isDeleted
        //        ? Ok()
        //        : NotFound();
        //}
    }
}