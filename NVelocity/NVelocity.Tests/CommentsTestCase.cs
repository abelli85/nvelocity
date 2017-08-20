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

    /// <summary> Test comments
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: CommentsTestCase.java 569256 2007-08-24 05:41:08Z wglass $
    /// </version>
    [TestFixture]
    public class CommentsTestCase : BaseTestCase
    {

        /// <summary> Test multiline comments</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testMultiLine()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "abc #* test\r\ntest2*#\r\ndef");
            Assert.AreEqual("abc \r\ndef", writer.ToString());
        }

        /// <summary> Test single line comments</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testSingleLine()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "123 ## test test\r\nabc");
            Assert.AreEqual("123 abc", writer.ToString());

            context = new VelocityContext();
            writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "123 \r\n## test test\r\nabc");
            Assert.AreEqual("123 \r\nabc", writer.ToString());
        }

        /// <summary> Test combined comments</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testCombined()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            IContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(context, writer, "test", "test\r\n## #* *# ${user \r\nabc");
            Assert.AreEqual("test\r\nabc", writer.ToString());
        }
    }
}
