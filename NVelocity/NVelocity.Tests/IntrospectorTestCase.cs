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
    using System.Reflection;
    using NUnit.Framework;

    using Runtime;
    using NVelocity.Util.Introspection;


    /// <summary> Test case for the Velocity Introspector which uses
    /// the Java Reflection API to determine the correct
    /// signature of the methods used in VTL templates.
    /// 
    /// This should be split into separate tests for each
    /// of the methods searched for but this is a start
    /// for now.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <version>  $Id: IntrospectorTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class IntrospectorTestCase : BaseTestCase
    {
        private static MethodProvider mp;

        [SetUp]
        public void setUp()
        {
            mp = new MethodProvider();
        }

        [Test]
        public virtual void testIntrospectorBoolean()
        {
            // Test boolean primitive.
            object[] booleanParams = new object[] { true };
            string type = "boolean";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", booleanParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])booleanParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorByte()
        {
            // Test byte primitive.
            object[] byteParams = new object[] { System.SByte.Parse("1") };
            string type = "byte";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", byteParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])byteParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorChar()
        {
            // Test char primitive.
            object[] characterParams = new object[] { 'a' };
            string type = "character";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", characterParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])characterParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorDouble()
        {

            // Test double primitive.
            object[] doubleParams = new object[] { (double)1 };
            string type = "double";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", doubleParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])doubleParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorFloat()
        {

            // Test float primitive.
            object[] floatParams = new object[] { (float)1 };
            string type = "float";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", floatParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])floatParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorInteger()
        {

            // Test integer primitive.
            object[] integerParams = new object[] { (System.Int32)1 };
            string type = "integer";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", integerParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])integerParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorPrimitiveLong()
        {

            // Test long primitive.
            object[] longParams = new object[] { (long)1 };
            string type = "long";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", longParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])longParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorPrimitiveShort()
        {
            // Test short primitive.
            object[] shortParams = new object[] { (short)1 };
            string type = "short";
            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), type + "Method", shortParams);
            string result = (string)method.Invoker.Invoke(mp, (object[])shortParams);

            Assert.AreEqual("Method could not be found", type, result);
        }

        [Test]
        public virtual void testIntrospectorUntouchable()
        {
            // Test untouchable

            object[] params_Renamed = new object[] { };

            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "untouchable", params_Renamed);

            Assert.IsNull(method, "able to access a private-access method.");
        }

        [Test]
        public virtual void testIntrospectorReallyUntouchable()
        {
            // Test really untouchable
            object[] params_Renamed = new object[] { };

            MethodEntry method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "reallyuntouchable", params_Renamed);

            Assert.IsNull(method, "able to access a private-access method.");
        }

        public class MethodProvider
        {
            /*
            * Methods with native parameter types.
            */
            public virtual string booleanMethod(bool p)
            {
                return "boolean";
            }
            public virtual string byteMethod(sbyte p)
            {
                return "byte";
            }
            public virtual string characterMethod(char p)
            {
                return "character";
            }
            public virtual string doubleMethod(double p)
            {
                return "double";
            }
            public virtual string floatMethod(float p)
            {
                return "float";
            }
            public virtual string integerMethod(int p)
            {
                return "integer";
            }
            public virtual string longMethod(long p)
            {
                return "long";
            }
            public virtual string shortMethod(short p)
            {
                return "short";
            }

            internal virtual string untouchable()
            {
                return "yech";
            }
            // don't remove! Used through introspection for testing!
            private string reallyuntouchable()
            {
                return "yech!";
            }
        }
    }
}
