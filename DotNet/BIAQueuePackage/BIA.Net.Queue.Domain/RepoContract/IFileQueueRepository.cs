// <copyright file="IFileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;

    /// <summary>
    /// Interface of FileQueueRepository.
    /// </summary>
    public interface IFileQueueRepository
    {
        /// <summary>
        /// Configure the queues.
        /// </summary>
        /// <param name="topics">The list of <see cref="TopicDto"/>.</param>
        void Configure(IEnumerable<TopicDto> topics);

        /// <summary>
        /// Configure the queues.
        /// </summary>
        /// <param name="headers">The list of <see cref="HeadersDto"/>.</param>
        void ConfigureHeaders(IEnumerable<HeadersDto> headers);

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/> with exchange type headers.
        /// </summary>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileMessageDto"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An iDisposable.</returns>
        IDisposable SubscribeToHeadersWithAuthentication(
            IObserver<FileMessageDto> observer,
            CancellationToken cancellationToken,
            string user,
            string password);

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/>  with exchange type headers.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IDisposable SubscribeToHeaders(IObserver<FileMessageDto> observer, CancellationToken cancellationToken);

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/>  with exchange type topics.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IDisposable SubscribeToTopicsWithAuthentication(
           IObserver<FileMessageDto> observer,
           CancellationToken cancellationToken,
           string user,
           string password);

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/>  with exchange type topics.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IDisposable SubscribeToTopics(IObserver<FileMessageDto> observer, CancellationToken cancellationToken);

        /// <summary>
        /// Send a new <see cref="FileMessageDto"/>.
        /// </summary>
        /// <param name="header">The global information to read a RabbitMQ Header.</param>
        /// <param name="file"> The file.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(HeadersDto header, FileMessageDto file);

        /// <summary>
        /// Send a new <see cref="FileMessageDto"/>.
        /// </summary>
        /// <param name="header">The global information to read a RabbitMQ Header.</param>
        /// <param name="file"> The file.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        bool SendFile(HeadersDto header, FileMessageDto file, string user, string password);
    }
}
