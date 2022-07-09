using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgoUserApp.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public  object Data { get; set; }
        public ResponseModel()
        {
            StatusCode = 200;
            Message = "Successful";
            Success = true;
        }
    }
}
