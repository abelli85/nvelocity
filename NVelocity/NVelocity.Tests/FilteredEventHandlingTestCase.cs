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
    using Exception;
    using Runtime;
    using Runtime.Log;


    /// <summary> Tests event handling for all event handlers when multiple event handlers are
    /// assigned for each type.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: FilteredEventHandlingTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class FilteredEventHandlingTestCase : BaseTestCase, ILogChute
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


        private string logString = null;


        /// <summary> Required by LogChute</summary>
        public virtual void Init(IRuntimeServices rs)
        {
            /* don't need it...*/
        }

        [Test]
        public virtual void testFilteredEventHandling()
        {
            string handler1 = "org.apache.velocity.test.eventhandler.Handler1";
            string handler2 = "org.apache.velocity.test.eventhandler.Handler2";
            string sequence1 = handler1 + "," + handler2;
            string sequence2 = handler2 + "," + handler1;

            assureResultsDirectoryExists(RESULTS_DIR);

            /**
            * Set up two VelocityEngines that will apply the handlers in both orders
            */
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, sequence1);
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_NULLSET, sequence1);
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, sequence1);
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_INCLUDE, sequence1);
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            VelocityEngine ve2 = new VelocityEngine();
            ve2.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve2.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, sequence2);
            ve2.SetProperty(RuntimeConstants.EVENTHANDLER_NULLSET, sequence2);
            ve2.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, sequence2);
            ve2.SetProperty(RuntimeConstants.EVENTHANDLER_INCLUDE, sequence2);
            ve2.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve2.Init();

            VelocityContext context;
            System.IO.StringWriter w;


            // check reference insertion with both sequences
            context = new VelocityContext();
            w = new System.IO.StringWriter();
            context.Put("test", "abc");
            ve.Evaluate(context, w, "test", "$test");
            if (!w.ToString().Equals("ABCABC"))
            {
                Assert.Fail("Reference insertion test 1");
            }

            context = new VelocityContext();
            w = new System.IO.StringWriter();
            context.Put("test", "abc");
            ve2.Evaluate(context, w, "test", "$test");
            if (!w.ToString().Equals("ABCabc"))
            {
                Assert.Fail("Reference insertion test 2");
            }

            // check method exception with both sequences

            // sequence 1
            context = new VelocityContext();
            w = new System.IO.StringWriter();
            context.Put("test", new System.Collections.ArrayList());

            try
            {
                ve.Evaluate(context, w, "test", "$test.Get(0)");
                Assert.Fail("Method exception event test 1");
            }
            catch (MethodInvocationException mee)
            {
                // do nothing
            }

            // sequence2
            context = new VelocityContext();
            w = new System.IO.StringWriter();
            context.Put("test", new System.Collections.ArrayList());

            ve2.Evaluate(context, w, "test", "$test.Get(0)");

            // check Log on null set with both sequences
            // sequence 1
            context = new VelocityContext();
            w = new System.IO.StringWriter();
            logString = null;
            ve.Evaluate(context, w, "test", "#set($test1 = $test2)");
            if (logString != null)
            {
                Assert.Fail("log null set test 1");
            }

            // sequence 2
            context = new VelocityContext();
            w = new System.IO.StringWriter();
            logString = null;
            ve2.Evaluate(context, w, "test", "#set($test1 = $test2)");
            if (logString != null)
            {
                Assert.Fail("log null set test 2");
            }


            // check include event handler with both sequences

            // sequence 1
            Template template;
            System.IO.FileStream fos;
           
            System.IO.StreamWriter fwriter;

            template = ve.GetTemplate(getFileName(null, "test4", TMPL_FILE_EXT));

           
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "test4", RESULT_FILE_EXT), System.IO.FileMode.Create);

           
            fwriter = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            context = new VelocityContext();
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test4", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }

            // sequence 2
            template = ve2.GetTemplate(getFileName(null, "test5", TMPL_FILE_EXT));

          
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "test5", RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            fwriter = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            context = new VelocityContext();
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test5", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }




        /// <summary>  handler for LogChute interface</summary>
        public virtual void Log(int level, string message)
        {
            logString = message;
        }

      
        public virtual void Log(int level, string message, System.Exception t)
        {
            logString = message;
        }

        public virtual bool IsLevelEnabled(int level)
        {
            return true;
        }
    }
}
