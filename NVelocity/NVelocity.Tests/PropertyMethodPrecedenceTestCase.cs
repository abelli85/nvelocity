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

    /// <summary> Used to check that vararg method calls on references work properly</summary>
    [TestFixture]
    public class PropertyMethodPrecedenceTestCase : BaseEvalTestCase
    {
        override protected internal VelocityContext Context
        {
            set
            {
                value.Put("geta", new getGetgetisTool());
                value.Put("getA", new GetgetisTool());
                value.Put("geta2", new get2getisTool());
                value.Put("get_a", new getisTool());
                value.Put("isA", new isTool());
            }

        }
      
        [Test]
        public virtual void testLowercasePropertyMethods()
        {
            assertEvalEquals("getfoo", "$geta.foo");
            assertEvalEquals("getFoo", "$getA.foo");
            assertEvalEquals("Get(foo)", "$get_a.foo");
            assertEvalEquals("true", "$isA.foo");
        }

        [Test]
        public virtual void testUppercasePropertyMethods()
        {
            assertEvalEquals("getFoo", "$geta.Foo");
            assertEvalEquals("getfoo", "$geta2.Foo");
            assertEvalEquals("getFoo", "$getA.Foo");
            assertEvalEquals("Get(Foo)", "$get_a.Foo");
            assertEvalEquals("true", "$isA.Foo");
        }


        public class isTool
        {
            public virtual bool isFoo()
            {
                return true;
            }
        }

        public class getisTool : isTool
        {
            public virtual string get_Renamed(string s)
            {
                return "Get(" + s + ")";
            }
        }

        public class GetgetisTool : getisTool
        {
            public virtual string getFoo()
            {
                return "getFoo";
            }
        }

        public class getGetgetisTool : GetgetisTool
        {
            public virtual string getfoo()
            {
                return "getfoo";
            }
        }

        public class get2getisTool : getisTool
        {
            public virtual string getfoo()
            {
                return "getfoo";
            }
        }
    }
}
