using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Store;
using System.IO.IsolatedStorage;

namespace Helpers
{
    public class InAppPurchaseHelper
    {
        // Code friendly name of the IAPs
        public enum AvailableIAPs
        {
            Product1,
            Product2,
            Product3,
        }

        // The IAP identifier from the enum names
        static string GetIAPIdentifier(AvailableIAPs IAP)
        {
            switch (IAP)
            {
                case AvailableIAPs.Product3:
                    return "myProductId_THREE";

                case AvailableIAPs.Product2:
                    return "myProductId_TWO";

                default:
                    return "myProductId_ONE";
            }
        }

        public async static Task<string> GetPrice(AvailableIAPs IAP)
        {
            string InAppProductKey = GetIAPIdentifier(IAP);

            try
            {
                // Load IAP listing filtered by current ID
                var listing = await CurrentApp.LoadListingInformationByProductIdsAsync(new string[] { InAppProductKey });

                // Get the IAP License Information
                var product = listing.ProductListings.FirstOrDefault();

                if (product.Value != null)
                {
                    // Return the Localised Price Label
                    return product.Value.FormattedPrice;
                }
                else
                {
                    // Error occured and couldn't find the IAP
                    return "";
                }
            }
            catch (Exception ex)
            {
                // Error occured and couldn't find the IAP
                return "";
            }
        }

        public async static Task<bool> Buy(AvailableIAPs IAP)
        {
            string InAppProductKey = GetIAPIdentifier(IAP);

            try
            {
                // Load IAP listing filtered by current ID
                ListingInformation products = await CurrentApp.LoadListingInformationByProductIdsAsync(new string[] { InAppProductKey });

                // Get the current ID IAP
                ProductListing productListing = null;
                if (!products.ProductListings.TryGetValue(InAppProductKey, out productListing))
                {
                    // Couldn't find IAP name
                    return false;
                }

                // Start IAP purchase
                await CurrentApp.RequestProductPurchaseAsync(productListing.ProductId, false);

                // Check for IAP product license
                ProductLicense productLicense = null;
                if (CurrentApp.LicenseInformation.ProductLicenses.TryGetValue(InAppProductKey, out productLicense))
                {
                    // If Consumable product purchased
                    if (productLicense.IsActive && productLicense.IsConsumable)
                    {
                        // Notify store that IAP has been fulfilled
                        CurrentApp.ReportProductFulfillment(InAppProductKey);

                        // Purchase Successful
                        return true;
                    }
                }
            }
            catch
            {
                // Error occured and Purchase Unsuccessfil
                return false;
            }

            // Purchase unsuccessful
            return false;
        }
    }
}
