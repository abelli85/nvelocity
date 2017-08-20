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

    /// <summary> Test use of an absolute path with the FileResourceLoader
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: AbsoluteFileResourceLoaderTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    public class AbsoluteFileResourceLoaderTestCase : BaseTestCase
    {
        /// <summary> VTL file extension.</summary>
        private const string TMPL_FILE_EXT = "vm";

        /// <summary> Comparison file extension.</summary>
        private const string CMP_FILE_EXT = "cmp";

        /// <summary> Comparison file extension.</summary>
        private const string RESULT_FILE_EXT = "res";

        /// <summary> Path to template file.  This will Get combined with the
        /// application directory to form an absolute path
        /// </summary>
        private static readonly string TEMPLATE_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/absolute/absolute";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/absolute/results";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/absolute/Compare";

        /// <summary> Default constructor.</summary>
        internal AbsoluteFileResourceLoaderTestCase()
        {

            try
            {
                assureResultsDirectoryExists(RESULTS_DIR);


                // signify we want to use an absolute path
                Velocity.AddProperty(NVelocity.Runtime.RuntimeConstants.FILE_RESOURCE_LOADER_PATH, "");

                Velocity.SetProperty(NVelocity.Runtime.RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

                Velocity.Init();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Cannot setup AbsoluteFileResourceLoaderTest!");
            }
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public void runTest()
        {
            try
            {

                string curdir = System.Environment.CurrentDirectory;
                string f = getFileName(curdir, TEMPLATE_PATH, TMPL_FILE_EXT);

                System.Console.Out.WriteLine("Retrieving template at absolute path: " + f);

                Template template1 = RuntimeSingleton.GetTemplate(f);

                System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(RESULTS_DIR, "absolute", RESULT_FILE_EXT), System.IO.FileMode.Create);

                System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);

                /*
                *  Put the Vector into the context, and merge both
                */
                VelocityContext context = new VelocityContext();

                template1.Merge(context, writer1);
                writer1.Flush();
                writer1.Close();

                if (!isMatch(RESULTS_DIR, COMPARE_DIR, "absolute", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
                {
                    Assert.Fail("Output incorrect.");
                }
            }
            catch (System.Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
