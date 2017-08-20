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
    using Misc;
    using Runtime;

    /// <summary> Make sure that a forward referenced macro inside another macro definition does
    /// not report an Error in the Log.
    /// (VELOCITY-71).
    /// 
    /// </summary>
    /// <author>  <a href="mailto:henning@apache.org">Henning P. Schmiedehausen</a>
    /// </author>
    /// <version>  $Id: MacroForwardDefineTestCase.java 697214 2008-09-19 19:55:59Z nbubna $
    /// </version>
    [TestFixture]
    public class MacroForwardDefineTestCase : BaseTestCase
    {
        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>

        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/macroforwarddefine";

        /// <summary> Results relative to the build directory.</summary>

        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/macroforwarddefine";

        /// <summary> Results relative to the build directory.</summary>

        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/macroforwarddefine/Compare";

        /// <summary> Collects the Log messages.</summary>
        private TestLogChute logger = new TestLogChute();


        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            // use Velocity.SetProperty (instead of properties file) so that we can use parameters instance of Log
            Velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_REFERENCE_LOG_INVALID, "true");

            // parameters instance of logger
            logger = new TestLogChute();
            logger.off();
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, logger);
            Velocity.SetProperty(TestLogChute.TEST_LOGGER_LEVEL, "debug");
            Velocity.Init();
        }

        [Test]
        public virtual void testLogResult()
        {
            VelocityContext context = new VelocityContext();
            Template template = Velocity.GetTemplate("macros.vm");

            // try to Get only messages during merge
            logger.on();
            template.Merge(context, new System.IO.StringWriter());
            logger.off();

            string resultLog = logger.Log;
            if (!isMatch(resultLog, COMPARE_DIR, "velocity.log", "cmp", System.Text.Encoding.Default))
            {
                string compare = getFileContents(COMPARE_DIR, "velocity.log", TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default);

                string msg = "Log output was incorrect\n" + "-----Result-----\n" + resultLog + "----Expected----\n" + compare + "----------------";

                Assert.Fail(msg);
            }
        }
    }
}
