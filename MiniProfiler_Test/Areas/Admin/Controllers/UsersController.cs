using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Controllers;
using MiniProfiler_Test.Interfaces.Api;
using MiniProfiler_Test.Models.Api.Users;

namespace MiniProfiler_Test.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class UsersController : SiteBaseController
{
    #region Constractor

    private readonly IApiUserService _userService;

    public UsersController(IApiUserService userService)
    {
        _userService = userService;
    }

    #endregion

    #region Get

    [HttpGet]
    public async Task<IActionResult> Index(string username = "", string firstname = "", string lastname = "",
        int pageNumber = 1, string? startDate = null, string? endDate = null)
    {
        try
        {
            var result = await _userService.GetAllUsersAsync(username, firstname, lastname, "createdat", pageNumber, 5,
                startDate, endDate);
            if (result.Success)
            {
                return View(result);
            }

            TempData[ErrorMessage] = result.Message;
            return View();
        }
        catch
        {
            TempData[ErrorMessage] = "خطا در اتصال";
            return View();
        }
    }

    #endregion

    #region Create

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var createdUser = await _userService.CreateUserAsync(model);

            if (!createdUser.Success)
            {
                TempData[ErrorMessage] = createdUser.Message;
                return RedirectToAction("Index");
            }

            TempData[SuccessMessage] = "کاربر با موفقیت ایجاد شد.";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData[ErrorMessage] = "خطا در اتصال";
            return RedirectToAction("Index");
        }
    }

    #endregion

    #region Delete

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData[ErrorMessage] = "شناسه کاربر نامعتبر است.";
                return RedirectToAction(nameof(Index));
            }

            var userId = Convert.ToInt32(id);

            var result = await _userService.DeleteUserAsync(userId);
            if (!result.Success)
                return RedirectToAction("Index");

            TempData["SuccessMessage"] = "کاربر با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData[ErrorMessage] = "خطا در اتصال";
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}