// <copyright file="QueueDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.Dto.Queue
{
    /// <summary>
    /// Dto to define address of RabbitMQ server.
    /// </summary>
    public class QueueDto
    {
        /// <summary>
        /// The RabbitMQ server URI.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The RabbitMQ port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The Virtual Host.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// The exchange to listen.
        /// </summary>
        public string Exchange { get; set; }
    }
}
