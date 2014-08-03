CoronaCardsWP8_IAPguide
=======================

Note that IAP methods will work ONLY with submitted public and beta app versions downloaded from the Windows Phone Store.

1. Make the beta app version and add products in the Windows Dev Center Dashboard - declare your product identifier names there.
2. Paste your unique product identifiers to the GetIAPIdentifier method in IAP.cs file, submit the beta app and test it. Note that all product prices in beta versions are 0.00 displayed in local currency.
3. Declare the exact same product identifiers names in your release app version
4. Reuse the same code for the release app

In the MainPage.xaml.cs file all the code blocks used for IAP I'm commenting starting with "//IAP_TUTORIAL: " so that you know which part is necessary to implement for IAPs.
