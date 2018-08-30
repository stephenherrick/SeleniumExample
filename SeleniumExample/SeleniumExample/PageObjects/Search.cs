using Framework.WebDriver;
using OpenQA.Selenium;

namespace SeleniumExample.PageObjects
{
    public class Search : IPageObject
    {
        private DriverFactory _driver;

        private IWebElement SearchTextField
        {
            get
            {
                return _driver.Instance.FindElement(By.Id("search_form_input_homepage"), _driver.GlobalWaitTime);
            }
        }

        private IWebElement SearchButton
        {
            get
            {
                return _driver.Instance.FindElement(By.Id("search_button_homepage"), _driver.GlobalWaitTime);
            }
        }

        public string Url
        {
            get
            {
                return "/";
            }
        }

        public Search(DriverFactory driver)
        {
            _driver = driver;
        }

        public Search EnterTextInSearchField(string inputText)
        {
            SearchTextField.SendKeys(inputText);
            return this;
        }

        public Search ClickSearchButton()
        {
            SearchButton.Click();
            return this;
        }

        public void NavigateToPage()
        {
            _driver.Instance.Url = _driver.BaseUrl + Url;
            WaitForPageToLoad();
        }

        public void WaitForPageToLoad()
        {
            _driver.Instance.WaitFor(SearchTextField, _driver.GlobalWaitTime);
        }
    }
}
