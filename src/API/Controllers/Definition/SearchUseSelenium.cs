using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace API.Controllers.Definition;

public static class SearchUseSelenium
{
    public async static Task<string> Search(string word)
    {
        string imageUrl = string.Empty;

        await Task.Run(() =>
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                
                driver.Navigate().GoToUrl("https://vikoeif.edupage.org/timetable/");
                driver.Navigate().GoToUrl("https://rozetka.com.ua");
                driver.Navigate().GoToUrl("https://djinni.co/my/dashboard/");
                driver.Navigate().GoToUrl("https://www.google.com/imghp");
                
                try
                {
                    var acceptCookies = driver.FindElements(By.XPath("//button/div[text()='Принять все']"));
                    if (acceptCookies.Count > 0)
                    {
                        acceptCookies[0].Click();
                    }
                }
                catch {}

                driver.Manage().Window.Maximize();
                
                var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("q")));
                searchBox.Clear();
                searchBox.SendKeys(word);
                searchBox.SendKeys(Keys.Enter);

                Thread.Sleep(2000);

                /*var thumbnails = wait.Until(d =>
                {
                    var imgs = d.FindElements(By.XPath("//*[@id='dimg_PcN3aLSELfTUwPAPs-LNiQI_9']"));
                    return imgs.Count > 0 ? imgs : null;
                });

                if (thumbnails.Count == 0)
                {
                    Console.WriteLine("No images found.");
                    driver.Quit();
                    return;
                }

                (thumbnails.Count > 4 ? thumbnails[4] : thumbnails[0]).Click();
                */
                var imgs = driver.FindElements(By.XPath("/html/body/div[3]/div/div[14]/div/div[2]/div[2]/div/div/div/div/div[1]/div/div/div[6]/div[2]/h3/a/div/div/div/g-img/img"));
                foreach (var item in imgs)
                {
                    if (item != null)
                    {
                        item.Click();
                    }
                }
                var fullImage = wait.Until(d =>
                {
                    var imgs = d.FindElements(By.CssSelector("img.sFlh5c.FyHeAf[src^='http']"));
                    return imgs.FirstOrDefault(img =>
                        img.Displayed &&
                        !img.GetAttribute("src").Contains("encrypted-tbn"));
                });

                if (fullImage != null)
                {
                    imageUrl = fullImage.GetAttribute("src");
                    Console.WriteLine("Image found: " + imageUrl);
                }

                driver.Quit();
            }
        });

        return imageUrl;
    }
}