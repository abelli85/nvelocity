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
    using App.Event;
    using Context;
    using Misc;
    using NVelocity.Util;
    using Runtime;

    /// <summary>  Tests event handling
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: IncludeEventHandlingTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class IncludeEventHandlingTestCase : BaseTestCase, IIncludeEventHandler, IRuntimeServicesAware
    {

        /// <summary> VTL file extension.</summary>
        private const string TMPL_FILE_EXT = "vm";

        /// <summary> Comparison file extension.</summary>
        private const string CMP_FILE_EXT = "cmp";

        /// <summary> Comparison file extension.</summary>
        private const string RESULT_FILE_EXT = "res";

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/includeevent";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/includeevent";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/includeevent/Compare";


        private const int PASS_THROUGH = 0;
        private const int RELATIVE_PATH = 1;
        private const int BLOCK = 2;

        private int EventHandlerBehavior = PASS_THROUGH;

        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            Velocity.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }


        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testIncludeEventHandling()
        {
            Template template1 = RuntimeSingleton.GetTemplate(getFileName(null, "test1", TMPL_FILE_EXT));

            Template template2 = RuntimeSingleton.GetTemplate(getFileName(null, "subdir/test2", TMPL_FILE_EXT));

            Template template3 = RuntimeSingleton.GetTemplate(getFileName(null, "test3", TMPL_FILE_EXT));


            System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(RESULTS_DIR, "test1", RESULT_FILE_EXT), System.IO.FileMode.Create);


            System.IO.FileStream fos2 = new System.IO.FileStream(getFileName(RESULTS_DIR, "test2", RESULT_FILE_EXT), System.IO.FileMode.Create);


            System.IO.FileStream fos3 = new System.IO.FileStream(getFileName(RESULTS_DIR, "test3", RESULT_FILE_EXT), System.IO.FileMode.Create);


            System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);

            System.IO.StreamWriter writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).Encoding);

            System.IO.StreamWriter writer3 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos3, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos3, System.Text.Encoding.Default).Encoding);

            /*
            *  lets make a Context and Add the event cartridge
            */

            IContext context = new VelocityContext();

            /*
            *  Now make an event cartridge, register the
            *  input event handler and attach it to the
            *  Context
            */

            EventCartridge ec = new EventCartridge();
            ec.AddEventHandler(this);
            ec.AttachToContext(context);


            // BEHAVIOR A: pass through #input and #parse with no change
            EventHandlerBehavior = PASS_THROUGH;

            template1.Merge(context, writer1);
            writer1.Flush();
            writer1.Close();

            // BEHAVIOR B: pass through #input and #parse with using a relative path
            EventHandlerBehavior = RELATIVE_PATH;

            template2.Merge(context, writer2);
            writer2.Flush();
            writer2.Close();

            // BEHAVIOR C: refuse to pass through #input and #parse
            EventHandlerBehavior = BLOCK;

            template3.Merge(context, writer3);
            writer3.Flush();
            writer3.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test1", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default) || !isMatch(RESULTS_DIR, COMPARE_DIR, "test2", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default) || !isMatch(RESULTS_DIR, COMPARE_DIR, "test3", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }


        public virtual void SetRuntimeServices(IRuntimeServices rs)
        {
        }

        /// <summary> Sample handler with different behaviors for the different tests.</summary>
        public virtual string IncludeEvent(string includeResourcePath, string currentResourcePath, string directiveName)
        {
            if (EventHandlerBehavior == PASS_THROUGH)
                return includeResourcePath;
            // treat as relative path
            else if (EventHandlerBehavior == RELATIVE_PATH)
            {
                // if the resource name starts with a slash, it's not a relative path
                if (includeResourcePath.StartsWith("/") || includeResourcePath.StartsWith("\\"))
                {
                    return includeResourcePath;
                }

                int lastslashpos = System.Math.Max(currentResourcePath.LastIndexOf("/"), currentResourcePath.LastIndexOf("\\"));

                // root of resource tree
                if ((lastslashpos == -1))
                    return includeResourcePath;
                // prepend path to the input path
                else
                    return currentResourcePath.Substring(0, (lastslashpos) - (0)) + "/" + includeResourcePath;
            }
            else if (EventHandlerBehavior == BLOCK)
                return null;
            // should never happen
            else
                return null;
        }
    }
}
