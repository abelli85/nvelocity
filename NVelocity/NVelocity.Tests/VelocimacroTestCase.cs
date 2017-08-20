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
    using Runtime;

    using Misc;

    /// <summary> This class tests strange Velocimacro issues.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: VelocimacroTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class VelocimacroTestCase 
    {
        private string template1 = "#macro(foo $a)$a#end #macro(bar $b)#foo($b)#end #foreach($i in [1..3])#if($i == 3)#foo($i)#else#bar($i)#end#end";
        private string result1 = "  123";
        private string template2 = "#macro(bar $a)#set($a = $a + 1)$a#bar($a)#end#bar(0)";
        private string template3 = "#macro(baz $a)#set($a = $a + 1)$a#inner($a)#end#macro(inner $b)#baz($b)#end#baz(0)";
        private string template4 = "#macro(bad $a)#set($a = $a + 1)$a#inside($a)#end#macro(inside $b)#loop($b)#end#macro(loop $c)#bad($c)#end#bad(0)";

      
        [SetUp]
        public void setUp()
        {
            /*
            *  setup local scope for templates
            */
            Velocity.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, true);
            Velocity.SetProperty(RuntimeConstants.VM_MAX_DEPTH, 5);
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);
            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testVelociMacro()
        {
            VelocityContext context = new VelocityContext();

            System.IO.StringWriter writer = new System.IO.StringWriter();
            Velocity.Evaluate(context, writer, "vm_chain1", template1);

            string out_Renamed = writer.ToString();

            if (!result1.Equals(out_Renamed))
            {
                Assert.Fail("output incorrect.");
            }
        }

        /// <summary> Test case for evaluating max calling depths of macros</summary>
        [Test]
        public virtual void testVelociMacroCallMax()
        {
            VelocityContext context = new VelocityContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();

            try
            {
                Velocity.Evaluate(context, writer, "vm_chain2", template2);
                Assert.Fail("Did not exceed max macro call depth as expected");
            }
            catch (VelocityException e)
            {

                Assert.AreEqual("Max calling depth of 5 was exceeded in Template:vm_chain2" + " and Macro:bar with Call Stack:bar->bar->bar->bar->bar", e.InnerException.InnerException.InnerException.InnerException.InnerException.Message);
            }

            try
            {
                Velocity.Evaluate(context, writer, "vm_chain3", template3);
                Assert.Fail("Did not exceed max macro call depth as expected");
            }
            catch (VelocityException e)
            {
                Assert.AreEqual("Max calling depth of 5 was exceeded in Template:vm_chain3" + " and Macro:inner with Call Stack:baz->inner->baz->inner->baz", e.InnerException.InnerException.InnerException.InnerException.InnerException.Message);
            }

            //try
            //{
            //    Velocity.Evaluate(context, writer, "vm_chain4", template4);
            //    Assert.Fail("Did not exceed max macro call depth as expected");
            //}
            //catch (VelocityException e)
            //{
            //    Assert.AreEqual("Max calling depth of 5 was exceeded in Template:vm_chain4" + " and Macro:loop with Call Stack:bad->inside->loop->bad->inside", e.InnerException.InnerException.InnerException.InnerException.InnerException.Message);
            //}
        }
    }
}
