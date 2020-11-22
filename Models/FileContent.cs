using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppUpload.Models
{
    [Table("dbo.FileContent")]
    public class FileContent
    {
        [Key]
        public int FileContentId { get; set; }
        public int? FileId { get; set; }
        public int? LineNumber { get; set; }
        public string Content { get; set; }
    }
}
