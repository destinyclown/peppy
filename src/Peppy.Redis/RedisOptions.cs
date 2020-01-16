namespace Peppy.Redis
{
    /// <summary>
    /// Redis config Options
    /// </summary>
    public class RedisOptions
    {
        /// <summary>
        /// Host
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Default Database
        /// </summary>
        public int DefaultDatabase { get; set; }
    }
}