namespace SeleniumExample.PageObjects
{
    public interface IPageObject
    {
        string Url { get; }

        void NavigateToPage();

        void WaitForPageToLoad();

    }
}
