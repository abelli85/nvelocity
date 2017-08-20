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

namespace NVelocity.Tests.Misc
{
    using Runtime;
    using Runtime.Log;

    /// <summary> LogChute implementation that can easily capture output
    /// or suppress it entirely.  By default, both capture and suppress
    /// are on. To have this behave like a normal SystemLogChute,
    /// you must turn it on() and stopCapture().
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <author>  Nathan Bubna
    /// </author>
    /// <version>  $Id: TestLogChute.java 697214 2008-09-19 19:55:59Z nbubna $
    /// </version>
    public class TestLogChute : SystemLogChute
    {
        /// <summary> Return the captured Log messages to date.</summary>
        /// <returns> Log messages
        /// </returns>
        virtual public string Log
        {
            get
            {
                char[] tmpChar;
                byte[] tmpByte;
                tmpByte = log.GetBuffer();
                tmpChar = new char[log.Length];
                System.Array.Copy(tmpByte, 0, tmpChar, 0, tmpChar.Length);
                return new string(tmpChar);
            }

        }
        public const string TEST_LOGGER_LEVEL = "runtime.log.logsystem.test.level";

        private System.IO.MemoryStream log;
    
        private System.IO.StreamWriter systemDotIn;
        private bool suppress = true;
        private bool capture = true;

        public TestLogChute()
        {
            log = new System.IO.MemoryStream();
          
            systemDotIn = new System.IO.StreamWriter(log);
        }

        public TestLogChute(bool suppress, bool capture)
            : this()
        {
            this.suppress = suppress;
            this.capture = capture;
        }

        public override void Init(IRuntimeServices rs)
        {
            base.Init(rs);

            string level = rs.GetString(TEST_LOGGER_LEVEL);
            if (level != null)
            {
                EnabledLevel = toLevel(level);
            }
        }

        public virtual void on()
        {
            suppress = false;
        }

        public virtual void off()
        {
            suppress = true;
        }

        public virtual void startCapture()
        {
            capture = true;
        }

        public virtual void stopCapture()
        {
            capture = false;
        }

        public override bool IsLevelEnabled(int level)
        {
            return !suppress && base.IsLevelEnabled(level);
        }

        protected internal override void write(System.IO.TextWriter ps, string prefix, string message, System.Exception t)
        {
            if (capture)
            {
                base.write(systemDotIn, prefix, message, t);
            }
            else
            {
                base.write(ps, prefix, message, t);
            }
        }
    }
}
