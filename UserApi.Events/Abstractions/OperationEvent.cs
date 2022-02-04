using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserApi.Events.Abstractions
{
    public class OperationEvent
    {
        public OperationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public OperationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public DateTime CreationDate { get; set; }
    }
}
