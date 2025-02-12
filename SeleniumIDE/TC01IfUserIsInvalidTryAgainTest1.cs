using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.IO;

[TestFixture]
public class TC01IfUserIsInvalidTryAgainTest : IDisposable
{
    private IWebDriver driver;
    public IDictionary<string, object> vars { get; private set; }
    private IJavaScriptExecutor js;

    [SetUp]
    public void SetUp()
    {
        // Set up ChromeOptions to run headless in CI
        ChromeOptions options = new ChromeOptions();
        options.AddArguments("--headless");

        // Create a unique user data directory path for ChromeDriver
        string userDataDir = Path.Combine(Directory.GetCurrentDirectory(), "chrome_user_data");
        options.AddArguments($"--user-data-dir={userDataDir}");

        // Pass options to ChromeDriver
        driver = new ChromeDriver(options);
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }

    [TearDown]
    public void TearDown()
    {
        Dispose();
    }

    public void Dispose()
    {
        // Dispose WebDriver correctly
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();
            driver = null;
        }
    }

    [Test]
    public void tC01IfUserIsInvalidTryAgain()
    {
        // Test name: TC01 - If User Is Invalid Try Again
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("user123");
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"login-password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

        vars["errorMessage"] = driver.FindElement(By.CssSelector("*[data-test=\"error\"]")).Text;

        if ((bool)js.ExecuteScript("return arguments[0] === 'Epic sadface: Username and password do not match any user in this service'", vars["errorMessage"]))
        {
            Console.WriteLine("Wrong username");
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Clear();
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

            Assert.That(driver.FindElement(By.CssSelector("*[data-test=\"title\"]")).Text, Is.EqualTo("Products"));
            Console.WriteLine("Successful login");
        }
    }
}
