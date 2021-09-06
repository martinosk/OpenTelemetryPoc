using System;
using System.Collections.Generic;

namespace SomeMessages
{

    public class SomeMessage
    {
        public Dictionary<string, object> TraceMetadata { get; set; }
        public Guid Id { get; set; }
        public SomeMessage() 
        {
            Id = Guid.NewGuid();
            TraceMetadata = new Dictionary<string, object>(); 
        }
        public static string FriendlyName => "some_message";
    }
}
