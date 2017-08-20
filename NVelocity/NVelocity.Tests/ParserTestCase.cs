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
    using Exception;

    /// <summary>  More specific parser tests where just templating
    /// isn't enough.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@apache.org">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: ParserTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class ParserTestCase 
    {

        /// <summary>  Test to make sure that using '=' in #if() throws a PEE</summary>
        public virtual void testEquals()
        {
            VelocityEngine ve = new VelocityEngine();

            ve.Init();

            /*
            *  this should parse fine -> uses ==
            */

            string template = "#if($a == $b) foo #end";

            ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);

            /*
            *  this should throw an exception
            */

            template = "#if($a = $b) foo #end";

            try
            {
                ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);
                Assert.Fail("Could evaluate template with errors!");
            }
            catch (ParseErrorException pe)
            {
                // Do nothing
            }
        }

        /// <summary>  Test to see if we force the first arg to #macro() to be a word</summary>
        [Test]
        public virtual void testMacro()
        {
            VelocityEngine ve = new VelocityEngine();

            ve.Init();

            /*
            * this should work
            */

            string template = "#macro(foo) foo #end";

            ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);

            /*
            *  this should throw an exception
            */

            template = "#macro($x) foo #end";

            try
            {
                ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);
                Assert.Fail("Could evaluate macro with errors!");
            }
            catch (ParseErrorException pe)
            {
                // Do nothing
            }
        }

        /// <summary>  Test to see if don't tolerage passing word tokens in anything but the
        /// 0th arg to #macro() and the 1th arg to foreach()
        /// </summary>
        [Test]
        public virtual void testArgs()
        {
            VelocityEngine ve = new VelocityEngine();

            ve.Init();

            /*
            * this should work
            */

            string template = "#macro(foo) foo #end";

            ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);

            /*
            *  this should work - spaces intentional
            */

            template = "#foreach(  $i     in  $woogie   ) end #end";

            ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);

            /*
            *  this should bomb
            */

            template = "#macro(   foo $a) $a #end #foo(woogie)";

            try
            {
                ve.Evaluate(new VelocityContext(), new System.IO.StringWriter(), "foo", template);
                Assert.Fail("Evaluation of macro with errors succeeded!");
            }
            catch (ParseErrorException pe)
            {
                // Do nothing
            }
        }

        /// <summary>  Test to see if we toString is called multiple times on references.</summary>
        [Test]
        public virtual void testASTReferenceToStringOnlyCalledOnce()
        {
            VelocityEngine ve = new VelocityEngine();

            ve.Init();

            string template = "$counter";

            ToStringCounter counter = new ToStringCounter();
            //UPGRADE_TODO: Class“java.util.HashMap”被转换为具有不同行为的 'System.Collections.Hashtable'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.IDictionary m = new System.Collections.Hashtable();
            m["counter"] = counter;

            ve.Evaluate(new VelocityContext(m), new System.IO.StringWriter(), "foo", template);

            Assert.AreEqual(1, counter.timesCalled);
        }

        public class ToStringCounter
        {
            public int timesCalled = 0;
            public override string ToString()
            {
                this.timesCalled++;
                return "foo";
            }
        }
    }
}
