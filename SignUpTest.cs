using NUnit.Framework;
using PlaywrightDotnetProject.Pages;
using System;
using System.Threading.Tasks;

namespace PlaywrightDotnetProject.Tests
{
    [TestFixture]
    public class SignupTest : BaseTest
    {
        [Test]
        public async Task SignupFlowTest()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            ExtentReportHelper.CreateTest(testName);

            try
            {
                var signupPage = new SignupPage(Page);
                
                ExtentReportHelper.LogStep("Navigating to Signup Page");
                await signupPage.SignupAsync();

                ExtentReportHelper.LogStep("Filling Company Info");
                await signupPage.AdministratorInfoAsync();

                ExtentReportHelper.LogStep("Entering Verification Code");
                await signupPage.EnterVerificationOTPAsync();

                ExtentReportHelper.LogStep("Filling Company Details");
                await signupPage.CompanySubscriptionAsync();

                ExtentReportHelper.LogStep("Filling Billing Info");
                await signupPage.BillingInfoAsync();

                ExtentReportHelper.LogStep("Filling Bank Info");
                await signupPage.PaymentInfoAsync();

                ExtentReportHelper.LogStep("Placing Order");
                await signupPage.PlaceOrderAsync();

                ExtentReportHelper.LogStep("Verifying Order Summary");
                await signupPage.VerifyOrderSummaryAsync();


                ExtentReportHelper.LogTestSuccess(testName);
            }
            catch (Exception ex)
            {
                var screenshotPath = $"ExtentReports/{testName}.png";
                await Page.ScreenshotAsync(new() { Path = screenshotPath });
                ExtentReportHelper.LogTestFailure(testName, screenshotPath);
                ExtentReportHelper.LogTestError($"Error Message: {ex.Message}");
                throw;
            }
        }
    }
}
