using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenQA.Selenium
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Clears text field and then sends keys to a text field.
        /// </summary>
        /// <param name="input">Text to insert into field</param>
        /// <param name="clearField">If true, the field is cleared before sending text, else the existing text remains</param>
        public static void SendKeys(this IWebElement element, string input, bool clearField = false)
        {
            if (clearField)
            {
                element.Clear();
                //This section was added because occasionally the clear() method does not always clear the field.
                if (!String.IsNullOrEmpty(element.Text))
                {
                    element.SendKeys(Keys.Control + "a" + Keys.Control + input);
                }
                else
                    element.SendKeys(input);
            }
            else if (!clearField)
                element.SendKeys(input);
        }

        /// <summary>
        /// A fluent wait wrapper for FindElement. Waits for a given amount of time for an element to display before locating.
        /// </summary>
        /// <param name="by"><see cref="By"/></param>
        /// <exception cref="NoSuchElementException"
        /// <returns>Single element<see cref="IWebElement"/></returns>
        public static IWebElement FindElement(this IWebDriver driver, By by, TimeSpan waitTime)
        {
            try
            {
                Console.WriteLine(String.Format("Finding element at: {0}", by.ToString()));
                driver.WaitFor(by, waitTime);
            }
            catch (Exception e)
            {
                if (e is NoSuchElementException || e is StaleElementReferenceException)
                {
                    Console.WriteLine("{0}, Element not found at: {1}", e.Message, by.ToString());
                }
                throw e;
            }
            return driver.FindElement(by);
        }

        /// <summary>
        ///  A fluent wait wrapper for FindElements. Waits for a given amount of time for an element to display before locating.
        /// </summary>
        /// <param name="driver"><see cref="IWebDriver"/></param>
        /// <param name="by"><see cref="By"/></param>
        /// <returns>List of Elements<seealso cref="IWebElement"/></returns>
        public static IList<IWebElement> FindElements(this IWebDriver driver, By by, TimeSpan waitTime)
        {
            try
            {
                Console.WriteLine(String.Format("Finding elements at: {0}", by.ToString()));
                driver.WaitFor(by, waitTime);
            }
            catch (Exception e)
            {
                if (e is NoSuchElementException || e is StaleElementReferenceException || e is WebDriverTimeoutException)
                {
                    Console.WriteLine("{0}, Elements not found at: {1}", e.Message, by.ToString());
                }
                throw e;
            }
            return driver.FindElements(by).Where(e => e.Displayed == true).ToList();
        }

        /// <summary>
        /// Wait until a specified element is visible.
        /// </summary>
        /// <param name="locator">The locator of the element<see cref="By"/></param>
        public static void WaitFor(this IWebDriver driver, By locator, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            IWebElement element;
            wait.Until((d) =>
            {
                element = d.FindElement(locator);
                if (element.Displayed)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for a specific element. This waits until a specified element is visible.
        /// </summary>
        /// <param name="element"><see cref="IWebElement"/></param>
        public static void WaitFor(this IWebDriver driver, IWebElement element, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            wait.Until((d) =>
            {
                if (element.Displayed &&
                    element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for a specific element is not visible.
        /// </summary>
        /// <param name="locator"><see cref="By"/></param>
        public static void WaitForElementIsInvisible(this IWebDriver driver, By locator, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            wait.Until((d) =>
            {
                IWebElement element = d.FindElement(locator);
                if (!element.Displayed ||
                    !element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for an element to not be visisble or disabled.
        /// </summary>
        /// <param name="element"><see cref="IWebElement"/> </param>
        public static void WaitForElementIsInvisible(this IWebDriver driver, IWebElement element, TimeSpan waitTime)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = waitTime;
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement searchResult = fluentWait.Until((d) =>
            {
                if (!element.Displayed || !element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Call to 'halt execution' and wait a specific amount of time.
        /// <seealso cref="Thread.Sleep(int)"/>
        /// </summary>
        /// <param name="waitInSeconds">Number of seconds to wait</param>
        public static void ExplicitWait(this IWebDriver driver, TimeSpan waitTime)
        {
            Thread.Sleep(waitTime);
        }
    }
}
