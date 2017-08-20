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
    using Context;
    using Runtime;

    using Provider;

    /// <summary> Used to check that method calls with number parameters are executed correctly.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Peter Romianowski</a>
    /// </author>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    [TestFixture]
    public class NumberMethodCallsTestCase 
    {
        private VelocityEngine ve = null;

        private const bool PRINT_RESULTS = false;


        public void setUp()
        {
            ve = new VelocityEngine();
            ve.Init();
        }

        public virtual void init(IRuntimeServices rs)
        {
            // do nothing with it
        }


        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testNumberMethodCalls()
        {
            VelocityContext vc = new VelocityContext();

            // context object with overloaded methods with number arguments
            vc.Put("Test", new NumberMethods());

            // numbers for context
            vc.Put("AByte", (object)System.SByte.Parse("10"));
            vc.Put("AShort", (object)System.Int16.Parse("10"));
            vc.Put("AInteger", (object)10);
            vc.Put("ALong", (object)10);
            vc.Put("ADouble", (object)10);
            vc.Put("AFloat", (object)10);

            // check context objects
            System.Console.Out.WriteLine("Testing: method calls with arguments as context objects");
            checkResults(vc, "$Test.numMethod($AByte)", "byte (10)");
            checkResults(vc, "$Test.numMethod($AShort)", "short (10)");
            checkResults(vc, "$Test.numMethod($AInteger)", "int (10)");
            checkResults(vc, "$Test.numMethod($ADouble)", "double (10.0)");
            checkResults(vc, "$Test.numMethod($AFloat)", "double (10.0)");
            checkResults(vc, "$Test.numMethod($ALong)", "long (10)");

            // check literals
            //    -- will cast floating point literal to smallest possible of Double, BigDecimal
            //    -- will cast integer literal to smallest possible of Integer, Long, BigInteger
            System.Console.Out.WriteLine("Testing: method calls with arguments as literals");
            checkResults(vc, "$Test.numMethod(10.0)", "double (10.0)");
            checkResults(vc, "$Test.numMethod(10)", "int (10)");
            checkResults(vc, "$Test.numMethod(10000000000)", "long (10000000000)");

            // check calculated results
            // -- note calculated value is cast to smallest possible type
            // -- method invoked is smallest relevant method
            // -- it's an unusual case here of both byte and int methods, but this works as expected
            System.Console.Out.WriteLine("Testing: method calls with arguments as calculated values");
            checkResults(vc, "#set($val = 10.0 + 1.5)$Test.numMethod($val)", "double (11.5)");
            checkResults(vc, "#set($val = 100 + 1)$Test.numMethod($val)", "int (101)");
            checkResults(vc, "#set($val = 100 * 1000)$Test.numMethod($val)", "int (100000)");
            checkResults(vc, "#set($val = 100 + 1.5)$Test.numMethod($val)", "double (101.5)");
            checkResults(vc, "#set($val = $ALong + $AInteger)$Test.numMethod($val)", "long (20)");;
        }


        private void checkResults(IContext vc, string template, string compare)
        {

            System.IO.StringWriter writer = new System.IO.StringWriter();
            ve.Evaluate(vc, writer, "test", template);
            Assert.AreEqual("Incorrect results for template '" + template + "'.", compare, writer.ToString());

            if (PRINT_RESULTS)
                System.Console.Out.WriteLine("Method call successful: " + template);
        }
    }
}
