﻿/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
* @author Richard Lyle (rolyle@us.ibm.com)
*/

using System;
using System.IO;

namespace IBM.Watson.Logging
{
    /// <summary>
    /// FileReactor log reactor class.
    /// </summary>
    public class FileReactor : ILogReactor
    {
        #region Public Properties
        public string LogFile { get; set; }
        public LogLevel Level { get; set; }
        #endregion

        #region Construction
        /// <summary>
        /// FileReactor constructor.
        /// </summary>
        /// <param name="logFile">The FileName of the log file.</param>
        /// <param name="level">The minimum level of log messages to be logged into the file.</param>
        /// <param name="logHistory">How many log files to keep as they are rotated each time this reactor is constructed.</param>
        public FileReactor(string logFile, LogLevel level = LogLevel.DEBUG, int logHistory = 2 )
        {
            LogFile = logFile;
            Level = level;

            // rotate existing log files..
            for(int i=logHistory;i>=0;--i)
            {
                string src = i > 0 ? LogFile + "." + i.ToString() : LogFile;
                if ( File.Exists( src ) )
                {
                    string dst = LogFile + "." + (i + 1).ToString();
                    File.Copy( src, dst, true );
                }
            }

            File.WriteAllText(LogFile, string.Format("Log File Started {0}...\n",
                DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")) );
        }
        #endregion

        #region ILogReactor interface
        public void ProcessLog(LogRecord log)
        {
            if (log.m_Level >= Level)
            {
                File.AppendAllText(LogFile, string.Format("[{0}][{1}][{2}] {3}\n",
                    log.m_TimeStamp.ToString("MM/dd/yyyy HH:mm:ss"),
                    log.m_SubSystem, log.m_Level.ToString(), log.m_Message));
            }
        }
        #endregion
    }
}
