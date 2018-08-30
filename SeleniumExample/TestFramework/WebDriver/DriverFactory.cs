using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestFramework.WebDriver
{
    public class DriverFactory
    {
        /// <summary>
        /// An instance of a WebDriver.  Call .initialize first to set the instance.
        /// <see cref="IWebDriver"/>
        /// </summary>
        public IWebDriver Instance { get; set; }

        /// <summary>
        /// This is the Base URL of the application, typically set in the app.config file.
        /// </summary>
        public string BaseUrl { get; set; }

        private string _baseApplicationPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// General time span used for Implicit and Fluent waits
        /// </summary>
        public TimeSpan GlobalWaitTime { get; set; }

        public DriverFactory(string browserName, string baseUrl, int waitTIme = 30)
        {
            GlobalWaitTime = TimeSpan.FromSeconds(waitTIme);
            Instance = SetBrowser(browserName);
            BaseUrl = baseUrl;

        }

        /// <summary>
        /// Starts WebDriver, adds implicit wait of 5 seconds, and then maximizes the window
        /// </summary>
        /// <param name="baseUrl"><see cref="BaseUrl"/></param>
        /// <param name="browserName"><see cref="SetBrowser(string)"/></param>
        /// <param name="waitTIme"><see cref="GlobalWaitTime"/></param>
        public void Initialize()
        {

            Instance.Manage().Timeouts().ImplicitWait = GlobalWaitTime;
            try
            {
                Instance.Manage().Window.Maximize();
            }
            catch (Exception) { }

            Instance.Navigate().GoToUrl(BaseUrl);
        }

        /// <summary>
        /// Closes the WebDriver instance
        /// </summary>
        public void Close()
        {
            Instance.Close();
            Instance.Quit();
        }

        /// <summary>
        /// Closes all browsers and driver instances.
        /// </summary>
        public void KillAll()
        {
            var processes = new List<string> { "chromedriver", "geckodriver", "IEDriverServer", "chrome", "firefox", "iexplorer" };
            foreach (string app in processes)
            {
                foreach (var process in Process.GetProcessesByName(app))
                {
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// Set the WebDriver browser by browser name.
        /// </summary>
        /// <param name="browserName">Options not case sensitive and are: "chrome", "firefox", "chromeheadless", "firefoxheadless", "ie"</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Returns a the appropriate WebDriver by browser name</returns>
        private IWebDriver SetBrowser(string browserName)
        {
            switch (browserName.ToLower())
            {
                case "firefox":
                    FirefoxDriverService fireservice = FirefoxDriverService.CreateDefaultService(_baseApplicationPath, "geckodriver.exe");
                    fireservice.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    var fireoptions = new FirefoxOptions();
                    //fireoptions.SetPreference("network.automatic-ntlm-auth.trusted-uris", "test.com");
                    return new FirefoxDriver(fireservice, fireoptions, GlobalWaitTime);

                case "firefoxheadless":
                    FirefoxDriverService firehservice = FirefoxDriverService.CreateDefaultService(_baseApplicationPath, "geckodriver.exe");
                    firehservice.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    var firehoptions = new FirefoxOptions();
                    //firehoptions.SetPreference("network.automatic-ntlm-auth.trusted-uris", "test.com");
                    firehoptions.AddArgument("--headless");
                    return new FirefoxDriver(firehservice, firehoptions, GlobalWaitTime);

                case "ie":
                    InternetExplorerDriverService ieservice = InternetExplorerDriverService.CreateDefaultService(_baseApplicationPath);
                    var ieoptions = new InternetExplorerOptions()
                    {
                        IgnoreZoomLevel = true,
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true
                    };
                    //ieoptions.AddAdditionalCapability("network.automatic-ntlm-auth.trusted-uris", "test.com");
                    return new InternetExplorerDriver(ieservice, ieoptions, GlobalWaitTime);

                case "chrome":
                    ChromeOptions coptions = new ChromeOptions();
                    //coptions.AddArgument("-auth-server-whitelist=\"test.com\"" +
                    //    "–auth-negotiate-delegatewhitelist=\"test.com\"" +
                    //    "-auth-schemes=\"digest,ntlm,negotiate\"");
                    return new ChromeDriver(_baseApplicationPath, coptions, GlobalWaitTime);

                case "chromeheadless":
                    ChromeOptions choptions = new ChromeOptions();
                    choptions.AddArgument("--headless --disable-gpu"/* +
                        "-auth-server-whitelist=\"test.com\"" +
                        "-auth-negotiate-delegatewhitelist=\"test.com\"" +
                        "-auth-schemes=\"digest,ntlm,negotiate\""*/);
                    return new ChromeDriver(_baseApplicationPath, choptions, GlobalWaitTime);

                default:
                    throw new ArgumentException("Provide a valid browser name.");
            }
        }
    }
}
