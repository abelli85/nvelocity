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

    /// <summary> Tests input encoding handling.  The input target is UTF-8, having
    /// chinese and and a spanish enyay (n-twiddle)
    /// 
    /// Thanks to Kent Johnson for the example input file.
    /// 
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: EncodingTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class EncodingTestCase : BaseTestCase
    {
      
        [SetUp]
        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH,TemplateTestBase_Fields.FILE_RESOURCE_LOADER_PATH);

            Velocity.SetProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testChineseEncoding()
        {
            VelocityContext context = new VelocityContext();

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            *  Get the template and the output
            */

            /*
            *  Chinese and spanish
            */

            Template template = Velocity.GetTemplate(getFileName(null, "encodingtest", TemplateTestBase_Fields.TMPL_FILE_EXT), "UTF-8");

          
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "encodingtest", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);


            System.IO.StreamWriter writer = new System.IO.StreamWriter(fos, System.Text.Encoding.GetEncoding("UTF-8"));

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR,TemplateTestBase_Fields.COMPARE_DIR, "encodingtest", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.UTF8))
            {
                Assert.Fail("Output 1 incorrect.");
            }
        }

        [Test]
        public virtual void testHighByteChinese()
        {
            VelocityContext context = new VelocityContext();

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            *  a 'high-byte' chinese example from Michael Zhou
            */

            Template template = Velocity.GetTemplate(getFileName(null, "encodingtest2", TemplateTestBase_Fields.TMPL_FILE_EXT), "UTF-8");

         
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "encodingtest2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fos,System.Text.Encoding.UTF8);

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "encodingtest2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.UTF8))
            {
                Assert.Fail("Output 2 incorrect.");
            }
        }

        [Test]
        public virtual void testHighByteChinese2()
        {
            VelocityContext context = new VelocityContext();

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            *  a 'high-byte' chinese from Ilkka
            */

            Template template = Velocity.GetTemplate(getFileName(null, "encodingtest3", TemplateTestBase_Fields.TMPL_FILE_EXT), "GBK");

         
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "encodingtest3", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.GetEncoding("GBK")).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.GetEncoding("GBK")).Encoding);

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "encodingtest3", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.GetEncoding("GBK")))
            {
                Assert.Fail("Output 3 incorrect.");
            }
        }

        [Test]
        public virtual void testRussian()
        {
            VelocityContext context = new VelocityContext();

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            *  Russian example from Vitaly Repetenko
            */

            Template template = Velocity.GetTemplate(getFileName(null, "encodingtest_KOI8-R",TemplateTestBase_Fields.TMPL_FILE_EXT), "KOI8-R");

         
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "encodingtest_KOI8-R", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);


            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.GetEncoding("KOI8-R")).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.GetEncoding("KOI8-R")).Encoding);

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "encodingtest_KOI8-R", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.GetEncoding("KOI8-R")))
            {
                Assert.Fail("Output 4 incorrect.");
            }
        }
    }
}
