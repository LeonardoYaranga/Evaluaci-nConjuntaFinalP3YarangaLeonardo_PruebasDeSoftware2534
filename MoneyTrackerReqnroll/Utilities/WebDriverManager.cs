using OpenQA.Selenium;
using System;

namespace MoneyTrackerReqnroll.Utilities
{
    public class WebDriverManager
    {
        private static IWebDriver _driver;

        public static IWebDriver GetDriver(string browser)
        {
            if (_driver == null)
            {
                _driver = browser.ToLower() switch
                {
                    "chrome" => new OpenQA.Selenium.Chrome.ChromeDriver(),
                    "firefox" => new OpenQA.Selenium.Firefox.FirefoxDriver(),
                    "edge" => new OpenQA.Selenium.Edge.EdgeDriver(),
                    _ => throw new NotSupportedException($"El navegador {browser} no es soportado.")
                };
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}