using System.Collections.Generic;

namespace ISUCorp.API.Resources
{
    public class SuccessResource
    {
        public bool Success => true;

        public List<string> Messages { get; private set; }

        public SuccessResource(List<string> messages)
        {
            Messages = messages ?? new List<string>();
        }

        public SuccessResource(string message)
        {
            Messages = new List<string>();

            if (!string.IsNullOrWhiteSpace(message))
            {
                Messages.Add(message);
            }
        }
    }
}
