// <copyright file="TopicDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.Queue
{
    /// <summary>
    /// Dto to define address of RabbitMQ server with headers type for exchange.
    /// </summary>
    public class TopicDto : QueueDto
    {
        /// <summary>
        /// The rabbitMQ routing key to listen.
        /// </summary>
        public string RoutingKey { get; set; }
    }
}