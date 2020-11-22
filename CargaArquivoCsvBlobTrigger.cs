using System;
using System.Data.SqlClient;
using System.IO;
using Dapper.Contrib.Extensions;
using FunctionAppUpload.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppUpload
{
    public static class CargaArquivoCsvBlobTrigger
    {
        [FunctionName("CargaArquivoCsvBlobTrigger")]
        public static void Run([BlobTrigger("file-process-csv/{name}", Connection = "AzureWebJobsStorage")]
                                Stream myBlob, string name, ILogger log)
        {

            log.LogInformation($"Arquivo: {name}");

            if (!name.ToLower().EndsWith(".csv"))
            {
                log.LogError($"O arquivo {name} não será processado já que possui uma extensão inválida!");
                return;
            }

            if (myBlob.Length > 0)
            {
                using var reader = new StreamReader(myBlob);
                using var conexaoSql = new SqlConnection(
                    Environment.GetEnvironmentVariable("FileBase"));

                UploadFile uploadFile = new UploadFile()
                {
                    Name = name,
                    UploadDate = DateTime.Now
                };
                conexaoSql.Insert(uploadFile);
                log.LogInformation(
                    $"Id gerado para o arquivo: {uploadFile.FileId}");

                int numLinha = 1;
                string linha = reader.ReadLine();
                while (linha != null)
                {
                    conexaoSql.Insert(new FileContent
                    {
                        FileId = uploadFile.FileId,
                        LineNumber = numLinha,
                        Content = linha
                    });
                    log.LogInformation($"Linha {numLinha}: {linha}");

                    numLinha++;
                    linha = reader.ReadLine();
                }

                log.LogInformation($"Concluído o processamento do arquivo {name}");
            }
            else
                log.LogError($"O arquivo {name} não possui conteúdo!");
        }
    }
}
