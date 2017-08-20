/*
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements.  See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership.  The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License.  You may obtain a copy of the License at
*
*   http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied.  See the License for the
* specific language governing permissions and limitations
* under the License.    
*/

namespace NVelocity.Tests
{
    using NUnit.Framework;

    using App;
    using Runtime;
    using Runtime.Log;

    /// <summary> Tests if we can hand Velocity an arbitrary class for logging.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: ExternalLoggerTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class ExternalLoggerTestCase : ILogChute
    {

        private string logString = null;
        private VelocityEngine ve = null;

        [SetUp]
        public void setUp()
        {
            /*
            *  use an alternative logger.  Set it up here and pass it in.
            */

            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve.Init();
        }

        public virtual void Init(IRuntimeServices rs)
        {
            // do nothing with it
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testExternalLogger()
        {
            /*
            *  simply Log something and see if we Get it.
            */

            logString = null;

            string testString = "This is a test.";

            ve.Log.Warn(testString);

            if (logString == null || !logString.Equals(LogChute_Fields.WARN_PREFIX + testString))
            {
                Assert.Fail("Didn't recieve log message.");
            }
        }

        public virtual void Log(int level, string message)
        {
            string out_Renamed = "";

            /*
            * Start with the appropriate prefix
            */
            switch (level)
            {

                case LogChute_Fields.DEBUG_ID:
                    out_Renamed = LogChute_Fields.DEBUG_PREFIX;
                    break;

                case LogChute_Fields.INFO_ID:
                    out_Renamed = LogChute_Fields.INFO_PREFIX;
                    break;

                case LogChute_Fields.TRACE_ID:
                    out_Renamed = LogChute_Fields.TRACE_PREFIX;
                    break;

                case LogChute_Fields.WARN_ID:
                    out_Renamed = LogChute_Fields.WARN_PREFIX;
                    break;

                case LogChute_Fields.ERROR_ID:
                    out_Renamed = LogChute_Fields.ERROR_PREFIX;
                    break;

                default:
                    out_Renamed = LogChute_Fields.INFO_PREFIX;
                    break;

            }

            logString = out_Renamed + message;
        }

        public virtual void Log(int level, string message, System.Exception t)
        {
            // ignore the Throwable, we're not testing this method here
            Log(level, message);
        }

        public virtual bool IsLevelEnabled(int level)
        {
            return true;
        }
    }
}
