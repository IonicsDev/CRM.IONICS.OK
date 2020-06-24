using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Framework.Logging
{
    /// <summary>
    /// Log Factory
    /// </summary>
    public static class LoggerFactory
    {
        #region Members

        static ILoggerFactory _currentLogFactory = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="logFactory">Log factory to use</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// </summary>
        /// <returns>Created ILog</returns>
        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : null;
        }

        #endregion
    }
}
