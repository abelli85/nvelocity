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

    using Misc;

    /// <summary> Load templates from the Classpath.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <author>  <a href="mailto:daveb@miceda-data.com">Dave Bryson</a>
    /// </author>
    /// <version>  $Id: MultiLoaderTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class MultiLoaderTestCase : BaseTestCase
    {
        /// <summary> VTL file extension.</summary>
        private const string TMPL_FILE_EXT = "vm";

        /// <summary> Comparison file extension.</summary>
        private const string CMP_FILE_EXT = "cmp";

        /// <summary> Comparison file extension.</summary>
        private const string RESULT_FILE_EXT = "res";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/multiloader";

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/multiloader";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/multiloader/Compare";

        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            /*
            * Set up the file loader.
            */

            Velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");

            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);

            Velocity.AddProperty(RuntimeConstants.RESOURCE_LOADER, "classpath");

            Velocity.AddProperty(RuntimeConstants.RESOURCE_LOADER, "jar");

            /*
            *  Set up the classpath loader.
            */

            Velocity.SetProperty("classpath." + RuntimeConstants.RESOURCE_LOADER + ".class", "org.apache.velocity.runtime.resource.loader.ClasspathResourceLoader");

            Velocity.SetProperty("classpath." + RuntimeConstants.RESOURCE_LOADER + ".cache", "false");

            Velocity.SetProperty("classpath." + RuntimeConstants.RESOURCE_LOADER + ".modificationCheckInterval", "2");

            /*
            *  setup the Jar loader
            */

            Velocity.SetProperty("jar." + RuntimeConstants.RESOURCE_LOADER + ".class", "org.apache.velocity.runtime.resource.loader.JarResourceLoader");

            Velocity.SetProperty("jar." + RuntimeConstants.RESOURCE_LOADER + ".path", "jar:file:" + FILE_RESOURCE_LOADER_PATH + "/test2.jar");


            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

 
        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testMultiLoader()
        {
            /*
            *  lets ensure the results directory exists
            */
            assureResultsDirectoryExists(RESULTS_DIR);

            /*
            * Template to find with the file loader.
            */
            Template template1 = Velocity.GetTemplate(getFileName(null, "path1", TMPL_FILE_EXT));

            /*
            * Template to find with the classpath loader.
            */
            Template template2 = Velocity.GetTemplate("template/test1." + TMPL_FILE_EXT);

            /*
            * Template to find with the jar loader
            */
            Template template3 = Velocity.GetTemplate("template/test2." + TMPL_FILE_EXT);

            /*
            * and the results files
            */

            //UPGRADE_TODO: 构造函数“java.io.FileOutputStream.FileOutputStream”被转换为具有不同行为的 'System.IO.FileStream.FileStream'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
            System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(RESULTS_DIR, "path1", RESULT_FILE_EXT), System.IO.FileMode.Create);

            //UPGRADE_TODO: 构造函数“java.io.FileOutputStream.FileOutputStream”被转换为具有不同行为的 'System.IO.FileStream.FileStream'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
            System.IO.FileStream fos2 = new System.IO.FileStream(getFileName(RESULTS_DIR, "test2", RESULT_FILE_EXT), System.IO.FileMode.Create);

            //UPGRADE_TODO: 构造函数“java.io.FileOutputStream.FileOutputStream”被转换为具有不同行为的 'System.IO.FileStream.FileStream'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
            System.IO.FileStream fos3 = new System.IO.FileStream(getFileName(RESULTS_DIR, "test3", RESULT_FILE_EXT), System.IO.FileMode.Create);

            //UPGRADE_ISSUE: “java.io.Writer”和“System.IO.StreamWriter”之间的类层次结构差异可能导致编译错误。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_WARNING: 在目标代码中，至少有一个表达式被使用了多次。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);
            //UPGRADE_ISSUE: “java.io.Writer”和“System.IO.StreamWriter”之间的类层次结构差异可能导致编译错误。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_WARNING: 在目标代码中，至少有一个表达式被使用了多次。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamWriter writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).Encoding);
            //UPGRADE_ISSUE: “java.io.Writer”和“System.IO.StreamWriter”之间的类层次结构差异可能导致编译错误。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_WARNING: 在目标代码中，至少有一个表达式被使用了多次。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamWriter writer3 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos3, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos3, System.Text.Encoding.Default).Encoding);

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

            template3.Merge(context, writer3);
            writer3.Flush();
            writer3.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "path1", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect for FileResourceLoader test.");
            }

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test2", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect for ClasspathResourceLoader test.");
            }

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test3", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect for JarResourceLoader test.");
            }
        }
    }
}
