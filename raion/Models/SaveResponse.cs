using System;
namespace raion.Models
{
    public class SaveResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public SaveResponse(bool Result , string Message)
        {
            this.Result = Result;
            this.Message = Message;
        }
        public SaveResponse() { }
    }
}
