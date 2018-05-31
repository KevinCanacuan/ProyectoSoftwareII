using Microsoft.Azure;
using storageTable=Microsoft.Azure.Storage;
using Microsoft.WindowsAzure.Storage;
using System;

namespace prjLegados.AzureStorage.Common
{
    public static class Common
    {
        /// <summary>
        /// Validates the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <returns>CloudStorageAccount object</returns>
        public static CloudStorageAccount fntCreateStorageAccountForBlobs()
        {
            CloudStorageAccount storageAccount;
            const string strMessage = "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.";

            try
            {
                storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            }
            catch (FormatException)
            {
                Console.WriteLine(strMessage);
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine(strMessage);
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        public static storageTable.CloudStorageAccount fntCreateStorageAccountForTables()
        {
            storageTable.CloudStorageAccount storageAccount;
            const string strMessage = "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.";

            try
            {
                storageAccount = storageTable.CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            }
            catch (FormatException)
            {
                Console.WriteLine(strMessage);
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine(strMessage);
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}