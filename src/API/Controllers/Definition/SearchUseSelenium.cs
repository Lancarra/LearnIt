using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace API.Controllers.Definition;

public static class SearchUseSelenium
{
    public async static Task<string> Search(string word)
    {
        var options = new ChromeOptions();
        /*
        options.AddArgument("--headless"); 
        */
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        
        string imageUrl = string.Empty;

        await Task.Run(() =>
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl($"https://vikoeif.edupage.org/timetable/");
                driver.Navigate().GoToUrl($"https://www.google.com/search?q=test&sca_esv=5a4b1ba8a892269d&rlz=1C1KNTJ_ruLT1052LT1052&udm=2&biw=1536&bih=695&sxsrf=AE3TifM-ON5_UZLoXN92iCyQuaSdEu_yZg%3A1752247115685&ei=SytxaKvHKeLJwPAPgvHqyAM&oq=&gs_lp=EgNpbWciACoCCAAyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gIyChAjGCcYyQIY6gJInhRQAFgAcAF4AJABAJgBAKABAKoBALgBAcgBAPgBAZgCAaACBagCCpgDBZIHATGgBwCyBwC4BwDCBwMyLTHIBwQ&sclient=img");
                try
                {
                    var acceptCookies = wait.Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//button/div[normalize-space()='Принять все']")));
                    acceptCookies.Click();
                }
                catch (WebDriverTimeoutException)
                {
                    // окно не появилось, значит, можно идти дальше
                }
                driver.Manage().Window.Maximize();
                
                var googleSearch = driver.FindElement(By.Name("q"));
                googleSearch.SendKeys(word);
                googleSearch.SendKeys(Keys.Enter);
                
                var imagesTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(@href, 'tbm=isch')]")));
                imagesTab.Click();
                
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("img")));
                var images = driver.FindElements(By.CssSelector("img"));
                if (images.Count > 3)
                {
                    var src = images[3].GetAttribute("src");
                    if (!string.IsNullOrEmpty(src) && src.StartsWith("http"))
                    {
                        imageUrl = src;
                    }
                }

                driver.Quit();
            }
        });
        return imageUrl;//TODO
    }
    
}