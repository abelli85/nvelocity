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
    using Runtime;
    using Runtime.Log;

    using Misc;

    /// <summary> Base for test cases that use Evaluate, instead of going
    /// through the resource loaders.
    /// </summary>
    [TestFixture]
    public class BaseEvalTestCase 
    {
        virtual protected internal VelocityEngine Properties
        {
            set
            {
                // extension hook
            }

        }
        virtual protected internal VelocityContext Context
        {
            set
            {
                // extension hook
            }

        }
        protected internal VelocityEngine engine;
        protected internal VelocityContext context;
        protected internal bool DEBUG = false;
        protected internal TestLogChute log;

        [SetUp]
        public void setUp()
        {
            engine = new VelocityEngine();

            //by default, make the engine's Log output go to the test-report
            log = new TestLogChute(false, false);
            log.EnabledLevel = LogChute_Fields.INFO_ID;
            log.SystemErrLevel = LogChute_Fields.WARN_ID;
            engine.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, log);

            context = new VelocityContext();
            Context = context;
        }

        [TearDown]
        public void tearDown()
        {
            engine = null;
            context = null;
        }

        [Test]
        public virtual void testBase()
        {
            assertEvalEquals("", "");
            assertEvalEquals("abc\n123", "abc\n123");
        }

        protected internal virtual void assertContextValue(string key, object expected)
        {
            if (DEBUG)
            {
                engine.Log.Info("Expected value of '" + key + "': " + expected);
            }
            object value_Renamed = context.Get(key);
            if (DEBUG)
            {
                engine.Log.Info("Result: " + value_Renamed);
            }
            Assert.AreEqual(expected, value_Renamed);
        }

        protected internal virtual void assertEvalEquals(string expected, string template)
        {
            if (DEBUG)
            {
                engine.Log.Info("Expectation: " + expected);
            }
            Assert.AreEqual(expected, evaluate(template));
        }

        protected internal virtual System.Exception assertEvalException(string evil)
        {
            return assertEvalException(evil, null);
        }

        protected internal virtual System.Exception assertEvalException(string evil, System.Type exceptionType)
        {
            try
            {
                if (!DEBUG)
                {
                    log.off();
                }
                evaluate(evil);
                Assert.Fail("Template '" + evil + "' should have thrown an exception.");
            }
            catch (System.Exception e)
            {
                if (exceptionType != null && !exceptionType.IsAssignableFrom(e.GetType()))
                {
                    Assert.Fail("Was expecting template '" + evil + "' to throw " + exceptionType + " not " + e);
                }
                return e;
            }
            finally
            {
                if (!DEBUG)
                {
                    log.on();
                }
            }
            return null;
        }

        protected internal virtual System.Exception assertEvalExceptionAt(string evil, string template, int line, int col)
        {
            string loc = template + "[line " + line + ", column " + col + "]";
            if (DEBUG)
            {
                engine.Log.Info("Expectation: Exception at " + loc);
            }
            System.Exception e = assertEvalException(evil, null);
      
            if (e.Message.IndexOf(loc) < 1)
            {
                Assert.Fail("Was expecting exception at " + loc + " instead of " + e.Message);
            }
            else if (DEBUG)
            {
                engine.Log.Info("Result: " + e.Message);
            }
            return e;
        }

        protected internal virtual System.Exception assertEvalExceptionAt(string evil, int line, int col)
        {
            return assertEvalExceptionAt(evil, "", line, col);
        }

        protected internal virtual string evaluate(string template)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            try
            {
                if (DEBUG)
                {
                    engine.Log.Info("Template: " + template);
                }

                // use template as its own name, since our templates are short
                engine.Evaluate(context, writer, template, template);

                string result = writer.ToString();
                if (DEBUG)
                {
                    engine.Log.Info("Result: " + result);
                }
                return result;
            }
            catch (System.SystemException re)
            {
                if (DEBUG)
                {
                    engine.Log.Info("RuntimeException!", re);
                }
                throw re;
            }
            catch (System.Exception e)
            {
                if (DEBUG)
                {
                    engine.Log.Info("Exception!", e);
                }
                throw e;
            }
        }
    }
}
