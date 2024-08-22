// <copyright file="FileQueueRepository.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Queue.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Queue.Common.Observer;
    using BIA.Net.Queue.Domain.Dto.FileQueue;
    using BIA.Net.Queue.Domain.Dto.Queue;
    using BIA.Net.Queue.Domain.RepoContract;
    using BIA.Net.Queue.Infrastructure.Service.Helpers;

    /// <summary>
    /// Repository for file queuing.
    /// </summary>
    public sealed class FileQueueRepository : IObservable<FileMessageDto>, IFileQueueRepository
    {
        private readonly List<IObserver<FileMessageDto>> observers;
        private IEnumerable<TopicDto> topics;
        private IEnumerable<HeadersDto> headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileQueueRepository"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint address of queue Server.</param>
        /// <param name="queueName">The queue name listen.</param>
        public FileQueueRepository()
        {
            this.observers = new List<IObserver<FileMessageDto>>();
        }

        /// <inheritdoc />
        public void Configure(IEnumerable<TopicDto> topics)
        {
            if (topics == null)
            {
                throw new ArgumentException("queues cannot be null");
            }

            this.topics = topics;
        }

        /// <inheritdoc />
        public void ConfigureHeaders(IEnumerable<HeadersDto> headers)
        {
            if (headers == null)
            {
                throw new ArgumentException("headers for queues cannot be null");
            }

            this.headers = headers;
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<FileMessageDto> observer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IFileQueueRepository.SubscribeToTopics(IObserver{FileMessageDto}, CancellationToken)"/>
        public IDisposable SubscribeToTopics(IObserver<FileMessageDto> observer, CancellationToken cancellationToken)
        {
            return this.Subscribe<TopicDto>(
                observer,
                (topic, cancellationToken) => RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(topic, observer.OnNext, cancellationToken),
                cancellationToken);
        }

        /// <inheritdoc cref="IFileQueueRepository.SubscribeToTopicsWithAuthentication(IObserver{FileMessageDto}, CancellationToken, string, string)"/>
        public IDisposable SubscribeToTopicsWithAuthentication(
            IObserver<FileMessageDto> observer,
            CancellationToken cancellationToken,
            string user,
            string password)
        {
            return this.Subscribe<TopicDto>(
                observer,
                (topic, user, password, cancellationToken) => RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(topic, observer.OnNext, cancellationToken, user, password),
                cancellationToken,
                user,
                password);
        }

        /// <inheritdoc cref="IFileQueueRepository.SubscribeToHeaders(IObserver{FileMessageDto}, CancellationToken)"/>
        public IDisposable SubscribeToHeaders(IObserver<FileMessageDto> observer, CancellationToken cancellationToken)
        {
            return this.Subscribe<HeadersDto>(
                observer,
                (header, cancellationToken) => RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(header, observer.OnNext, cancellationToken),
                cancellationToken);
        }

        /// <inheritdoc cref="IFileQueueRepository.SubscribeToHeadersWithAuthentication(IObserver{FileMessageDto}, CancellationToken, string, string)"/>
        public IDisposable SubscribeToHeadersWithAuthentication(
            IObserver<FileMessageDto> observer,
            CancellationToken cancellationToken,
            string user,
            string password)
        {
            return this.Subscribe<HeadersDto>(
                observer,
                (header, user, password, cancellationToken) => RabbitMQHelper.ReceiveMessageAsync<FileMessageDto>(header, observer.OnNext, cancellationToken, user, password),
                cancellationToken,
                user,
                password);
        }

        /// <inheritdoc />
        public bool SendFile(HeadersDto header, FileMessageDto file)
        {
            return RabbitMQHelper.SendMessage(header, file);
        }

        /// <inheritdoc />
        public bool SendFile(HeadersDto header, FileMessageDto file, string user, string password)
        {
            return RabbitMQHelper.SendMessage(header, file, user, password);
        }

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/> without authentication.
        /// </summary>
        /// <typeparam name="TQueue">The type of exchange.</typeparam>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileMessageDto"/>.</param>
        /// <param name="receiveMessageAction">The receiveMessageAction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An iDisposable.</returns>
        private IDisposable Subscribe<TQueue>(
            IObserver<FileMessageDto> observer,
            Func<TQueue, CancellationToken, Task> receiveMessageAction,
            CancellationToken cancellationToken)
        {
            // Check whether observer is already registered. If not, add it
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);

                IEnumerable<TQueue> exchangeCollection = this.GetCollection<TQueue>();

                foreach (TQueue exchange in exchangeCollection)
                {
                    receiveMessageAction(exchange, cancellationToken);
                }
            }

            return new Unsubscriber<FileMessageDto>(this.observers, observer);
        }

        /// <summary>
        /// Subscribe to receive <see cref="FileMessageDto"/> with authentication.
        /// </summary>
        /// <typeparam name="TQueue">The type of exchange binding with the queue.</typeparam>
        /// <param name="observer">The <see cref="IObserver{T}"/> of <see cref="FileMessageDto"/>.</param>
        /// <param name="receiveMessageAction">The receiveMessageAction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="user">The user identifier.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An iDisposable.</returns>
        private IDisposable Subscribe<TQueue>(
            IObserver<FileMessageDto> observer,
            Func<TQueue, string, string, CancellationToken, Task> receiveMessageAction,
            CancellationToken cancellationToken,
            string user,
            string password)
        {
            // Check whether observer is already registered. If not, add it
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);

                IEnumerable<TQueue> exchangeCollection = this.GetCollection<TQueue>();

                foreach (TQueue exchange in exchangeCollection)
                {
                    receiveMessageAction(exchange, user, password, cancellationToken);
                }
            }

            return new Unsubscriber<FileMessageDto>(this.observers, observer);
        }

        /// <summary>
        /// Get the type of collection.
        /// </summary>
        /// <typeparam name="TQueue">The type of exchange.</typeparam>
        /// <returns>The type.</returns>
        /// <exception cref="InvalidOperationException">Type not supported.</exception>
        private IEnumerable<TQueue> GetCollection<TQueue>()
        {
            if (typeof(TQueue) == typeof(TopicDto))
            {
                return (IEnumerable<TQueue>)this.topics;
            }
            else if (typeof(TQueue) == typeof(HeadersDto))
            {
                return (IEnumerable<TQueue>)this.headers;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported type: {typeof(TQueue).Name}");
            }
        }
    }
}