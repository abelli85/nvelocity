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

    using Exception;
    using Runtime;

    /// <summary> This class tests support for strict math mode.</summary>
    [TestFixture]
    public class StrictMathTestCase : BaseEvalTestCase
    {
        [SetUp]
        public void setUp()
        {
            base.setUp();
            engine.SetProperty(RuntimeConstants.STRICT_MATH, (object)true);
            context.Put("num", (object)5);
            context.Put("zero", (object)0);
        }

        protected internal virtual void assertNullMathEx(string operation)
        {
            string leftnull = "#set( $foo = $null " + operation + " $num )";
            assertEvalException(leftnull, typeof(MathException));
            string rightnull = "#set( $foo = $num " + operation + " $null )";
            assertEvalException(rightnull, typeof(MathException));
        }

        protected internal virtual void assertImaginaryMathEx(string operation)
        {
            string infinity = "#set( $foo = $num " + operation + " $zero )";
            assertEvalException(infinity, typeof(MathException));
        }

        [Test]
        public virtual void testAdd()
        {
            assertNullMathEx("+");
        }

        [Test]
        public virtual void testSub()
        {
            assertNullMathEx("-");
        }

        [Test]
        public virtual void testMul()
        {
            assertNullMathEx("*");
        }

        [Test]
        public virtual void testMod()
        {
            assertNullMathEx("%");
            assertImaginaryMathEx("%");
        }

        [Test]
        public virtual void testDiv()
        {
            assertNullMathEx("/");
            assertImaginaryMathEx("/");
        }
    }
}
