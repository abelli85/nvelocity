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

namespace NVelocity.Tests.Util.Introspection
{
    using NUnit.Framework;

    using App;
    using NVelocity.Util.Introspection;
    using Runtime;
    using Tests.Misc;

    /// <summary> Tests uberspectors chaining</summary>
    [TestFixture]
    public class ChainedUberspectorsTestCase : BaseTestCase
    {
        [SetUp]
        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);
            Velocity.AddProperty(RuntimeConstants.UBERSPECT_CLASSNAME, "NVelocity.Util.Introspection.UberspectImpl");
            Velocity.AddProperty(RuntimeConstants.UBERSPECT_CLASSNAME, "NVelocity.Tests.Util.Introspection.ChainedUberspector;NVelocity.Tests");
            Velocity.AddProperty(RuntimeConstants.UBERSPECT_CLASSNAME, "NVelocity.Tests.Util.Introspection.LinkedUberspector;NVelocity.Tests");
            Velocity.Init();
        }

        [TearDown]
        public void tearDown()
        {
        }

        [Test]
        public virtual void testChaining()
        {
            VelocityContext context = new VelocityContext();
            context.Put("foo", new Foo());
            System.IO.StringWriter writer = new System.IO.StringWriter();

            Velocity.Evaluate(context, writer, "test", "$foo.zeMethod()");
            Assert.AreEqual(writer.ToString(), "ok");

            Velocity.Evaluate(context, writer, "test", "#set($foo.foo = 'someValue')");

            writer = new System.IO.StringWriter();
            Velocity.Evaluate(context, writer, "test", "$foo.bar");
            Assert.AreEqual(writer.ToString(), "someValue");

            writer = new System.IO.StringWriter();
            Velocity.Evaluate(context, writer, "test", "$foo.foo");
            Assert.AreEqual(writer.ToString(), "someValue");
        }
    }

    // replaces GetFoo by getBar
    public class ChainedUberspector : AbstractChainableUberspector
    {
        public override IVelPropertySet GetPropertySet(object obj, string identifier, object arg, Info info)
        {
            identifier = identifier.Replace("foo", "bar");
            return inner.GetPropertySet(obj, identifier, arg, info);
        }
    }

    // replaces setFoo by setBar
    public class LinkedUberspector : UberspectImpl
    {
        public override IVelPropertyGet GetPropertyGet(object obj, string identifier, Info info)
        {
            identifier = identifier.Replace("foo", "bar");
            return base.GetPropertyGet(obj, identifier, info);
        }
    }

    public class Foo
    {
        virtual public string Bar
        {
            get
            {
                return bar;
            }

            set
            {
                bar = value;
            }

        }
        private string bar;

        public virtual string zeMethod()
        {
            return "ok";
        }
    }
}
