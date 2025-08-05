using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace InvoiceProcessorFunctionApp
{
    public class Function1
    {
        [FunctionName("ProcessInvoiceBlobFunction")]
        public void Run([BlobTrigger("invoice-upload/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name,
             [CosmosDB(
        databaseName: "InvoiceDB",
        containerName: "Metadata",
        SqlQuery = "SELECT * FROM c WHERE c.invoiceId = {name}",
        Connection = "CosmosDBConnection")] IEnumerable<dynamic> invoiceMetadata, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            if (!invoiceMetadata.Any())
            {
                log.LogWarning($"No metadata found for invoice: {name}");
                return;
            }

            foreach (var meta in invoiceMetadata)
            {
                log.LogInformation($"Invoice Metadata: {meta}");
            }
        }
    }
}
