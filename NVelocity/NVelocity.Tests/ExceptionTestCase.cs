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
    using Misc;
    using Provider;
    using Runtime;

    /// <summary> Test case for miscellaneous Exception related issues.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ExceptionTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class ExceptionTestCase : BaseTestCase
    {
        internal VelocityEngine ve;

        [Test]
        public virtual void testReferenceInsertionEventHandlerException()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, typeof(ExceptionGeneratingEventHandler).FullName);
            ve.Init();
            assertException(ve);
        }

        /// <summary> Note - this is the one case where RuntimeExceptions *are not* passed through
        /// verbatim.
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testMethodExceptionEventHandlerException()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, typeof(ExceptionGeneratingEventHandler).FullName);
            ve.Init();
            IContext context = new VelocityContext();
            context.Put("test", new TestProvider());
            assertMethodInvocationException(ve, context, "$test.getThrow()");
            assertMethodInvocationException(ve, context, "$test.throw");
        }

        [Test]
        public virtual void testNullSetEventHandlerException()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_NULLSET, typeof(ExceptionGeneratingEventHandler).FullName);
            ve.Init();
            assertException(ve, "#set($test = $abc)");
        }

        [Test]
        public virtual void testIncludeEventHandlerException()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_INCLUDE, typeof(ExceptionGeneratingEventHandler).FullName);
            ve.Init();
            assertException(ve, "#include('dummy')");
        }

        [Test]
        public virtual void testResourceLoaderException()
        {
            ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.RESOURCE_LOADER, "except");
            ve.SetProperty("except.resource.loader.class", typeof(ExceptionGeneratingResourceLoader).FullName);
            try
            {
                ve.Init(); // tries to Get the macro file
                ve.GetTemplate("test.txt");
                Assert.Fail("Should have thrown RuntimeException");
            }
            catch (System.SystemException E)
            {
                // do nothing
            }
        }

        [Test]
        public virtual void testDirectiveException()
        {
            ve = new VelocityEngine();
            ve.SetProperty("userdirective", typeof(ExceptionGeneratingDirective).FullName);
            ve.Init();
            assertException(ve, "#Exception() test #end");
        }


        [Test]
        public virtual void assertException(VelocityEngine ve)
        {
            IContext context = new VelocityContext();
            context.Put("test", "test");
            assertException(ve, context, "this is a $test");
        }

        [Test]
        public virtual void assertException(VelocityEngine ve, string input)
        {
            IContext context = new VelocityContext();
            context.Put("test", "test");
            assertException(ve, context, input);
        }

        [Test]
        public virtual void assertException(VelocityEngine ve, IContext context, string input)
        {
            try
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                ve.Evaluate(context, writer, "test", input);
                Assert.Fail("Expected RuntimeException");
            }
            catch (System.SystemException E)
            {
                // do nothing
            }
        }
        [Test]
        public virtual void assertMethodInvocationException(VelocityEngine ve, IContext context, string input)
        {
            try
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                ve.Evaluate(context, writer, "test", input);
                Assert.Fail("Expected MethodInvocationException");
            }
            catch (MethodInvocationException E)
            {
                // do nothing
            }
        }
    }
}
