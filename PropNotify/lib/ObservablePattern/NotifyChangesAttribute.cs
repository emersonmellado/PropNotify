using System;

namespace ThermometerLibrary.ObservablePattern
{
    public class NotifyChangesAttribute : Attribute
    {
        public string Message;

        public NotifyChangesAttribute(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
