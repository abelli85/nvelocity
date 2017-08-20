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
    using System.IO;

    using NUnit.Framework;

    using App;
    using Misc;
    using Runtime;

    /// <summary> Tests if we are context safe : can we switch objects in the context
    /// and re-merge the template safely.
    /// 
    /// NOTE:
    /// This class should not extend RuntimeTestCase because this test
    /// is run from the VelocityTestSuite which in effect a runtime
    /// test suite and the test suite initializes the Runtime. Extending
    /// RuntimeTestCase causes the Runtime to be initialized twice.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: ContextSafetyTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class ContextSafetyTestCase : BaseTestCase
    {
        [SetUp]
        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateTestBase_Fields.FILE_RESOURCE_LOADER_PATH));

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testContextSafety()
        {
            /*
            *  make a Vector and String array because
            *  they are treated differently in Foreach()
            */
            System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            v.Add(new System.Text.StringBuilder("vector hello 1").ToString());
            v.Add(new System.Text.StringBuilder("vector hello 2").ToString());
            v.Add(new System.Text.StringBuilder("vector hello 3").ToString());

            string[] strArray = new string[3];

            strArray[0] = "array hello 1";
            strArray[1] = "array hello 2";
            strArray[2] = "array hello 3";

            VelocityContext context = new VelocityContext();

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            *  Get the template and the output
            */

            Template template = RuntimeSingleton.GetTemplate(getFileName(null, "context_safety", TemplateTestBase_Fields.TMPL_FILE_EXT));

        
            System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "context_safety1", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            System.IO.FileStream fos2 = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "context_safety2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

         
            System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);
          
            System.IO.StreamWriter writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).Encoding);

            /*
            *  Put the Vector into the context, and merge
            */

            context.Put("vector", v);
            template.Merge(context, writer1);
            writer1.Flush();
            writer1.Close();

            /*
            *  now Put the string array into the context, and merge
            */

            context.Put("vector", strArray);
            template.Merge(context, writer2);
            writer2.Flush();
            writer2.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "context_safety1", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default) || !isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "context_safety2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }
    }
}
