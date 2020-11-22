using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppUpload.Models
{
    [Table("dbo.UploadFile")]
    public class UploadFile
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
