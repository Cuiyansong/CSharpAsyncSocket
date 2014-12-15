using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace CYS.IO.Socket.WinFormServer.Common
{
    /// <summary>
    /// Logger Enum
    /// </summary>
    public enum LogType
    {
        Debug = 0,
        Warn = 1,
        Error = 2,
        Info = 3,
    }
     
    /// <summary>
    /// 
    /// </summary>
    public class Logger
    {
        #region Public Properties

        #endregion Public Properties

        #region Private Properties

        private static readonly ILog _Logger;

        #endregion Private Properties

        #region Constructor

        static Logger()
        {
            _Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        #endregion Constructor

        #region Public Method

        /// <summary>
        /// Write Logger
        /// </summary>
        /// <param name="logtype">LoggerType</param>
        /// <param name="message">Message</param>
        /// <param name="exp">Exception</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void Write(LogType logtype, string message, Exception exp)
        {
            switch (logtype)
            {
                case LogType.Debug:
                    _Logger.Debug(message, exp);
                    break;
                case LogType.Info:
                    _Logger.Info(message, exp);
                    break;
                case LogType.Warn:
                    _Logger.Warn(message, exp);
                    break;
                case LogType.Error:
                    _Logger.Error(message, exp);
                    break;
                default:
                    throw new NotImplementedException("Logger switch not exist");
            }
        }

        #endregion Public Method

        #region Private Method

        #endregion Private Method
    }
} 
