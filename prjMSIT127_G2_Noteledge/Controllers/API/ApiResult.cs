using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;

namespace prjMSIT127_G2_Noteledge.Controllers.API
{
    public class ApiResult : ContentResult
    {
        public ApiResult() : this(new ResultData(){result = "ok"})
        {
            
        }

        public ApiResult(string message) : this(new ResultData() { result = "error", message = message })
        {

        }

        private ApiResult(ResultData data)
        {
            Content = JsonSerializer.Serialize<ResultData>(data);
            ContentEncoding = Encoding.UTF8;
            ContentType = "json";
        }
    }

    class ResultData
    {

        public string result { get; set; }
        public string message { get; set; }
    }
}