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
        // Set up ChromeOptions to run headless in CI (Jenkins)
        ChromeOptions options = new ChromeOptions();
        options.AddArguments("--headless", "--disable-gpu", "--no-sandbox", "--disable-dev-shm-usage");

        // Optionally, set the path to your ChromeDriver
        // string chromedriverPath = "/path/to/chromedriver";
        // driver = new ChromeDriver(chromedriverPath, options);

        // Initialize the ChromeDriver with the provided options
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
        // Dispose of WebDriver correctly after each test
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
        // Navigate to the test URL
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        // Set window size to ensure visibility of elements
        driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);

        // Input invalid username and password
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("user123");
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"login-password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).SendKeys("secret_sauce");
        driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

        // Capture error message
        vars["errorMessage"] = driver.FindElement(By.CssSelector("*[data-test=\"error\"]")).Text;

        // Check if the error message matches the expected message for wrong credentials
        if ((bool)js.ExecuteScript("return arguments[0] === 'Epic sadface: Username and password do not match any user in this service'", vars["errorMessage"]))
        {
            Console.WriteLine("Wrong username, trying again...");

            // Clear username field and input correct username for login
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Clear();
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

            // Verify successful login by checking the page title
            Assert.That(driver.FindElement(By.CssSelector("*[data-test=\"title\"]")).Text, Is.EqualTo("Products"));
            Console.WriteLine("Successful login");
        }
    }
}
