using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy
{
    /// <summary>
    /// Represents all the options you can use to configure the system.
    /// </summary>
    public class PeppyOptions
    {
        /// <summary>
        ///
        /// </summary>
        public PeppyOptions()
        {
            Extensions = new List<IPeppyOptionsExtension>();
        }

        internal IList<IPeppyOptionsExtension> Extensions { get; }

        /// <summary>
        /// Registers an extension that will be executed when building services.
        /// </summary>
        /// <param name="extension"></param>
        public void RegisterExtension(IPeppyOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}