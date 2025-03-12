namespace PlaywrightDotnetProject.Locators
{
    public static class SignupPageLocators
    {
        // Administrator Info Section
        public const string FullNameInput = "input[name='Full Name']";
        public const string PhoneInput = "input[name='Phone']";
        public const string AdminEmailInput = "input[name='Admin Email']";
        public const string PasswordInput = "input[name='Password']";
        public const string ConfirmPasswordInput = "input[name='Confirm Password']";
        public const string NextButton = "button:has-text('Next')";
        public const string OkButton = "button:has-text('OK')";

        // Verification OTP Section
        public const string VerificationCodeInput = "input[name='Verification Code']";

        // Company Info Section
        public const string CompanyNameInput = "input[name='Company Name']";
        public const string AddressInput = "input[name='Address Line 1']";
        public const string CityInput = "input[name='City']";
        public const string ZipInput = "input[name='Zip/Postal Code']";
        public const string StateDropdown = "#cbomultistate";
        public const string ApplicationDropdown = "#ddlmultiChooseApplication";
        public const string ApplicationSeatsInput = "input[name='0']";
        public const string CompanyNextButton = "button:has-text('Next')";

        // Billing Info Section
        public const string BillingCheckbox = "input#chkBillingSameAsCompany";
        public const string BillingNextButton = "input[onclick*=\"ValidateForm('.reqBilling'\"]";

        // Payment Info Section
        public const string AccountTitleInput = "input[name='Account Title']";
        public const string AccountNumberInput = "input[name='Account Number']";
        public const string RoutingNumberInput = "input[name='Routing/ABA Number']";
        public const string AccountTypeDropdown = "#ddlAccountType";
        public const string ReviewCheckbox = "input[name='Please review and accept ACH']";
        public const string PaymentNextButton = "#btnSubmit";

        // Place Order Section
        public const string UnderstandCheckbox = "#chkUnderstand";
        public const string PlaceOrderButton = "button:has-text('Place Order')";

        // Order Summary Section
        public const string SubscriptionInfo = "//*[@id='DivData']/table/tbody/tr[2]/td[1]/p";
        public const string AdminInfo = "//*[@id='DivData']/table/tbody/tr[2]/td[2]/p";
        public const string GetStartedButton = "button:has-text('Get Started')";
    }
}
