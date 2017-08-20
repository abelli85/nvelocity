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
    using Runtime.Resource.Loader;

    using Misc;

    /// <summary> Multiple paths in the file resource loader.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <version>  $Id: StringResourceLoaderTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class StringResourceLoaderTestCase : BaseTestCase
    {
        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/stringloader";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/stringloader/Compare";

        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            Velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "string");
            Velocity.AddProperty("string.resource.loader.class", typeof(StringResourceLoader).FullName);
            Velocity.AddProperty("string.resource.loader.modificationCheckInterval", "1");

            // Silence the logger.
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        [Test]
        public virtual void testSimpleTemplate()
        {
            StringResourceLoader.GetRepository().PutStringResource("simpletemplate.vm", "This is a test for ${foo}");

            Template template = RuntimeSingleton.GetTemplate(getFileName(null, "simpletemplate", TemplateTestBase_Fields.TMPL_FILE_EXT));

          
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "simpletemplate", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

           
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            VelocityContext context = new VelocityContext();
            context.Put("foo", "a foo object");

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "simpletemplate", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }

        [Test]
        public virtual void testMultipleTemplates()
        {
            StringResourceLoader.GetRepository().PutStringResource("multi1.vm", "I am the $first template.");
            StringResourceLoader.GetRepository().PutStringResource("multi2.vm", "I am the $second template.");

            Template template1 = RuntimeSingleton.GetTemplate(getFileName(null, "multi1", TemplateTestBase_Fields.TMPL_FILE_EXT));

         
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "multi1", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            VelocityContext context = new VelocityContext();
            context.Put("first", (object)1);
            context.Put("second", "two");

            template1.Merge(context, writer);
            writer.Flush();
            writer.Close();

            Template template2 = RuntimeSingleton.GetTemplate(getFileName(null, "multi2", TemplateTestBase_Fields.TMPL_FILE_EXT));

          
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "multi2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

         
            writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            template2.Merge(context, writer);
            writer.Flush();
            writer.Close();



            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "multi1", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Template 1 incorrect.");
            }

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "multi2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Template 2 incorrect.");
            }
        }

        [Test]
        public virtual void testContentChange()
        {
            StringResourceLoader.GetRepository().PutStringResource("change.vm", "I am the $first template.");

            Template template = RuntimeSingleton.GetTemplate(getFileName(null, "change", TemplateTestBase_Fields.TMPL_FILE_EXT));

          
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "change1", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

         
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            VelocityContext context = new VelocityContext();
            context.Put("first", (object)1);
            context.Put("second", "two");

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();

            StringResourceLoader.GetRepository().PutStringResource("change.vm", "I am the $second template.");
          
            System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64)10000 * 2000L));
            template = RuntimeSingleton.GetTemplate(getFileName(null, "change", TemplateTestBase_Fields.TMPL_FILE_EXT));

           
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "change2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            template.Merge(context, writer);
            writer.Flush();
            writer.Close();



            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "change1", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Template 1 incorrect.");
            }

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "change2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Template 2 incorrect.");
            }
        }
    }
}
