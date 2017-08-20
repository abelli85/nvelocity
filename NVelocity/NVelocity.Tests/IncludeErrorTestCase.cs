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
    using Context;
    using Exception;
    using Runtime;


    /// <summary> Test that #parse and #include pass errors to calling code.
    /// Specifically checking against VELOCITY-95 and VELOCITY-96.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: IncludeErrorTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class IncludeErrorTestCase : BaseTestCase
    {
        internal VelocityEngine ve;

        [SetUp]
        public void setUp()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, "test/includeerror");

            ve.Init();
        }

        [Test]
        public virtual void testMissingParseError()
        {
            checkException("missingparse.vm", typeof(ResourceNotFoundException));
        }

        [Test]
        public virtual void testMissingIncludeError()
        {
            checkException("missinginclude.vm", typeof(ResourceNotFoundException));
        }

        [Test]
        public virtual void testParseError()
        {
            checkException("parsemain.vm", typeof(ParseErrorException));
        }

        [Test]
        public virtual void testParseError2()
        {
            checkException("parsemain2.vm", typeof(ParseErrorException));
        }


        /// <summary> Check that an exception is thrown for the given template</summary>
        /// <param name="templateName">
        /// </param>
        /// <param name="exceptionClass">
        /// </param>
        /// <throws>  Exception </throws>
        private void checkException(string templateName, System.Type exceptionClass)
        {
            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            Template template = ve.GetTemplate(templateName, "UTF-8");

            try
            {
                template.Merge(context, writer);
                writer.Flush();
                Assert.Fail("File should have thrown an exception");
            }
            catch (System.Exception E)
            {
                Assert.IsTrue(exceptionClass.IsAssignableFrom(E.GetType()));
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
