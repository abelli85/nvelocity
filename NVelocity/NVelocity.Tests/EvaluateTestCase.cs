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
    using App.Event.Implement;
    using Context;
    using Exception;
    using Runtime;

    /// <summary> Test #Evaluate directive.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: EvaluateTestCase.java 685287 2008-08-12 20:06:31Z nbubna $
    /// </version>
    [TestFixture]
    public class EvaluateTestCase : BaseTestCase
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
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/evaluate";

       
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/evaluate";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/evaluate/Compare";

        [SetUp]
        public  void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);
        }


        /// <summary> Test basic functionality.</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEvaluate()
        {
            testFile("eval1", new System.Collections.Hashtable());
        }

        /// <summary> Test Evaluate directive preserves macros (VELOCITY-591)</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEvaluateMacroPreserve()
        {
            System.Collections.IDictionary properties = new System.Collections.Hashtable();
            properties.Clear();
            properties[RuntimeConstants.VM_CONTEXT_LOCALSCOPE] = "false";
            testFile("eval2", properties);

            properties.Clear();
            properties[RuntimeConstants.VM_CONTEXT_LOCALSCOPE] = "true";
            testFile("eval2", properties);

            properties.Clear();
            properties[RuntimeConstants.VM_PERM_ALLOW_INLINE_REPLACE_GLOBAL] = "false";
            testFile("eval2", properties);
        }

        /// <summary> Test in a macro context.</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEvaluateVMContext()
        {
            testFile("evalvmcontext", new System.Collections.Hashtable());
        }

        /// <summary> Test #stop (since it is attached to context).</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testStop()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "test #stop test2 #evaluate('test3')");
            Assert.AreEqual("test ", writer.ToString());

            context = new VelocityContext();
            writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "test test2 #evaluate('test3 #stop test4') test5");
            Assert.AreEqual("test test2 test3  test5", writer.ToString());
        }

        /// <summary> Test that the event handlers work in #Evaluate (since they are
        /// attached to the context).  Only need to check one - they all 
        /// work the same.
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEventHandler()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, typeof(EscapeHtmlReference).FullName);
            ve.Init();

            IContext context = new VelocityContext();
            context.Put("lt", "<");
            context.Put("gt", ">");
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "${lt}test${gt} #evaluate('${lt}test2${gt}')");
            Assert.AreEqual("&lt;test&gt; &lt;test2&gt;", writer.ToString());
        }


        /// <summary> Test errors are thrown</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testErrors()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            IContext context = new VelocityContext();

            // no arguments
            System.IO.StringWriter writer = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, writer, "test", "#evaluate()");
                Assert.Fail("Expected exception");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("test", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(1, e.ColumnNumber);
            }

            // too many arguments
            writer = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, writer, "test", "#evaluate('aaa' 'bbb')");
                Assert.Fail("Expected exception");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("test", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(17, e.ColumnNumber);
            }

            // argument not a string or reference
            writer = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, writer, "test", "#evaluate(10)");
                Assert.Fail("Expected exception");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("test", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(11, e.ColumnNumber);
            }

            // checking line/col for parse Error
            writer = new System.IO.StringWriter();
            try
            {
                string eval = "this is a multiline\n\n\n\n\n test #foreach() with an error";
                context.Put("eval", eval);
                ve.Evaluate(context, writer, "test", "first line\n second line: #evaluate($eval)");
                Assert.Fail("Expected exception");
            }
            catch (ParseErrorException e)
            {
                // should be start of #Evaluate
                Assert.AreEqual("test", e.TemplateName);
                Assert.AreEqual(2, e.LineNumber);
                Assert.AreEqual(15, e.ColumnNumber);
            }
        }

        /// <summary> Test a file parses with no errors and Compare to existing file.</summary>
        /// <param name="basefilename">
        /// </param>
        /// <throws>  Exception </throws>
        private void testFile(string basefilename, System.Collections.IDictionary properties)
        {
            VelocityEngine ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);

    
            for (System.Collections.IEnumerator i = properties.Keys.GetEnumerator(); i.MoveNext(); )
            {
                string key = (string)i.Current;
                string value_Renamed = (string)properties[key];
                ve.AddProperty(key, value_Renamed);
            }

            ve.Init();

            Template template;
            System.IO.FileStream fos;
          
            System.IO.StreamWriter fwriter;
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
                string result = getFileContents(RESULTS_DIR, basefilename, RESULT_FILE_EXT, System.Text.Encoding.Default);
                string compare = getFileContents(COMPARE_DIR, basefilename, CMP_FILE_EXT, System.Text.Encoding.Default);

                string msg = "Output was incorrect\n" + "-----Result-----\n" + result + "----Expected----\n" + compare + "----------------";

                Assert.Fail(msg);
            }
        }

        public virtual void setupContext(IContext context)
        {
        }
    }
}
