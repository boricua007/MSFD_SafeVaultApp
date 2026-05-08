using Microsoft.AspNetCore.Mvc;
using MSFD_SafeVault.Controllers;
using MSFD_SafeVault.Models;

namespace MSFD_SafeVault.Tests;

public class AccountControllerTests
{
    [Test]
    public async Task Login_InvalidCredentials_ReturnsViewWithError()
    {
        var controller = new AccountController();
        var model = new LoginModel
        {
            Username = "admin",
            Password = "wrongpassword"
        };

        var result = await controller.Login(model);

        Assert.That(result is ViewResult);
        Assert.That(controller.ModelState[string.Empty]?.Errors.Count == 1);
        Assert.That(controller.ModelState[string.Empty]?.Errors[0].ErrorMessage == "Invalid username or password.");
    }
}