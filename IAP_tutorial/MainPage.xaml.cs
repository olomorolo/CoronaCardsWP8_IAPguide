using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IAP_tutorial.Resources;

// IAP_TUTORIAL: declare that you're using IAP.cs class method:
using Helpers;


namespace IAP_tutorial
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            // Initialize this page's components that were set up via the UI designer.
            InitializeComponent();

            // Set up Corona to automatically start up when the control's Loaded event has been raised.
            // Note: By default, Corona will run the "main.lua" file in the "Assets\Corona" directory.
            //       You can change the defaults via the CoronaPanel's AutoLaunchSettings property.
            fCoronaPanel.AutoLaunchEnabled = true;

            // IAP_TUTORIAL: I'm changing Corona's default Documents Folder to Local Folder
            fCoronaPanel.AutoLaunchSettings.DocumentsDirectoryPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

            // Add Corona event handlers.
            fCoronaPanel.Runtime.Loaded += OnCoronaRuntimeLoaded;

            // Set up the CoronaPanel control to render fullscreen via the DrawingSurfaceBackgroundGrid control.
            // This significantly improves the framerate and is the only means of achieving 60 FPS.
            fCoronaPanel.BackgroundRenderingEnabled = true;
            fDrawingSurfaceBackgroundGrid.SetBackgroundContentProvider(fCoronaPanel.BackgroundContentProvider);
            fDrawingSurfaceBackgroundGrid.SetBackgroundManipulationHandler(fCoronaPanel.BackgroundManipulationHandler);

        }

        // IAP_TUTORIAL: declare event listeners for events dispatched from Corona with these event names:
        private void OnCoronaRuntimeLoaded(object sender, CoronaLabs.Corona.WinRT.CoronaRuntimeEventArgs e)
        {
            e.CoronaRuntimeEnvironment.AddEventListener("buyProductOne", OnCoronaBuyProductOneEventReceived);
            e.CoronaRuntimeEnvironment.AddEventListener("buyProductTwo", OnCoronaBuyProductTwoEventReceived);
            e.CoronaRuntimeEnvironment.AddEventListener("buyProductThree", OnCoronaBuyProductThreeEventReceived);
        }

        // IAP_TUTORIAL: declare what happens when the events are received:
        private CoronaLabs.Corona.WinRT.ICoronaBoxedData OnCoronaBuyProductOneEventReceived(
        CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender, CoronaLabs.Corona.WinRT.CoronaLuaEventArgs e)
        {
            buyProductOne_Click(sender);
            return null;
        }

        private CoronaLabs.Corona.WinRT.ICoronaBoxedData OnCoronaBuyProductTwoEventReceived(
        CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender, CoronaLabs.Corona.WinRT.CoronaLuaEventArgs e)
        {
            buyProductTwo_Click(sender);
            return null;
        }

        private CoronaLabs.Corona.WinRT.ICoronaBoxedData OnCoronaBuyProductThreeEventReceived(
        CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender, CoronaLabs.Corona.WinRT.CoronaLuaEventArgs e)
        {
            buyProductThree_Click(sender);
            return null;
        }

        // IAP_TUTORIAL: declare methods that connect to the store:
        private async void buyProductOne_Click(CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender)
        {
            bool PurchaseSuccessful = await InAppPurchaseHelper.Buy(InAppPurchaseHelper.AvailableIAPs.Product1);

            if (PurchaseSuccessful)
            {
                // I知 passing purchased coins value here but you can add anything you wish to
                OnPurchase(sender, 3000);
            }
            else
            {
                // Show error
                OnPurchaseError(sender);
            }
        }

        private async void buyProductTwo_Click(CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender)
        {
            bool PurchaseSuccessful = await InAppPurchaseHelper.Buy(InAppPurchaseHelper.AvailableIAPs.Product2);

            if (PurchaseSuccessful)
            {
                // I知 passing purchased coins value here but you can add anything you wish to
                OnPurchase(sender, 7000);
            }
            else
            {
                OnPurchaseError(sender);
            }
        }

        private async void buyProductThree_Click(CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender)
        {
            bool PurchaseSuccessful = await InAppPurchaseHelper.Buy(InAppPurchaseHelper.AvailableIAPs.Product3);

            if (PurchaseSuccessful)
            {
                // I知 passing purchased coins value here but you can add anything you wish to
                OnPurchase(sender, 12000);
            }
            else
            {
                OnPurchaseError(sender);
            }
        }

        // IAP_TUTORIAL: dispatch events to let know Corona that the purchase was successful or not:
        private void OnPurchase(
        CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender, int coinsValue)
        {
            // I知 passing purchased coins value here but you can add anything you wish to 
            var eventProperties = CoronaLabs.Corona.WinRT.CoronaLuaEventProperties.CreateWithName("purchase");
            eventProperties.Set("coins", coinsValue);

            var result = sender.DispatchEvent(new CoronaLabs.Corona.WinRT.CoronaLuaEventArgs(eventProperties));
        }

        private void OnPurchaseError(
        CoronaLabs.Corona.WinRT.CoronaRuntimeEnvironment sender)
        {
            var eventProperties = CoronaLabs.Corona.WinRT.CoronaLuaEventProperties.CreateWithName("purchaseError");
            var result = sender.DispatchEvent(new CoronaLabs.Corona.WinRT.CoronaLuaEventArgs(eventProperties));
        }

        // IAP_TUTORIAL: I temporarly set the prices to text value that will be overrided when fetching real prices from store
        // If you comment out upvalue calls from OnNavigatedTo() method - then you'll see that the prices are fetched form the Prices.txt file
        // If you're testing a local build, not a beta version, but you leave the upvalue calls - then your price labels will be blank since 
        // the method won't connect to the store with the local app build and it'll upvalue the prices with null.
        private string Product1_Price = "Price1 from C#";
        private string Product2_Price = "Price2 from C#";
        private string Product3_Price = "Price3 from C#";

        // IAP_TUTORIAL: This method fetches the localised Product price values and writes them in local Prices.txt file
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Set the localised Price labels in each buttons. Comment those out to test price fetching from file on a local app build
            // Note that all product prices in beta versions are 0.00 displayed in local currency.
            Product1_Price = await InAppPurchaseHelper.GetPrice(InAppPurchaseHelper.AvailableIAPs.Product1);

            Product2_Price = await InAppPurchaseHelper.GetPrice(InAppPurchaseHelper.AvailableIAPs.Product2);

            Product3_Price = await InAppPurchaseHelper.GetPrice(InAppPurchaseHelper.AvailableIAPs.Product3);

            // Write the Prices to File
            WritePricesToFile();
        }

        // IAP_TUTORIAL: this method writes fetched prices to the file stored in Local Folder
        void WritePricesToFile()
        {
            // Get the Local Folder for File
            System.IO.IsolatedStorage.IsolatedStorageFile local = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            // Create a new file named Price.txt.
            using (var isoFileStream = new System.IO.IsolatedStorage.IsolatedStorageFileStream("Prices.txt", System.IO.FileMode.Create, local))
            {
                // Write the price data in newlines so that we can cut it later in Corona
                using (var isoFileWriter = new System.IO.StreamWriter(isoFileStream))
                {
                    string Prices = Product1_Price + "\n" + Product2_Price + "\n" + Product3_Price;

                    isoFileWriter.Write(Prices);
                }
            }
        }


    }
}
