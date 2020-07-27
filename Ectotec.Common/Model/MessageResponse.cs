using System.ComponentModel;

namespace Ectotec.Common.Model
{
    public class MessageResponse<T>
    {
        public MessageResponse()
        {
            success = true;
            errorMessage = "";
        }
        [DefaultValue(true)]
        public bool success { get; set; }
        [DefaultValue("")]
        public string errorMessage { get; set; }
        public T response { get; set; }
    }
}
