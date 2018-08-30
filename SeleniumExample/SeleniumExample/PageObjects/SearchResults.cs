using Framework.WebDriver;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace SeleniumExample.PageObjects
{
    public class SearchResults : IPageObject
    {
        private DriverFactory _driver;

        private IList<IWebElement> ResultItems
        {
            get
            {
                var results = _driver.Instance.FindElements(By.XPath("//*[@class='result__a']"), _driver.GlobalWaitTime);
                return results;
            }
            
        }

        private IList<string> ResultUrls
        {
            get
            {
                var urls = ResultItems.Select(e => e.GetAttribute("href").ToString());
                return urls.ToList();
            }
        }

        public string Url
        {
            get
            {
                return "/q";
            }
        }
                

        public SearchResults(DriverFactory driver)
        {
            _driver = driver;
        }

        public IList<string> GetVisibleResults()
        {
            return ResultUrls;
        }

        public void NavigateToPage()
        {
            _driver.Instance.Url = _driver.BaseUrl + Url;
            WaitForPageToLoad();
        }

        public void WaitForPageToLoad()
        {
            _driver.Instance.WaitFor(ResultItems[0], _driver.GlobalWaitTime);
        }
    }
}
