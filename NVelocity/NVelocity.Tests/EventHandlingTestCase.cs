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
    using App;
    using App.Event;
    using Exception;
    using Misc;
    using NUnit.Framework;
    using Runtime;
    using Runtime.Log;

    /// <summary> Tests event handling for all event handlers except IIncludeEventHandler.  This is tested
    /// separately due to its complexity.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: EventHandlingTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class EventHandlingTestCase : ILogChute
    {
        public static string LogString
        {
            get
            {
                return logString;
            }

            set
            {
                logString = value;
            }

        }
        public static string NO_REFERENCE_VALUE = "<no reference value>";
        public static string REFERENCE_VALUE = "<reference value>";

        private static string logString = null;

        [Test]
        public virtual void testManualEventHandlers()
        {
            TestEventCartridge te = new TestEventCartridge();
            /**
            * Test attaching the event cartridge to the context
            */
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve.Init();

            /*
            *  lets make a Context and Add the event cartridge
            */

            VelocityContext inner = new VelocityContext();

            /*
            *  Now make an event cartridge, register all the
            *  event handlers (at once) and attach it to the
            *  Context
            */

            EventCartridge ec = new EventCartridge();
            ec.AddEventHandler(te);
            ec.AttachToContext(inner);

            /*
            *  now wrap the event cartridge - we want to make sure that
            *  we can do this w/instance harm
            */

            doTestReferenceInsertionEventHandler1(ve, inner);
            doTestReferenceInsertionEventHandler2(ve, inner);
            doTestNullValueEventHandler(ve, inner);
            doTestSetNullValueEventHandler(ve, inner);
            doTestMethodExceptionEventHandler1(ve, inner);
            doTestMethodExceptionEventHandler2(ve, inner);
        }

        /// <summary> Test assigning the event handlers via properties</summary>

        [Test]
        public virtual void testConfigurationEventHandlers()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, this);
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_METHODEXCEPTION, "NVelocity.Tests.Misc.TestEventCartridge;NVelocity.Tests");
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_NULLSET, "NVelocity.Tests.Misc.TestEventCartridge;NVelocity.Tests");
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_REFERENCEINSERTION, "NVelocity.Tests.Misc.TestEventCartridge;NVelocity.Tests");

            ve.Init();

            doTestReferenceInsertionEventHandler1(ve, null);
            doTestReferenceInsertionEventHandler2(ve, null);
            doTestNullValueEventHandler(ve, null);
            doTestSetNullValueEventHandler(ve, null);
            doTestMethodExceptionEventHandler1(ve, null);
            doTestMethodExceptionEventHandler2(ve, null);
        }

        /// <summary> Test all the event handlers using the given engine.</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="vcontext">
        /// </param>

        private void doTestReferenceInsertionEventHandler1(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);

             context.Put("name", "Velocity");

            /*
            *  First, the reference insertion handler
            */

            string s = "$name$name$name";

            System.IO.StringWriter w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);

            if (!w.ToString().Equals(REFERENCE_VALUE + REFERENCE_VALUE + REFERENCE_VALUE))
            {
                Assert.Fail("Reference insertion test 1");
            }
        }

        private void doTestReferenceInsertionEventHandler2(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);
             context.Put("name", "Velocity");

            /*
            *  using the same handler, we can deal with
            *  null references as well
            */

            string s = "$floobie";

          
            System.IO.TextWriter w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);

          
            if (!w.ToString().Equals(NO_REFERENCE_VALUE))
            {
                Assert.Fail("Reference insertion test 2");
            }
        }


        private void doTestNullValueEventHandler(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);

            /*
            *  now lets test setting a null value - this test
            *  should result in *no* Log output.
            */

            string s = "#set($settest = $NotAReference)";
           
            System.IO.TextWriter w = new System.IO.StringWriter();
            clearLogString();
            ve.Evaluate(context, w, "mystring", s);

            if (LogString != null)
            {
                Assert.Fail("NullSetEventHandler test 1");
            }
        }

        private void doTestSetNullValueEventHandler(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);

            /*
            *  now lets test setting a null value - this test
            *  should result in Log output.
            */

            string s = "#set($logthis = $NotAReference)";
           
            System.IO.TextWriter w = new System.IO.StringWriter();
            clearLogString();
            ve.Evaluate(context, w, "mystring", s);

            if (LogString == null)
            {
                Assert.Fail("NullSetEventHandler test 2");
            }
        }

        private void doTestMethodExceptionEventHandler1(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);

            /*
            *  finally, we test a method exception event - we do this
            *  by putting this class in the context, and calling
            *  a method that does nothing but throw an exception.
            *  we use flag in the context to turn the event handling
            *  on and off
            *
            *  Note also how the reference insertion process
            *  happens as well
            */

             context.Put("allow_exception", (object)true);

             context.Put("this", this);

            string s = " $this.throwException()";
           
            System.IO.TextWriter w = new System.IO.StringWriter();

            ve.Evaluate(context, w, "mystring", s);
        }


        private void doTestMethodExceptionEventHandler2(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);
             context.Put("this", this);

            /*
            *  now, we remove the exception flag, and we can see that the
            *  exception will propgate all the way up here, and
            *  wil be caught by the catch() block below
            */

            string s = " $this.throwException()";
           
            System.IO.TextWriter w = new System.IO.StringWriter();

            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("No MethodExceptionEvent received!");
            }
            catch (MethodInvocationException)
            {
                // Do nothing
            }
        }

        /// <summary>  silly method to throw an exception to test
        /// the method invocation exception event handling
        /// </summary>
        public virtual void throwException()
        {
            throw new System.Exception("Hello from throwException()");
        }

        /// <summary> Required by LogChute</summary>
        public virtual void Init(IRuntimeServices rs)
        {
            /* don't need it...*/
        }

        /// <summary> handler for LogChute interface</summary>
        public virtual void Log(int level, string message)
        {
            LogString = message;
        }

      
        public virtual void Log(int level, string message, System.Exception t)
        {
            LogString = message;
        }

        public virtual bool IsLevelEnabled(int level)
        {
            return true;
        }

        public static void clearLogString()
        {
            logString = null;
        }
    }
}
