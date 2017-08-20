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
    using Context;
    using Runtime;
    using Runtime.Log;

    /// <summary> Test a reported bug in which method overloading throws IllegalArgumentException 
    /// after a null return value.
    /// (VELOCITY-132).
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: MethodOverloadingTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class MethodOverloadingTestCase : BaseTestCase, ILogChute
    {
        internal string logData;

        /// <summary> VTL file extension.</summary>
        private const string TMPL_FILE_EXT = "vm";

        /// <summary> Comparison file extension.</summary>
        private const string CMP_FILE_EXT = "cmp";

        /// <summary> Comparison file extension.</summary>
        private const string RESULT_FILE_EXT = "res";

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
     
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/methodoverloading";

        /// <summary> Results relative to the build directory.</summary>
       
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/methodoverloading";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/methodoverloading/Compare";

        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);
        }

        [Test]
        public virtual void testMethodOverloading()
        {
            /**
            * test overloading in a single template
            */
            testFile("single");

            Assert.IsTrue(logData.IndexOf("IllegalArgumentException") == -1);
        }

        [Test]
        public virtual void testParsedMethodOverloading()
        {
            /**
            * test overloading in a file included with #parse
            */
            testFile("main");

            Assert.IsTrue(logData.IndexOf("IllegalArgumentException") == -1);
        }

        [Test]
        public virtual void testFile(string basefilename)
        {

            VelocityEngine ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve.Init();

            Template template;
            System.IO.FileStream fos;
          
            System.IO.TextWriter fwriter;
            IContext context;

            template = ve.GetTemplate(getFileName(null, basefilename, TMPL_FILE_EXT));

          
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, basefilename, RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            fwriter = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            context = new VelocityContext();
            setupContext(context);
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, basefilename, RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }

        public virtual void setupContext(IContext context)
        {
            context.Put("test", this);
            context.Put("nullValue", (object)null);
        }


     
        public virtual string overloadedMethod(ref System.Int32 s)
        {
            return "Integer";
        }

        public virtual string overloadedMethod(string s)
        {
            return "String";
        }


     
        public virtual string overloadedMethod2(ref System.Int32 s)
        {
            return "Integer";
        }

        public virtual string overloadedMethod2(string i)
        {
            return "String";
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

            logData += ("\n" + out_Renamed + message);
        }

        public virtual void Init(IRuntimeServices rs)
        {
            // do nothing with it
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
