namespace Peppy.RabbitMQ
{
    public class PeppyRabbitMQOptions
    {
        /// <summary>
        /// RabbitMQ connection host
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// RabbitMQ connection port
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// RabbitMQ connection username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// RabbitMQ connection password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Application-specific connection name
        /// </summary>
        public string ClientProvidedName { get; set; }
    }
}