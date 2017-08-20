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
    using NVelocity.Util;

    using Provider;

    /// <summary> Test thrown exceptions include a proper cause (under JDK 1.4+).
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: WrappedExceptionTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class WrappedExceptionTestCase : BaseTestCase
    {
        internal VelocityEngine ve;

        [SetUp]
        public void setUp()
        {
            ve = new VelocityEngine();
            ve.Init();
        }

        [Test]
        public virtual void testMethodException()
        {

            // accumulate a list of invalid references
            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            context.Put("test", new TestProvider());

            try
            {
                ve.Evaluate(context, writer, "test", "$test.getThrow()");
                Assert.Fail("expected an exception");
            }
            catch (MethodInvocationException E)
            {
                Assert.AreEqual(typeof(System.Exception), E.InnerException.GetType());
                Assert.AreEqual("From getThrow()", E.InnerException.Message);
            }
        }
        [Test]
        public virtual void testExceptionUtils()
        {
            System.ApplicationException e = new System.ApplicationException("Inside");
            System.SystemException re = new System.SystemException("Outside", e);
            Assert.AreEqual(e, re.InnerException, "cause was set");
        }
    }
}
