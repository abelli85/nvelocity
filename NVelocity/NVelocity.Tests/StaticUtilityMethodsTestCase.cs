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

    /// <summary> This class tests support for putting static utility classes
    /// like java.lang.Math directly into the context in order to
    /// use their methods.
    /// </summary>
    [TestFixture]
    public class StaticUtilityMethodsTestCase : BaseEvalTestCase
    {
     
        [Test]
        public virtual void testMath()
        {
            context.Put("Math", typeof(System.Math));
            assertEvalEquals("System.Math", "$Math");
            assertEvalEquals("3.0", "$Math.Ceiling(2.5)");
        }

        [Test]
        public virtual void testFoo()
        {
            context.Put("Foo", typeof(Foo));
            assertEvalEquals("test", "$Foo.foo('test')");
        }

        public class Foo
        {
            internal Foo()
            {
            }
            public static string foo(string s)
            {
                return s == null ? "foo" : s;
            }
        }
    }
}
