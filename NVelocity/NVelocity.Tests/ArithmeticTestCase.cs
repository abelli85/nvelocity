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
    using System;

    using NUnit.Framework;

    using Runtime.Parser.Node;

    /// <summary> Test arithmetic operations. Introduced after extending from Integer-only
    /// to Number-handling.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:pero@antaramusic.de">Peter Romianowski</a>
    /// </author>
    [TestFixture]
    public class ArithmeticTestCase
    {
        [Test]
        public virtual void testAdd()
        {
            addHelper(10, (short)20, 30, typeof(System.Int32));
            addHelper((sbyte)10, (short)20, 30, typeof(System.Int16));
            addHelper((float)10, (short)20, 30, typeof(System.Single));
            addHelper((sbyte)10, (double)20, 30, typeof(System.Double));

            addHelper((System.Int64)10, 20, 30, typeof(System.Int64));

            addHelper(20, (System.Double)10, 30, typeof(System.Double));

            // Test overflow
            addHelper((System.Int32)System.Int32.MaxValue, (short)20, (double)System.Int32.MaxValue + 20, typeof(System.Int64));

            addHelper(20, (long)System.Int64.MaxValue, (double)System.Int64.MaxValue + 20, typeof(System.Int64));

            addHelper(-20, (long)System.Int64.MinValue, (double)System.Int64.MinValue - 20, typeof(System.Int64));
        }

        private void addHelper(Object n1, Object n2, double expectedResult, System.Type expectedResultType)
        {
            Object result = MathUtils.Add(MathUtils.ToMaxType(n1.GetType(), n2.GetType()), n1, n2);
            Assert.AreEqual(expectedResult, System.Convert.ToDouble(result), 0.01, "The arithmetic operation produced an unexpected result.");
            Assert.AreEqual(expectedResultType, result.GetType(), "ResultType does not match.");
        }

        [Test]
        public virtual void testSubtract()
        {
            subtractHelper(100, (short)20, 80, typeof(System.Int32));
            subtractHelper((sbyte)100, (short)20, 80, typeof(System.Int16));
            subtractHelper((float)100, (short)20, 80, typeof(System.Single));
            subtractHelper((sbyte)100, (double)20, 80, typeof(System.Double));

            subtractHelper(new System.Decimal(100), 20, 80, typeof(System.Decimal));

            subtractHelper(100, new System.Decimal(20), 80, typeof(System.Decimal));

            // Test overflow
            subtractHelper((System.Int32)System.Int32.MinValue, (short)20, (double)System.Int32.MinValue - 20, typeof(System.Int64));

            subtractHelper(-20, (long)System.Int64.MaxValue, -20d - (double)System.Int64.MaxValue, typeof(System.Decimal));

            subtractHelper((System.Int32)System.Int32.MaxValue, (long)System.Int64.MinValue, (double)System.Int64.MaxValue + (double)System.Int32.MaxValue, typeof(System.Decimal));
        }

        private void subtractHelper(Object n1, Object n2, double expectedResult, System.Type expectedResultType)
        {
            Object result = MathUtils.Subtract(MathUtils.ToMaxType(n1.GetType(), n2.GetType()), n1, n2);
            Assert.AreEqual(expectedResult, System.Convert.ToDouble(result), 0.01, "The arithmetic operation produced an unexpected result.");
            Assert.AreEqual(expectedResultType, result.GetType(), "ResultType does not match.");
        }

        [Test]
        public virtual void testMultiply()
        {
            multiplyHelper(10, (short)20, 200, typeof(System.Int32));
            multiplyHelper((sbyte)100, (short)20, 2000, typeof(System.Int16));
            multiplyHelper((sbyte)100, (short)2000, 200000, typeof(System.Int32));
            multiplyHelper((float)100, (short)20, 2000, typeof(System.Single));
            multiplyHelper((sbyte)100, (double)20, 2000, typeof(System.Double));

            multiplyHelper(new System.Decimal(100), 20, 2000, typeof(System.Decimal));

            multiplyHelper(100, new System.Decimal(20), 2000, typeof(System.Decimal));

            // Test overflow
            multiplyHelper((System.Int32)System.Int32.MaxValue, (short)10, (double)System.Int32.MaxValue * 10d, typeof(System.Int64));
            multiplyHelper((System.Int32)System.Int32.MaxValue, (short)(-10), (double)System.Int32.MaxValue * (-10d), typeof(System.Int64));

            multiplyHelper(20, (long)System.Int64.MaxValue, 20d * (double)System.Int64.MaxValue, typeof(System.Decimal));
        }

        private void multiplyHelper(Object n1, Object n2, double expectedResult, System.Type expectedResultType)
        {
            Object result = MathUtils.Multiply(MathUtils.ToMaxType(n1.GetType(), n2.GetType()), n1, n2);
            Assert.AreEqual(expectedResult, System.Convert.ToDouble(result), 0.01, "The arithmetic operation produced an unexpected result.");
            Assert.AreEqual(expectedResultType, result.GetType(), "ResultType does not match.");
        }

        [Test]
        public virtual void testDivide()
        {
            divideHelper(10, (short)2, 5, typeof(System.Int32));
            divideHelper((sbyte)10, (short)2, 5, typeof(System.Int16));

            divideHelper(new System.Decimal(10), (short)2, 5, typeof(System.Decimal));
            divideHelper(10, (short)4, 2, typeof(System.Int32));
            divideHelper(10, (float)2.5f, 4, typeof(System.Single));
            divideHelper(10, (double)2.5, 4, typeof(System.Double));
            //UPGRADE_TODO: Class“java.math.BigDecimal”被转换为具有不同行为的 'System.Decimal'。 "ms-help:/
            divideHelper(10, new System.Decimal(2.5), 4, typeof(System.Decimal));
        }

        private void divideHelper(Object n1, Object n2, double expectedResult, System.Type expectedResultType)
        {
            Object result = MathUtils.Divide(MathUtils.ToMaxType(n1.GetType(), n2.GetType()), n1, n2);
            Assert.AreEqual(expectedResult, System.Convert.ToDouble(result), 0.01, "The arithmetic operation produced an unexpected result.");
            Assert.AreEqual(expectedResultType, result.GetType(), "ResultType does not match.");
        }

        [Test]
        public virtual void testModulo()
        {
            moduloHelper(10, (short)2, 0, typeof(System.Int32));
            moduloHelper((sbyte)10, (short)3, 1, typeof(System.Int16));

            moduloHelper(10, (float)5.5f, 4.5, typeof(System.Single));

            try
            {
                moduloHelper(10, new System.Decimal(2.5), 4, typeof(System.Decimal));
                Assert.Fail("Modulo with BigDecimal is not allowed! Should have thrown an ArithmeticException.");
            }
            catch (System.ArithmeticException e)
            {
                // do nothing
            }
        }

        private void moduloHelper(Object n1, Object n2, double expectedResult, System.Type expectedResultType)
        {
            Object result = MathUtils.Modulo(MathUtils.ToMaxType(n1.GetType(), n2.GetType()), n1, n2);
            Assert.AreEqual(expectedResult, System.Convert.ToDouble(result), 0.01, "The arithmetic operation produced an unexpected result.");
            Assert.AreEqual(expectedResultType, result.GetType(), "ResultType does not match.");
        }

        /*
        *
        *    COMMENT OUT FOR PERFORMANCE-MEASSUREMENTS
        *
        *    public void testProfile()
        *    {
        *
        *        long start = System.currentTimeMillis();
        *
        *        Number v1 = new Long (1000);
        *        Number v2 = new Double (10.23);
        *        Number result = null;
        *        for (int a = 0; a < 10000; a++)
        *        {
        *
        *            result = MathUtils.typeConvert (
        *                new BigDecimal (v1.doubleValue()).Add (
        *                new BigDecimal (v2.doubleValue())), v1, v2, false);
        *
        *        }
        *
        *        System.out.println ("took: "+(System.currentTimeMillis()-start));
        *
        *        start = System.currentTimeMillis();
        *        for (int a = 0; a < 10000; a++)
        *        {
        *
        *            result = MathUtils.Divide( v1, v2);
        *        }
        *
        *        Number result2 = result;
        *        System.out.println ("took: "+(System.currentTimeMillis()-start));
        *    }
        *
        */

    }
}
