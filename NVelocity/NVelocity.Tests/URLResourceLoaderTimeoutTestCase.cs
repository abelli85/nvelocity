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

    using Runtime;
    using Runtime.Resource.Loader;

    using Misc;

    /// <summary> This class tests support for custom timeouts in URLResourceLoader.</summary>
    [TestFixture]
    public class URLResourceLoaderTimeoutTestCase : BaseEvalTestCase
    {
        private static bool isJava5plus;
        private TestLogChute logger = new TestLogChute();
        private URLResourceLoader loader = new URLResourceLoader();
        private int timeout = 2000;

        [SetUp]
        public void setUp()
        {
            base.setUp();
            engine.SetProperty("resource.loader", "url");
            engine.SetProperty("url.resource.loader.instance", loader);
            engine.SetProperty("url.resource.loader.timeout", (object)timeout);

            // parameters instance of logger
            logger.on();
            engine.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, logger);
            engine.SetProperty("runtime.log.logsystem.test.level", "debug");
            engine.Init();
        }

        [Test]
        public virtual void testTimeout()
        {
            if (isJava5plus)
            {
                System.Console.Out.WriteLine("Testing a 1.5+ JDK");
                Assert.AreEqual(timeout, loader.Timeout);
            }
            else
            {
                System.Console.Out.WriteLine("Testing a pre-1.5 JDK");
                Assert.AreEqual(-1, loader.Timeout);
            }
        }

        static URLResourceLoaderTimeoutTestCase()
        {
            {
                try
                {
                    System.Type.GetType("java.lang.annotation.Annotation");
                    isJava5plus = true;
                }
                catch (System.Exception cnfe)
                {
                    isJava5plus = false;
                }
            }
        }
    }
}
