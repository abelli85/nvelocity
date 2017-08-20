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
    using NVelocity.Runtime;

    using Misc;
    /// <summary> Multiple paths in the file resource loader.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <version>  $Id: MultipleFileResourcePathTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class MultipleFileResourcePathTestCase : BaseTestCase
    {

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private static readonly string FILE_RESOURCE_LOADER_PATH1 = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/multi/path1";

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private static readonly string FILE_RESOURCE_LOADER_PATH2 = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/multi/path2";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/multi";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/multi/Compare";

       
        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            Velocity.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH1);

            Velocity.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH2);

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testMultipleFileResources()
        {
            Template template1 = RuntimeSingleton.GetTemplate(getFileName(null, "path1", TemplateTestBase_Fields.TMPL_FILE_EXT));

            Template template2 = RuntimeSingleton.GetTemplate(getFileName(null, "path2", TemplateTestBase_Fields.TMPL_FILE_EXT));

           
            System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(RESULTS_DIR, "path1", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

         
            System.IO.FileStream fos2 = new System.IO.FileStream(getFileName(RESULTS_DIR, "path2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

           
            System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);
          
            System.IO.StreamWriter writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).Encoding);

            /*
            *  Put the Vector into the context, and merge both
            */

            VelocityContext context = new VelocityContext();

            template1.Merge(context, writer1);
            writer1.Flush();
            writer1.Close();

            template2.Merge(context, writer2);
            writer2.Flush();
            writer2.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "path1", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default) || !isMatch(RESULTS_DIR, COMPARE_DIR, "path2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }
    }
}
