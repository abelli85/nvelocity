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
    using App;
    using App.Event;
    using App.Event.Implement;
    using Context;
    using NUnit.Framework;
    using Runtime;

    /// <summary> Tests the operation of the built in event handlers.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: BuiltInEventHandlerTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class BuiltInEventHandlerTestCase : BaseTestCase
    {

        protected internal bool DEBUG = false;

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

       
        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);
        }


        protected internal virtual void log(string out_Renamed)
        {
            if (DEBUG)
            {
                System.Console.Out.WriteLine(out_Renamed);
            }
        }

        /// <summary> Test reporting of invalid syntax</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testReportInvalidReferences1()
        {
            VelocityEngine ve = new VelocityEngine();
            ReportInvalidReferences reporter = new ReportInvalidReferences();
            ve.Init();

            VelocityContext context = new VelocityContext();
            EventCartridge ec = new EventCartridge();
            ec.AddEventHandler(reporter);
            ec.AttachToContext(context);

            context.Put("a1", "test");
            context.Put("b1", "test");
          
            System.IO.TextWriter writer = new System.IO.StringWriter();

            ve.Evaluate(context, writer, "test", "$a1 $c1 $a1.Length $a1.foobar()");

            System.Collections.IList errors = reporter.InvalidReferences;
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("$c1", ((InvalidReferenceInfo)errors[0]).InvalidReference);
            Assert.AreEqual("$a1.foobar()", ((InvalidReferenceInfo)errors[1]).InvalidReference);

            log("Caught invalid references (local configuration).");
        }

        [Test]
        public virtual void testReportInvalidReferences2()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty("eventhandler.invalidreference.exception", "true");
            ReportInvalidReferences reporter = new ReportInvalidReferences();
            ve.Init();

            VelocityContext context = new VelocityContext();
            EventCartridge ec = new EventCartridge();
            ec.AddEventHandler(reporter);
            ec.AttachToContext(context);

            context.Put("a1", "test");
            context.Put("b1", "test");
         
            System.IO.TextWriter writer = new System.IO.StringWriter();

            ve.Evaluate(context, writer, "test", "$a1 no problem");

            try
            {
                ve.Evaluate(context, writer, "test", "$a1 $c1 $a1.Length $a1.foobar()");
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException E)
            {
            }


            log("Caught invalid references (global configuration).");
        }

        /// <summary> Test escaping</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEscapeHtml()
        {
            EscapeReference esc = new EscapeHtmlReference();
            Assert.AreEqual("test string&amp;another&lt;b&gt;bold&lt;/b&gt;test", esc.ReferenceInsert("", "test string&another<b>bold</b>test"));
            Assert.AreEqual("&lt;&quot;&gt;", esc.ReferenceInsert("", "<\">"));
            Assert.AreEqual("test string", esc.ReferenceInsert("", "test string"));

            log("Correctly escaped HTML");
        }

        /// <summary> Test escaping</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEscapeXml()
        {
            EscapeReference esc = new EscapeXmlReference();
            Assert.AreEqual("test string&amp;another&lt;b&gt;bold&lt;/b&gt;test", esc.ReferenceInsert("", "test string&another<b>bold</b>test"));
            Assert.AreEqual("&lt;&quot;&gt;", esc.ReferenceInsert("", "<\">"));
            Assert.AreEqual("&apos;", esc.ReferenceInsert("", "'"));
            Assert.AreEqual("test string", esc.ReferenceInsert("", "test string"));

            log("Correctly escaped XML");
        }

        /// <summary> Test escaping</summary>
        /// <throws>  Exception </throws>
       [Test]
        public virtual void testEscapeSql()
        {
            EscapeReference esc = new EscapeSqlReference();
            Assert.AreEqual("Jimmy''s Pizza", esc.ReferenceInsert("", "Jimmy's Pizza"));
            Assert.AreEqual("test string", esc.ReferenceInsert("", "test string"));

            log("Correctly escaped SQL");
        }

        /// <summary> Test escaping</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEscapeJavaScript()
        {
            EscapeReference esc = new EscapeJavaScriptReference();
            Assert.AreEqual("Jimmy\\'s Pizza", esc.ReferenceInsert("", "Jimmy's Pizza"));
            Assert.AreEqual("test string", esc.ReferenceInsert("", "test string"));


            log("Correctly escaped Javascript");
        }

        /// <summary> test that escape reference handler works with no match restrictions</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEscapeReferenceMatchAll()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, "NVelocity.App.Event.Implement.EscapeHtmlReference");
            ve.Init();

            IContext context;
         
            System.IO.TextWriter writer;

            // test normal reference
            context = new VelocityContext();
            writer = new System.IO.StringWriter();
            context.Put("bold", "<b>");
            ve.Evaluate(context, writer, "test", "$bold test & test");
         
            Assert.AreEqual("&lt;b&gt; test & test", writer.ToString());

            // test method reference
            context = new VelocityContext();
            writer = new System.IO.StringWriter();
            context.Put("bold", "<b>");
            ve.Evaluate(context, writer, "test", "$bold.substring(0,1)");
        
            Assert.AreEqual("&lt;", writer.ToString());

            log("Escape matched all references (global configuration)");
        }

        /// <summary> test that escape reference handler works with match restrictions</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testEscapeReferenceMatch()
        {
            // set up HTML match on everything, JavaScript match on _js*
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, "NVelocity.App.Event.Implement.EscapeHtmlReference,NVelocity.App.Event.Implement.EscapeJavaScriptReference");
            ve.SetProperty("eventhandler.escape.javascript.match", "/.*_js.*/");
            ve.Init();

            System.IO.TextWriter writer;

            // Html no JavaScript
            writer = new System.IO.StringWriter();
            ve.Evaluate(newEscapeContext(), writer, "test", "$test1");
          
            Assert.AreEqual("Jimmy's &lt;b&gt;pizza&lt;/b&gt;", writer.ToString());

            // comment out bad test -- requires latest commons-lang
            /**
			
            // JavaScript and HTML
            textWriter = new StringWriter();
            ve.Evaluate(newEscapeContext(),textWriter,"test","$test1_js");
            Assert.AreEqual("Jimmy\\'s &lt;b&gt;pizza&lt;/b&gt;",textWriter.toString());
			
            // JavaScript and HTML
            textWriter = new StringWriter();
            ve.Evaluate(newEscapeContext(),textWriter,"test","$test1_js_test");
            Assert.AreEqual("Jimmy\\'s &lt;b&gt;pizza&lt;/b&gt;",textWriter.toString());
			
            // JavaScript and HTML (method call)
            textWriter = new StringWriter();
            ve.Evaluate(newEscapeContext(),textWriter,"test","$test1_js.substring(0,7)");
            Assert.AreEqual("Jimmy\\'s",textWriter.toString());
			
            **/

            log("Escape selected references (global configuration)");
        }

        private IContext newEscapeContext()
        {
            IContext context = new VelocityContext();
            context.Put("test1", "Jimmy's <b>pizza</b>");
            context.Put("test1_js", "Jimmy's <b>pizza</b>");
            context.Put("test1_js_test", "Jimmy's <b>pizza</b>");
            return context;
        }

        [Test]
        public virtual void testPrintExceptionHandler()
        {
            VelocityEngine ve1 = new VelocityEngine();
            ve1.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, "NVelocity.App.Event.Implement.PrintExceptions");
            ve1.Init();

            VelocityEngine ve2 = new VelocityEngine();
            ve2.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, "NVelocity.App.Event.Implement.PrintExceptions");
            ve2.SetProperty("eventhandler.methodexception.message", "true");
            ve2.Init();

            VelocityEngine ve3 = new VelocityEngine();
            ve3.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, "NVelocity.App.Event.Implement.PrintExceptions");
            ve3.SetProperty("eventhandler.methodexception.stacktrace", "true");
            ve3.Init();

            IContext context;
            System.IO.StringWriter writer;

            context = new VelocityContext();
            context.Put("list", new System.Collections.ArrayList());

            // exception only
            writer = new System.IO.StringWriter();
            ve1.Evaluate(context, writer, "test", "$list.get_Item(0)");
            Assert.IsTrue(writer.ToString().IndexOf("IndexOutOfBoundsException") != -1);
            Assert.IsTrue(writer.ToString().IndexOf("Index: 0, Size: 0") == -1);
            Assert.IsTrue(writer.ToString().IndexOf("ArrayList") == -1);

            // message
            writer = new System.IO.StringWriter();
            ve2.Evaluate(context, writer, "test", "$list.get_Item(0)");
            Assert.IsTrue(writer.ToString().IndexOf("IndexOutOfBoundsException") != -1);
            Assert.IsTrue(writer.ToString().IndexOf("Index: 0, Size: 0") != -1);
            Assert.IsTrue(writer.ToString().IndexOf("ArrayList") == -1);

            // stack Trace
            writer = new System.IO.StringWriter();
            ve3.Evaluate(context, writer, "test", "$list.get_Item(0)");
            Assert.IsTrue(writer.ToString().IndexOf("IndexOutOfBoundsException") != -1);
            Assert.IsTrue(writer.ToString().IndexOf("ArrayList") != -1);

            log("PrintException handler successful.");
        }

        [Test]
        public virtual void testIncludeNotFound()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_INCLUDE, "NVelocity.App.Event.Implement.IncludeNotFound");
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            Template template;
            System.IO.FileStream fos;
        
            System.IO.StreamWriter fwriter;
            IContext context;

            template = ve.GetTemplate(getFileName(null, "test6", TMPL_FILE_EXT));

            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "test6", RESULT_FILE_EXT), System.IO.FileMode.Create);

            fwriter = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            context = new VelocityContext();
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test6", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }

            log("IncludeNotFound handler successful.");
        }

        [Test]
        public virtual void testIncludeRelativePath()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_INCLUDE, "NVelocity.App.Event.Implement.IncludeRelativePath");
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            Template template;
            System.IO.FileStream fos;
           
            System.IO.StreamWriter fwriter;
            IContext context;

            template = ve.GetTemplate(getFileName(null, "subdir/test2", TMPL_FILE_EXT));

          
            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, "test2", RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            fwriter = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            context = new VelocityContext();
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "test2", RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }

            log("IncludeRelativePath handler successful.");
        }
    }
}
