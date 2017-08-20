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
    using Misc;
    using NVelocity.Util.Introspection;
    using Runtime;
    using Runtime.Log;

    /// <summary> Test the ClassMap Lookup</summary>
    [TestFixture]
    public class ClassMapTestCase : BaseTestCase
    {

        [SetUp]
        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, "NVelocity.Tests.Util.Introspection.ClassMapTestCase:NVelocity.Tests");

            Velocity.Init();
        }

        [TearDown]
        public void tearDown()
        {

        }

        [Test]
        public virtual void testPrimitives()
        {
            Log log = Velocity.Log;

            ClassMap c = new ClassMap(typeof(TestClassMap), log);
            Assert.IsNotNull(c.FindMethod("set_Boolean", new object[] { true }));
            Assert.IsNotNull(c.FindMethod("set_Byte", new object[] { (byte)4 }));
            Assert.IsNotNull(c.FindMethod("set_Character", new object[] { 'c' }));
            Assert.IsNotNull(c.FindMethod("set_Double", new object[] { (double)8.0 }));
            Assert.IsNotNull(c.FindMethod("set_Float", new object[] { (float)15.0 }));
            Assert.IsNotNull(c.FindMethod("set_Integer", new object[] { 16 }));
            Assert.IsNotNull(c.FindMethod("set_Long", new object[] { 23 }));
            Assert.IsNotNull(c.FindMethod("set_Short", new object[] { (short)42 }));
        }


        public sealed class TestClassMap
        {
            public bool Boolean { get; set; }

            public byte Byte { get; set; }

            public char Character { get; set; }

            public double Double { get; set; }

            public float Float { get; set; }

            public int Integer { get; set; }

            public long Long { get; set; }

            public short Short { get; set; }
        }
    }
}
