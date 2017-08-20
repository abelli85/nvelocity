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
    using Exception;

    /// <summary> Test parser exception is generated with appropriate Info.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ParseExceptionTestCase.java 576946 2007-09-18 15:18:12Z wglass $
    /// </version>
    [TestFixture]
    public class ParseExceptionTestCase : BaseTestCase
    {
        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private const string FILE_RESOURCE_LOADER_PATH = "test/parseexception";

        /// <summary> Tests that parseException has useful Info when called by template.marge()</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionFromTemplate()
        {

            VelocityEngine ve = new VelocityEngine();

            ve.SetProperty("file.resource.loader.cache", "true");
            ve.SetProperty("file.resource.loader.path", FILE_RESOURCE_LOADER_PATH);
            ve.Init();


       
            System.IO.TextWriter writer = new System.IO.StringWriter();

            VelocityContext context = new VelocityContext();

            try
            {
                Template template = ve.GetTemplate("badtemplate.vm");
                template.Merge(context, writer);
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("badtemplate.vm", e.TemplateName);
                Assert.AreEqual(5, e.LineNumber);
                Assert.AreEqual(9, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary> Tests that parseException has useful Info when thrown in VelocityEngine.Evaluate()</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionFromEval()
        {

            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, writer, "test", "   #set($abc)   ");
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("test", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(13, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary> Tests that parseException has useful Info when thrown in VelocityEngine.Evaluate()
        /// and the problem comes from a macro definition
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionFromMacroDef()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, writer, "testMacro", "#macro($blarg) foo #end");
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("testMacro", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(7, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary> Tests that parseException has useful Info when thrown in VelocityEngine.Evaluate()
        /// and the problem comes from a macro definition
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionFromMacroDefBody()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, writer, "testMacro", "#macro(aa $blarg) #set(!! = bb) #end #aa('aa')");
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("testMacro", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(24, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        /// <summary> Tests that parseException has useful Info when thrown in VelocityEngine.Evaluate()
        /// and the problem comes from a macro invocation
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionFromMacroInvoke()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, writer, "testMacroInvoke", "#macro(   foo $a) $a #end #foo(woogie)");
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("testMacroInvoke", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(31, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }


        /// <summary> Tests that parseException has useful Info with macro calls with
        /// invalid number of arguments
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionMacroInvalidArgumentCount()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty("velocimacro.arguments.strict", "true");
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, writer, "testMacroInvoke", "#macro(foo $a) $a #end #foo('test1' 'test2')");
                Assert.Fail("Should have thown a ParseErrorException");
            }
            catch (ParseErrorException e)
            {
                Assert.AreEqual("testMacroInvoke", e.TemplateName);
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(24, e.ColumnNumber);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }


        /// <summary> Tests that parseException has useful Info with macro calls with
        /// invalid number of arguments
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseExceptionMacroInvalidArgumentCountNoException()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            VelocityContext context = new VelocityContext();

            System.IO.TextWriter writer = new System.IO.StringWriter();

            // will not throw an exception
            try
            {
                ve.Evaluate(context, writer, "testMacroInvoke", "#macro(foo $a) $a #end #foo('test1' 'test2')");
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
