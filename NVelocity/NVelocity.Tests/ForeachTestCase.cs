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
    using Provider;
    using Runtime;

    /// <summary> This class tests the Foreach loop.
    /// 
    /// </summary>
    /// <author>  Daniel Rall
    /// </author>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    [TestFixture]
    public class ForeachTestCase
    {
        private VelocityContext context;

        [SetUp]
        public void setUp()
        {
            // Limit the loop to three iterations.
            Velocity.SetProperty(RuntimeConstants.MAX_NUMBER_LOOPS, (object)3);

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName + ";" + "NVelocity.Tests");

            Velocity.Init();

            context = new VelocityContext();
        }

        /// <summary> Tests limiting of the number of loop iterations.</summary>
        [Test]
        public virtual void testMaxNbrLoopsConstraint()
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            string template = "#foreach ($item in [1..10])$item #end";
            Velocity.Evaluate(context, writer, "test", template);
            Assert.AreEqual("1 2 3 ", writer.ToString(), "Max number loops not enforced");
        }

        /// <summary> Tests proper method execution during a Foreach loop over a Collection
        /// with items of varying classes.
        /// </summary>
        [Test]
        public virtual void testCollectionAndMethodCall()
        {
            System.Collections.IList col = new System.Collections.ArrayList();
            col.Add(100);
            col.Add("STRVALUE");
            context.Put("helper", new ForeachMethodCallHelper());
            context.Put("col", col);

            System.IO.StringWriter writer = new System.IO.StringWriter();
            Velocity.Evaluate(context, writer, "test", "#foreach ( $item in $col )$helper.getFoo($item) " + "#end");
            Assert.AreEqual("int 100 str STRVALUE ", writer.ToString(), "Method calls while looping over varying classes failed");
        }

        /// <summary> Tests that #foreach will be able to retrieve an iterator from
        /// an arbitrary object that happens to have an iterator() method.
        /// (With the side effect of supporting the new Java 5 Iterable interface)
        /// </summary>
        [Test]
        public virtual void testObjectWithIteratorMethod()
        {
            context.Put("iterable", new MyIterable());

            System.IO.StringWriter writer = new System.IO.StringWriter();
            string template = "#foreach ($i in $iterable)$i #end";
            Velocity.Evaluate(context, writer, "test", template);
            Assert.AreEqual("1 2 3 ", writer.ToString(), "Failed to call iterator() method");
        }

        [Test]
        public virtual void testNotReallyIterableIteratorMethod()
        {
            context.Put("nri", new NotReallyIterable());

            System.IO.StringWriter writer = new System.IO.StringWriter();
            string template = "#foreach ($i in $nri)$i #end";
            Velocity.Evaluate(context, writer, "test", template);
            Assert.AreEqual("", writer.ToString());
        }


        public class MyIterable
        {
            private System.Collections.IList foo;

            public MyIterable()
            {
                foo = new System.Collections.ArrayList();
                foo.Add(1);
                foo.Add(2);
                foo.Add("3");
            }

            public virtual System.Collections.IEnumerator iterator()
            {
                return foo.GetEnumerator();
            }
        }

        public class NotReallyIterable
        {
            public virtual object iterator()
            {
                return new object();
            }
        }
    }
}
