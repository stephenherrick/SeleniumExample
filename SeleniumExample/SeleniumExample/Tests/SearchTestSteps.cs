using Framework.WebDriver;
using NUnit.Framework;
using SeleniumExample.PageObjects;
using System;
using System.Configuration;
using TechTalk.SpecFlow;

namespace SeleniumExample
{
    [Binding]
    public class SearchTestSteps
    {
        private static DriverFactory _driver;
        private static Search _search;
        private static SearchResults _searchResults;

        [BeforeTestRun]
        public static void StartUp()
        {
            _driver = new DriverFactory("Chrome", ConfigurationManager.AppSettings["BaseUrl"], 7);
            _driver.Initialize();
            
        }

        [Given(@"I have entered (.*) in the search text box")]
        public void GivenIHaveEnteredTextInTheSearchTextBox(string searchTerm)
        {
            _search = new Search(_driver);
            _search.WaitForPageToLoad();
            _search.EnterTextInSearchField(searchTerm);
        }

        [When(@"I click the search button")]
        public void WhenIClickTheSearchButton()
        {
            _search.ClickSearchButton();
        }

        [Then(@"search results display")]
        public void ThenSearchResultsDisplay()
        {
            _searchResults = new SearchResults(_driver);
            _searchResults.WaitForPageToLoad();
             foreach(var result in _searchResults.GetVisibleResults())
            {
                Console.WriteLine(result);
            }

            Assert.That(_searchResults.GetVisibleResults().Count, Is.GreaterThan(0));
        }

        [After]
        public void Reset()
        {
            _search.NavigateToPage();
            _driver.Instance.Navigate().Refresh();
        }

        [AfterTestRun]
        public static void TearDown()
        {
            _driver.Close();
            _driver.KillAll();
        }
    }
}
