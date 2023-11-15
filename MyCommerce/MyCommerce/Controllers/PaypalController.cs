using Microsoft.AspNetCore.Mvc;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
	public class PaypalController : Controller
	{
		private readonly PaypalClient _paypalClient;

		public PaypalController(PaypalClient paypalClient)
		{
			_paypalClient = paypalClient;
		}

		public IActionResult Index()
		{
			// ViewBag.ClientId is used to get the Paypal Checkout javascript SDK
			ViewBag.ClientId = _paypalClient.ClientId;

			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Order(CancellationToken cancellationToken)
		{
			// Tạo đơn hàng (thông tin lấy từ Session???)
			var tongTien = "1009.0";
			var donViTienTe = "USD";
			// OrderId - mã tham chiếu (duy nhất)
			var orderIdref = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				// a. Create paypal order
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, orderIdref);

				return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
		}

        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key
                // Lưu đơn hàng vô database

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
