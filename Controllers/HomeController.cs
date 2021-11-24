using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PrivacyAsync()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {Headless = false, SlowMo = 50 });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = @"C:\Users\cashless\Desktop\screenshot.png" });
            return Content("Hello World");
        }

        public IActionResult MyLogin()
        {
            return View();
        }

        public async Task<IActionResult> SubmitAsync(TestViewModel viewModel)
        {
            string username = viewModel.UserName;
            string password = viewModel.Password;
            string FirstName = viewModel.FirstName;
            string UserPassportNumber = viewModel.PassportNumber;
            string date = viewModel.Date;
            string applicationNumber = "7005491964";
            string sponserId = "7000874060";
            if (date == null)
            {
                date = DateTime.Now.Date.ToString();
            }
            List<Travellers> Travellers = new List<Travellers>();
            Travellers CurrentTraveller = new Travellers();
            List<string> dirs = new List<string>(Directory.GetFiles("./Images"));
            var filePath = Path.GetFullPath(dirs[0]);
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 50});
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            // Netflix Test
            /*await page.GotoAsync("https://netflix.com");
            await page.ClickAsync("text= Sign In");
            await page.FillAsync("input[name='userLoginId']", username);
            await page.FillAsync("input[name='password']", password);
            await page.ClickAsync("button[data-uia='login-submit-button']");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = @"C:\Users\cashless\Desktop\screenshot.png" });*/
            //Working with Mofa
            await page.GotoAsync("https://visa.mofa.gov.sa/");
            var ButtonSelector = "#dlgAlert > div.modal-dialog > div > div#dlgMessageContent > div.modal-footer > button";
            await page.ClickAsync(ButtonSelector);
            var SearchOptions = await page.QuerySelectorAsync("#SearchingType");
            await SearchOptions.SelectOptionAsync("2");
            await page.FillAsync("input[id='ApplicationNumber']", applicationNumber);
            await page.FillAsync("input[id='SponserID']", sponserId);
            await page.ClickAsync("input[id='Captcha']");
            await page.WaitForSelectorAsync("#content");
            // application type getter
            var applicationTypeSelector = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(1) > div:nth-child(2) > h2";
            var text1 = await page.TextContentAsync(applicationTypeSelector);
            // first row of form
            var RakamMostanad = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(4) > div:nth-child(2) > label";
            var RakamMostanadText = await page.TextContentAsync(RakamMostanad);
            var TarekhMostanad = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(4) > div:nth-child(4) > label";
            var TarekhMostanadText = await page.TextContentAsync(TarekhMostanad);
            // second row of from
            var EsmGehaTaleba = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(5) > div:nth-child(2) > label";
            var EsmGehaTalebaText = await page.TextContentAsync(EsmGehaTaleba);
            var RakamSegel = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(5) > div:nth-child(4) > label";
            var RakamSegelText = await page.TextContentAsync(RakamSegel);
            // third row of form
            var RakamGawal = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(6) > div > label";
            var RakamGawalText = await page.TextContentAsync(RakamGawal);
            // fourth row of form
            var Address = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(7) > div:nth-child(2) > label";
            var AddressText = await page.TextContentAsync(Address);
            var PhoneNumber = "#content > div > div.row > div > div > div.portlet-body.form > div.form-body.form-display.form-horizontal.page-print > div:nth-child(7) > div:nth-child(4) > label";
            var PhoneNumberText = await page.TextContentAsync(PhoneNumber);
            // Getting users in table data
            var tableSelector = "#tblDocumentVisaList > tbody > tr.jtable-data-row";
            await page.WaitForSelectorAsync(tableSelector);
            var allTravellers = await page.QuerySelectorAllAsync(tableSelector);
            foreach(var traveller in allTravellers)
            {
                var VisaType = await traveller.QuerySelectorAsync("td:nth-child(1)");
                var VisaTypeText = VisaType.TextContentAsync();

                var PassportNumber = await traveller.QuerySelectorAsync("td:nth-child(2)");
                var PassportNumberText = PassportNumber.TextContentAsync();

                var Name = await traveller.QuerySelectorAsync("td:nth-child(4)");
                var NameText = Name.TextContentAsync();

                var Destination = await traveller.QuerySelectorAsync("td:nth-child(5)");
                var DestinationText = Destination.TextContentAsync();

                var Gender = await traveller.QuerySelectorAsync("td:nth-child(6)");
                var GenderText = Gender.TextContentAsync();

                var Nationality = await traveller.QuerySelectorAsync("td:nth-child(7)");
                var NationalityText = Nationality.TextContentAsync();

                var Job = await traveller.QuerySelectorAsync("td:nth-child(8)");
                var JobText = Job.TextContentAsync();

                var NumberOfEntryTimes = await traveller.QuerySelectorAsync("td:nth-child(9)");
                var NumberOfEntryTimesText = NumberOfEntryTimes.TextContentAsync();

                var LengthOfStayInDays = await traveller.QuerySelectorAsync("td:nth-child(10)");
                var LengthOfStayInDaysText = LengthOfStayInDays.TextContentAsync();
                Travellers traveller1 = new Travellers {
                    VisaType = VisaTypeText.Result, 
                    PassportNumber = PassportNumberText.Result, 
                    Name = NameText.Result, 
                    Destination = DestinationText.Result, 
                    Gender = GenderText.Result, 
                    Nationality = NationalityText.Result, 
                    Job = JobText.Result, 
                    NumberOfEntryTimes = NumberOfEntryTimesText.Result, 
                    LengthOfStayInDays = LengthOfStayInDaysText.Result
                };
                Travellers.Add(traveller1);
            }
            await page.WaitForTimeoutAsync(10000);
            // end of mofa, working with data before using enjaz
            foreach(var traveller in Travellers)
            {
                if (traveller.PassportNumber.Contains(UserPassportNumber))
                    CurrentTraveller = traveller;
            }
            string[] CurrentTravellerName = CurrentTraveller.Name.Split(" ");
            string CurrentTravellerFirstNameArabic = CurrentTravellerName[0];
            string CurrentTravellerSecondNameArabic = CurrentTravellerName[1];
            string CurrentTravellerThirdNameArabic = CurrentTravellerName[2];
            string CurrentTravellerFamilyNameArabic = CurrentTravellerName[CurrentTravellerName.Length-1];
            // Working on Enjaz
            await page.GotoAsync("https://enjazit.com.sa/account/login/person");
            await page.TypeAsync("input[id='UserName']", username);
            await page.TypeAsync("input[id='Password']", password);
            await page.ClickAsync("input[id='Captcha']");
            await page.ClickAsync("#btnSubmit");
            await page.ClickAsync("a[href='/SmartForm/Agreement']");
            await page.ClickAsync("a[href='/SmartForm/ElectronicAgreement']");
            await page.ClickAsync("a[href='/SmartForm/TraditionalApp']");
            // upload profile picture
            await page.WaitForSelectorAsync("#PersonalImage");
            var file = await page.QuerySelectorAsync("#PersonalImage");
            await file.SetInputFilesAsync(filePath);
            // entering calender dates
            /*await page.WaitForSelectorAsync("#PASSPORT_ISSUE_DATE");
            var elementHandle = await page.QuerySelectorAsync("#PASSPORT_ISSUE_DATE");
            await elementHandle.EvaluateAsync("el => el.removeAttribute('readonly')");
            await page.TypeAsync("input[id='PASSPORT_ISSUE_DATE']", date);
            await elementHandle.EvaluateAsync("el => el.setAttribute('readonly', 'true')");*/
            // entering Name
            await page.TypeAsync("input[id='AFIRSTNAME']", CurrentTravellerFirstNameArabic);
            await page.TypeAsync("input[id='AFATHER']", CurrentTravellerSecondNameArabic);
            await page.TypeAsync("input[id='AGRAND']", CurrentTravellerThirdNameArabic);
            await page.TypeAsync("input[id='AFAMILY']", CurrentTravellerFamilyNameArabic);
            await page.TypeAsync("input[id='PASSPORTnumber']", UserPassportNumber);
            var NationalitySearchOptions = await page.QuerySelectorAsync("#NATIONALITY");
            await NationalitySearchOptions.SelectOptionAsync(new SelectOptionValue { Label = CurrentTraveller.Nationality});
            await page.ClickAsync("input[id='JOB_OR_RELATION']");
            await page.TypeAsync("input[id='JOB_OR_RELATION']", CurrentTraveller.Job);
            var DestinationSearchOptions = await page.QuerySelectorAsync("#EmbassyCode");
            await DestinationSearchOptions.SelectOptionAsync(new SelectOptionValue { Label = CurrentTraveller.Destination});
            await page.TypeAsync("input[id='DocumentNumber']", RakamMostanadText);
            await page.TypeAsync("input[id='SPONSER_NAME']", EsmGehaTalebaText);
            await page.TypeAsync("input[id='SPONSER_NUMBER']", RakamSegelText);
            await page.TypeAsync("input[id='SPONSER_ADDRESS']", AddressText);
            await page.TypeAsync("input[id='SPONSER_PHONE']", PhoneNumberText);
            await page.TypeAsync("input[id='porpose']", CurrentTraveller.VisaType);
            await page.WaitForTimeoutAsync(20000);
            await context.CloseAsync();
            return Content(text1 + "//" + RakamMostanadText + "//" + TarekhMostanadText + "//" + EsmGehaTalebaText + "//" + RakamSegelText + "//" + RakamGawalText + "//" + AddressText + "//" + PhoneNumberText + "//" + allTravellers.Count.ToString() + "//" + "//" + CurrentTraveller.Name);
        }

        public IActionResult Testing()
        {
            List<string> dirs = new List<string>(Directory.GetFiles("./Images"));
            var filePath = Path.GetFullPath(dirs[0]);
            return Content(filePath);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
