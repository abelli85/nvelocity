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
    using System;

    using NUnit.Framework;

    using App;
    using Runtime;
    using Runtime.Resource.Loader;

    using Misc;

    /// <summary> Test that an instance of a ResourceLoader can be successfully passed in.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ResourceLoaderInstanceTestCase.java 691334 2008-09-02 18:10:41Z nbubna $
    /// </version>
    [TestFixture]
    public class ResourceLoaderInstanceTestCase : BaseTestCase
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
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/resourceinstance";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/resourceinstance";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/resourceinstance/Compare";

        private TestLogChute logger = new TestLogChute();

        [SetUp]
        public void setUp()
        {

            ResourceLoader rl = new FileResourceLoader();

            // pass in an instance to Velocity
            Velocity.SetProperty("resource.loader", "testrl");
            Velocity.SetProperty("testrl.resource.loader.instance", rl);
            Velocity.SetProperty("testrl.resource.loader.path", FILE_RESOURCE_LOADER_PATH);

            // parameters instance of logger
            logger.on();
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, logger);
            Velocity.SetProperty("runtime.log.logsystem.test.level", "debug");

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testResourceLoaderInstance()
        {
            //caveman hacks to Get gump to give more Info
            try
            {
                assureResultsDirectoryExists(RESULTS_DIR);

                Template template = RuntimeSingleton.GetTemplate(getFileName(null, "testfile", TMPL_FILE_EXT));

               
                System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "testfile", RESULT_FILE_EXT), System.IO.FileMode.Create);
                //caveman hack to Get gump to give more Info
                System.Console.Out.WriteLine("All needed files exist");

              
                System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

                /*
                *  Put the Vector into the context, and merge both
                */

                VelocityContext context = new VelocityContext();

                template.Merge(context, writer);
                writer.Flush();
                writer.Close();
            }
            catch (System.Exception e)
            {
                System.Console.Out.WriteLine("Log was: " + logger.Log);
                
                System.Console.Out.WriteLine(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "testfile", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                string result = getFileContents(TemplateTestBase_Fields.RESULT_DIR, "testfile", RESULT_FILE_EXT, System.Text.Encoding.Default);
                string compare = getFileContents(COMPARE_DIR, "testfile", CMP_FILE_EXT, System.Text.Encoding.Default);

                string msg = "Processed template did not match expected output\n" + "-----Result-----\n" + result + "----Expected----\n" + compare + "----------------";

                //caveman hack to Get gump to give more Info
                System.Console.Out.WriteLine(msg);
                System.Console.Out.WriteLine("Log was: " + logger.Log);
                Assert.Fail(msg);
            }
        }
    }
}
