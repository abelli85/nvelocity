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

    using App;
    using Misc;
    using Runtime;
    using NVelocity.Util.Introspection;

    
    /// <summary> Test case for the Velocity Introspector which
    /// tests the ability to find a 'best match'
    /// 
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@apache.org">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: Introspector2TestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class Introspector2TestCase : BaseTestCase
    {
        [Test]
        public virtual void testIntrospector()
        {
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();

            MethodEntry method;
            string result;
            Tester t = new Tester();

            object[] params_Renamed = new object[] { new Foo(), new Foo() };

            method = RuntimeSingleton.Introspector.GetMethod(typeof(Tester), "find", params_Renamed);

            if (method == null)
                Assert.Fail("Returned method was null");

            result = ((string)method.Invoker.Invoke(t, (object[])params_Renamed));

            if (!result.Equals("Bar-Bar"))
            {
                Assert.Fail("Should have gotten 'Bar-Bar' : received '" + result + "'");
            }

            /*
            *  now test for failure due to ambiguity
            */

            method = RuntimeSingleton.Introspector.GetMethod(typeof(Tester2), "find", params_Renamed);

            if (method != null)
                Assert.Fail("Introspector shouldn't have found a method as it's ambiguous.");
        }

        public interface Woogie
        {
        }

        public class Bar : Introspector2TestCase.Woogie
        {
            internal int i;
        }

        public class Foo : Bar
        {
            internal int j;
        }

        public class Tester
        {
            public static string find(Introspector2TestCase.Woogie w, object o)
            {
                return "Woogie-Object";
            }

            public static string find(object w, Bar o)
            {
                return "Object-Bar";
            }

            public static string find(Bar w, Bar o)
            {
                return "Bar-Bar";
            }

            public static string find(object o)
            {
                return "Object";
            }

            public static string find(Introspector2TestCase.Woogie o)
            {
                return "Woogie";
            }
        }

        public class Tester2
        {
            public static string find(Introspector2TestCase.Woogie w, object o)
            {
                return "Woogie-Object";
            }

            public static string find(object w, Bar o)
            {
                return "Object-Bar";
            }

            public static string find(Bar w, object o)
            {
                return "Bar-Object";
            }

            public static string find(object o)
            {
                return "Object";
            }

            public static string find(Introspector2TestCase.Woogie o)
            {
                return "Woogie";
            }
        }
    }
}
