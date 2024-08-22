// <copyright file="HeadersDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.Queue
{
    using System.Collections.Generic;

    /// <summary>
    /// Dto to define address of RabbitMQ server with headers type for exchange.
    /// </summary>
    public class HeadersDto : QueueDto
    {
        /// <summary>
        /// The rabbitMQ headers to listen.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the queue name of rabbitmq.
        /// </summary>
        public string QueueName { get; set; }
    }
}
