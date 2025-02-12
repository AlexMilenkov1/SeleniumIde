using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

[TestFixture]
public class TC01IfUserIsInvalidTryAgainTest
{
    private IWebDriver driver;
    private ChromeOptions options;
    private IJavaScriptExecutor js;
    private IDictionary<string, object> vars;

    [SetUp]
    public void SetUp()
    {
        // Initialize ChromeOptions
        options = new ChromeOptions();
        options.AddArgument("--headless");  // Run Chrome in headless mode
        options.AddArgument("--no-sandbox");  // Bypass sandboxing (required for Docker)
        options.AddArgument("--disable-dev-shm-usage");  // Disable dev shm usage to avoid crashes

        // Initialize WebDriver with options
        driver = new ChromeDriver(options);

        // Initialize the JavaScriptExecutor and other variables
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<string, object>();
    }

    [TearDown]
    protected void TearDown()
    {
        // Clean up and quit the driver
        driver.Quit();
    }

    [Test]
    public void tC01IfUserIsInvalidTryAgain()
    {
        // Test name: TC01 - If User Is Invalid Try Again

        // Step 1: Navigate to the URL
        driver.Navigate().GoToUrl("https://www.saucedemo.com/");

        // Step 2: Set window size
        driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);

        // Step 3: Click the username field
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Click();

        // Step 4: Type invalid username
        driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("user123");

        // Step 5-8: Click the password field and type password
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).Click();
        driver.FindElement(By.CssSelector("*[data-test=\"password\"]")).SendKeys("secret_sauce");

        // Step 9: Click the login button
        driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

        // Step 10: Get the error message if exists
        vars["errorMessage"] = driver.FindElement(By.CssSelector("*[data-test=\"error\"]")).Text;

        // Step 11: Check if the error message matches the expected message
        if (vars["errorMessage"]?.ToString() == "Epic sadface: Username and password do not match any user in this service")
        {
            // Step 12: Print a message if the login fails
            Console.WriteLine("Wrong username");

            // Step 13-15: Clear the username field, type the correct username, and click the login button
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).Clear();
            driver.FindElement(By.CssSelector("*[data-test=\"username\"]")).SendKeys("standard_user");
            driver.FindElement(By.CssSelector("*[data-test=\"login-button\"]")).Click();

            // Step 16: Assert that the page title is "Products"
            Assert.That(driver.FindElement(By.CssSelector("*[data-test=\"title\"]")).Text, Is.EqualTo("Products"));

            // Step 17: Print a success message
            Console.WriteLine("Successful login");
        }

        // Step 19: Close the browser
        driver.Quit();  // Use Quit instead of Close to ensure the entire session is terminated
    }
}
