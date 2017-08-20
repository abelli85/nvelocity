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

    using Runtime;
    using NVelocity.Util.Introspection;

    /// <summary>  Simple introspector test case for primitive problem found in 1.3
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@apache.org">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: Introspector3TestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class Introspector3TestCase : BaseTestCase
    {
        [Test]
        public virtual void testSimple()
        {
            MethodEntry method;
            string result;

            MethodProvider mp = new MethodProvider();

            /*
            * string integer
            */

            object[] listIntInt = new object[] { new System.Collections.ArrayList(), 1, 2 };
            object[] listLongList = new object[] { new System.Collections.ArrayList(), 1, new System.Collections.ArrayList() };
            object[] intInt = new object[] { 1, 2 };
            object[] longInt = new object[] { 1, 2 };
            object[] longLong = new object[] { 1, 2 };

            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "lii", listIntInt);
            result = ((string)method.Invoker.Invoke(mp, (object[])listIntInt));

            Assert.IsTrue(result.Equals("lii"));

            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "ii", intInt);
            result = ((string)method.Invoker.Invoke(mp, (object[])intInt));

            Assert.IsTrue(result.Equals("ii"));

            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "ll", longInt);
            result = ((string)method.Invoker.Invoke(mp, (object[])longInt));

            Assert.IsTrue(result.Equals("ll"));

            /*
            * test overloading with primitives
            */

            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "ll", longLong);
            result = ((string)method.Invoker.Invoke(mp, (object[])longLong));

            Assert.IsTrue(result.Equals("ll"));

            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "lll", listLongList);
            result = ((string)method.Invoker.Invoke(mp, (object[])listLongList));

            Assert.IsTrue(result.Equals("lll"));

            /*
            *  test invocation with nulls
            */

            object[] oa = new object[] { null, 0 };
            method = RuntimeSingleton.Introspector.GetMethod(typeof(MethodProvider), "lll", oa);
            result = ((string)method.Invoker.Invoke(mp, (object[])oa));

            Assert.IsTrue(result.Equals("Listl"));
        }

        public class MethodProvider
        {
            public virtual string ii(int p, int d)
            {
                return "ii";
            }

            public virtual string lii(System.Collections.IList s, int p, int d)
            {
                return "lii";
            }

            public virtual string lll(System.Collections.IList s, long p, System.Collections.IList d)
            {
                return "lll";
            }


            public virtual string lll(System.Collections.IList s, long p, int d)
            {
                return "lli";
            }

            public virtual string lll(System.Collections.IList s, long p)
            {
                return "Listl";
            }

            public virtual string ll(long p, long d)
            {
                return "ll";
            }
        }
    }
}
