using Microsoft.Playwright;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bogus;

namespace PlaywrightDotnetProject.Pages
{
    public class SignupPage
    {
        private readonly IPage _page;
        private string? _verificationCode;
        private bool _isConsoleListenerAttached = false;
        private readonly Faker _faker = new();

        public SignupPage(IPage page)
        {
            _page = page;
            AttachConsoleListener();
        }

        private void AttachConsoleListener()
        {
            if (_isConsoleListenerAttached) return;

            _page.Console += (_, msg) =>
            {
                Console.WriteLine($"[Console Log]: {msg.Text}");
                var match = Regex.Match(msg.Text, @"\b\d{4,6}\b");
                if (match.Success)
                {
                    _verificationCode = match.Value;
                    Console.WriteLine($"[Captured Verification Code]: {_verificationCode}");
                }
            };

            _isConsoleListenerAttached = true;
        }

        public async Task SignupAsync()
        {
            await _page.GotoAsync("https://qasignup.e-bizsoft.net/Signup", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = 60000
            });
        }

        public async Task AdministratorInfoAsync()
        {
            var fullName = _faker.Name.FullName();
            var phone = _faker.Phone.PhoneNumber("##########");
            var email = _faker.Internet.Email();
            var password = "Aa1234567";

            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Full Name" }).FillAsync(fullName);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Phone" }).FillAsync(phone);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Admin Email" }).FillAsync(email);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync(password);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password", Exact = true }).FillAsync(password);
            await _page.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();
        }

        public async Task EnterVerificationOTPAsync()
        {
            Console.WriteLine("[Waiting for Verification Code input...]");
            var verificationInput = _page.GetByRole(AriaRole.Textbox, new() { Name = "Verification Code" });
            await verificationInput.WaitForAsync(new() { Timeout = 30000 });

            int retries = 30;
            while (retries-- > 0 && string.IsNullOrEmpty(_verificationCode))
            {
                await Task.Delay(1000);
            }

            if (string.IsNullOrEmpty(_verificationCode))
                throw new Exception("[Error] Verification code not captured from console logs.");

            Console.WriteLine($"[Entering Verification Code]: {_verificationCode}");
            await verificationInput.FillAsync(_verificationCode);
            await _page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Company Name" }).WaitForAsync(new() { Timeout = 30000 });
        }

        public async Task CompanySubscriptionAsync()
        {
            var companyName = _faker.Company.CompanyName();
            var phone = _faker.Phone.PhoneNumber("##########");
            var address = _faker.Address.StreetAddress();
            var city = _faker.Address.City();
            var zip = _faker.Address.ZipCode("#####");

            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Company Name" }).FillAsync(companyName);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Phone" }).FillAsync(phone);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Address Line 1" }).FillAsync(address);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "City" }).FillAsync(city);
            await _page.Locator("#cbomultistate").SelectOptionAsync("195");
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Zip/Postal Code" }).FillAsync(zip);
            await _page.Locator("#ddlmultiChooseApplication").SelectOptionAsync("2");
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "0" }).FillAsync("5");

            await _page.Locator("div#overlay").WaitForAsync(new() { State = WaitForSelectorState.Hidden });
            await _page.GetByRole(AriaRole.Button, new() { Name = "Next" }).ClickAsync();

            var noButton = _page.GetByRole(AriaRole.Button, new() { Name = "No" });
            await noButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await noButton.ScrollIntoViewIfNeededAsync();
            await noButton.ClickAsync(new() { Force = true });

            Console.WriteLine("[Modal]: Clicked on 'No' button.");

            await Task.WhenAll(
                _page.Locator("div.sweet-alert").WaitForAsync(new() { State = WaitForSelectorState.Hidden }),
                _page.Locator(".sweet-overlay").WaitForAsync(new() { State = WaitForSelectorState.Hidden })
            );

            Console.WriteLine("[Modal]: SweetAlert modal and overlay are hidden.");
        }
                public async Task BillingInfoAsync()
{
    Console.WriteLine("[Action]: Starting Billing Information process...");

    // Handle overlay if visible
    var overlay = _page.Locator("div#overlay");
    if (await overlay.IsVisibleAsync())
    {
        Console.WriteLine("[Info]: Waiting for overlay to hide.");
        await overlay.WaitForAsync(new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });
    }
    var billingCheckbox = _page.Locator("input#chkBillingSameAsCompany").First;

    await billingCheckbox.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
    await billingCheckbox.ScrollIntoViewIfNeededAsync();

    if (!await billingCheckbox.IsCheckedAsync())
    {
        await billingCheckbox.CheckAsync(new() { Force = true });
        Console.WriteLine("[Action]: Checked 'Billing Same As Company'.");
    }
    else
    {
        Console.WriteLine("[Info]: 'Billing Same As Company' checkbox was already checked.");
    }

    // Wait and click the Next button
    var billingNextButton = _page.Locator("input[onclick*=\"ValidateForm('.reqBilling'\"]");
    await billingNextButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
    await billingNextButton.ScrollIntoViewIfNeededAsync();
    await billingNextButton.ClickAsync(new() { Force = true });

    Console.WriteLine("[Action]: Clicked on Billing section Next button.");
    Console.WriteLine("[Success]: Billing Information submitted successfully.");
}

        public async Task PaymentInfoAsync()
        {
            var accountTitle = _faker.Name.FullName();
            var accountNumber = _faker.Finance.Account();
            var routingNumber = _faker.Random.ReplaceNumbers("#########");

            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Account Title" }).FillAsync(accountTitle);
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Account Number" }).FillAsync(accountNumber);
            await _page.Locator("#ddlAccountType").SelectOptionAsync(new SelectOptionValue { Label = "Checking Account" });
            await _page.GetByRole(AriaRole.Textbox, new() { Name = "Routing/ABA Number" }).FillAsync(routingNumber);

            var reviewCheckbox = _page.GetByRole(AriaRole.Checkbox, new() { Name = "Please review and accept ACH" });
            await reviewCheckbox.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await reviewCheckbox.ScrollIntoViewIfNeededAsync();

            if (!await reviewCheckbox.IsCheckedAsync())
            {
                await reviewCheckbox.CheckAsync(new() { Force = true });
            }

            Console.WriteLine("[Action]: Checked 'Please review and accept ACH'.");

            var nextButton = _page.Locator("#btnSubmit");
            await nextButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await nextButton.ScrollIntoViewIfNeededAsync();
            await nextButton.ClickAsync(new() { Force = true });
        }
            public async Task PlaceOrderAsync()
    {
            var overlay = _page.Locator("#overlay");
            if (await overlay.IsVisibleAsync())
            await overlay.WaitForAsync(new() { State = WaitForSelectorState.Hidden });

            var understandCheckbox = _page.Locator("#chkUnderstand").First;
            await understandCheckbox.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
            await understandCheckbox.ScrollIntoViewIfNeededAsync();

            if (!await understandCheckbox.IsCheckedAsync())
            await understandCheckbox.CheckAsync(new() { Force = true });

            var placeOrderButton = _page.GetByRole(AriaRole.Button, new() { Name = "Place Order" });
             await placeOrderButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
             await placeOrderButton.ScrollIntoViewIfNeededAsync();
            await placeOrderButton.ClickAsync(new() { Force = true });

            Console.WriteLine("[Success]: Order placed successfully.");
    }
           
public async Task VerifyOrderSummaryAsync()
{
    // Subscription Info
    var subscriptionLocator = _page.Locator("//*[@id='DivData']/table/tbody/tr[2]/td[1]/p");
    await subscriptionLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });

    var subscriptionText = await subscriptionLocator.InnerTextAsync();
    if (string.IsNullOrEmpty(subscriptionText))
        throw new Exception("[Error]: Missing subscription info.");
    Console.WriteLine($"[Debug]: Subscription info: {subscriptionText}");

    // Admin Info
    var adminLocator = _page.Locator("//*[@id='DivData']/table/tbody/tr[2]/td[2]/p");
    await adminLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });

    var adminText = await adminLocator.InnerTextAsync();
    if (string.IsNullOrEmpty(adminText))
        throw new Exception("[Error]: Missing admin info.");
    Console.WriteLine($"[Debug]: Admin info: {adminText}");

    Console.WriteLine("[Success]: Both subscription and admin info verified.");

    // Click on the "Get Started" button
    await _page.GetByRole(AriaRole.Button, new() { Name = "Get Started" }).ClickAsync();
}

}}

        
    

