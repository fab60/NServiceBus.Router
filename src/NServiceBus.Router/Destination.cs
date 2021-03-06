namespace NServiceBus.Router
{
    using System;

    /// <summary>
    /// Route table destination
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// Creates a new destination.
        /// </summary>
        public Destination(string endpoint, string site)
        {
            if (endpoint == null && site == null)
            {
                throw new ArgumentException("Either endpoint or site has to be specified.");
            }
            Endpoint = endpoint;
            Site = site;
        }

        /// <summary>
        /// Name of the destination endpoint.
        /// </summary>
        public string Endpoint { get; }

        /// <summary>
        /// Optional name of the destination site.
        /// </summary>
        public string Site { get; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Site != null
                ? $"{Endpoint}@{Site}"
                : Endpoint;
        }
    }
}