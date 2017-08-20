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
    using Misc;
    using Runtime;

    

    /// <summary> Tests if we can hand Velocity an arbitrary class for logging.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: MethodInvocationExceptionTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class MethodInvocationExceptionTestCase 
    {
        protected internal bool DEBUG = false;

        [SetUp]
        public void setUp()
        {
            /*
            *  Init() Runtime with defaults
            */

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        protected internal virtual void log(string out_Renamed)
        {
            Velocity.Log.Debug(out_Renamed);
            if (DEBUG)
            {
                System.Console.Out.WriteLine(out_Renamed);
            }
        }

        /// <summary> Runs the test :
        /// 
        /// uses the Velocity class to eval a string
        /// which accesses a method that throws an
        /// exception.
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testNormalMethodInvocationException()
        {
            string template = "$woogie.doException() boing!";

            VelocityContext vc = new VelocityContext();

            vc.Put("woogie", this);

            System.IO.StringWriter w = new System.IO.StringWriter();

            try
            {
                Velocity.Evaluate(vc, w, "test", template);
                Assert.Fail("No exception thrown");
            }
            catch (MethodInvocationException mie)
            {
                log("Caught MIE (good!) :");
                log("  reference = " + mie.ReferenceName);
                log("  method    = " + mie.MethodName);

                System.Exception t = mie.InnerException;
              
                log("  throwable = " + t);

                if (t is System.Exception)
                {
                    log("  exception = " + ((System.Exception)t).Message);
                }
            }
        }

        [Test]
        public virtual void testGetterMethodInvocationException()
        {
            VelocityContext vc = new VelocityContext();
            vc.Put("woogie", this);

            System.IO.StringWriter w = new System.IO.StringWriter();

            /*
            *  second test - to ensure that methods accessed via Get+ construction
            *  also work
            */

            string template = "$woogie.foo boing!";

            try
            {
                Velocity.Evaluate(vc, w, "test", template);
                Assert.Fail("No exception thrown, second test.");
            }
            catch (MethodInvocationException mie)
            {
                log("Caught MIE (good!) :");
                log("  reference = " + mie.ReferenceName);
                log("  method    = " + mie.MethodName);

               
                System.Exception t = mie.InnerException;
             
                log("  throwable = " + t);

                if (t is System.Exception)
                {
                    
                    log("  exception = " + ((System.Exception)t).Message);
                }
            }
        }

        [Test]
        public virtual void testCapitalizedGetterMethodInvocationException()
        {
            VelocityContext vc = new VelocityContext();
            vc.Put("woogie", this);

            System.IO.StringWriter w = new System.IO.StringWriter();

            string template = "$woogie.Foo boing!";

            try
            {
                Velocity.Evaluate(vc, w, "test", template);
                Assert.Fail("No exception thrown, third test.");
            }
            catch (MethodInvocationException mie)
            {
                log("Caught MIE (good!) :");
                log("  reference = " + mie.ReferenceName);
                log("  method    = " + mie.MethodName);

          
                System.Exception t = mie.InnerException;
             
                log("  throwable = " + t);

                if (t is System.Exception)
                {
                    
                    log("  exception = " + ((System.Exception)t).Message);
                }
            }
        }

        [Test]
        public virtual void testSetterMethodInvocationException()
        {
            VelocityContext vc = new VelocityContext();
            vc.Put("woogie", this);

            System.IO.StringWriter w = new System.IO.StringWriter();

            string template = "#set($woogie.foo = 'lala') boing!";

            try
            {
                Velocity.Evaluate(vc, w, "test", template);
                Assert.Fail("No exception thrown, set test.");
            }
            catch (MethodInvocationException mie)
            {
                log("Caught MIE (good!) :");
                log("  reference = " + mie.ReferenceName);
                log("  method    = " + mie.MethodName);

                System.Exception t = mie.InnerException;
               
                log("  throwable = " + t);

                if (t is System.Exception)
                {
                   
                    log("  exception = " + ((System.Exception)t).Message);
                }
            }
        }


        /// <summary> test that no exception is thrown when in parameter to macro.
        /// This is the way we expect the system to work, but it would be better
        /// to throw an exception.
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testMacroInvocationException()
        {
            VelocityContext vc = new VelocityContext();
            vc.Put("woogie", this);

            System.IO.StringWriter w = new System.IO.StringWriter();

            string template = "#macro (macro1 $param) $param #end  #macro1($woogie.getFoo())";

            try
            {
                Velocity.Evaluate(vc, w, "test", template);
                Assert.Fail("No exception thrown, macro invocation test.");
            }
            catch (MethodInvocationException mie)
            {
                log("Caught MIE (good!) :");
                log("  reference = " + mie.ReferenceName);
                log("  method    = " + mie.MethodName);

                System.Exception t = mie.InnerException;
             
                log("  throwable = " + t);

                if (t is System.Exception)
                {
                  
                    log("  exception = " + ((System.Exception)t).Message);
                }
            }
            catch (System.Exception e)
            {
                Assert.Fail("Wrong exception thrown, test of exception within macro parameter");
            }
        }

        public virtual void doException()
        {
            throw new System.NullReferenceException();
        }

        public virtual void getFoo()
        {
            throw new System.Exception("Hello from getFoo()");
        }

        public virtual void setFoo(string foo)
        {
            throw new System.Exception("Hello from setFoo()");
        }
    }
}
