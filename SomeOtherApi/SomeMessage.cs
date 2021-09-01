using System;

namespace SomeOtherApi
{
    public class SomeMessage
    {
        public Guid Id { get; set; }
        public SomeMessage() => Id = Guid.NewGuid();
    }
}