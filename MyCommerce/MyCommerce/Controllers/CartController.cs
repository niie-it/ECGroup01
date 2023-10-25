using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
	public class CartController : Controller
	{
		const string KeyCartName = "CART";
		private readonly MyeStoreContext _ctx;
		public CartController(MyeStoreContext ctx)
		{
			_ctx = ctx;
		}

		// Hiển thị giỏ hàng
		public IActionResult Index()
		{
			return View(Carts);
		}

		public List<CartItem> Carts
		{
			get
			{
				//Lấy từ session ra
				var gh = HttpContext.Session.Get<List<CartItem>>(KeyCartName);
				if (gh == null)
				{
					gh = new List<CartItem>();
				}
				return gh;
			}
		}
		public IActionResult AddToCart(int id, int qty = 1)
		{
			var gioHang = Carts;

			//kiểm tra id (MaHH) truyền qua đã nằm trong giỏ hàng hay chưa
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null) //đã có
			{
				item.SoLuong += qty;
			}
			else
			{
				var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
				if (hangHoa == null)//id không có trong Database
				{
					return RedirectToAction("Index", "HangHoas");
				}
				item = new CartItem
				{
					MaHh = id,
					SoLuong = qty,
					TenHh = hangHoa.TenHh,
					Hinh = hangHoa.Hinh,
					DonGia = hangHoa.DonGia.Value
				};
				//thêm vào giỏ hàng
				gioHang.Add(item);
			}
			//cập nhật session
			HttpContext.Session.Set(KeyCartName, gioHang);
			return RedirectToAction("Index");
		}
	}
}
