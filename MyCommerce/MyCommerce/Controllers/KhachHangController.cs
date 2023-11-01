using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
	public class KhachHangController : Controller
	{
		private readonly MyeStoreContext _ctx;

		public KhachHangController(MyeStoreContext ctx)
		{
			_ctx = ctx;
		}

		public IActionResult DangKy()
		{
			return View();
		}

		[HttpPost]
		public IActionResult DangKy(RegisterVM model, IFormFile FileHinh)
		{
			try
			{
				var khachHang = new KhachHang
				{
					MaKh = model.MaKh,
					HoTen = model.HoTen,
					NgaySinh = model.NgaySinh,
					DiaChi = model.DiaChi,
					GioiTinh = model.GioiTinh,
					DienThoai = model.DienThoai,
					Email = model.Email,
					Hinh = MyTool.UploadFileToFolder(FileHinh, "KhachHang"),

					HieuLuc = true, //false + gửi mail active tài khoản
					RandomKey = MyTool.GetRandom()
				};
				khachHang.MatKhau = model.MatKhau.ToSHA512Hash(khachHang.RandomKey);
				_ctx.Add(khachHang);
				_ctx.SaveChanges();
				return RedirectToAction("DangNhap");
			}
			catch (Exception ex)
			{
				return View();
			}
		}
	}
}
