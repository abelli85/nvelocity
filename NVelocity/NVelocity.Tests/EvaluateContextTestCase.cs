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

    using Context;
    using Runtime;
    using Misc;

    /// <summary> Tests scope of EvaluateContext.  
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: EvaluateContextTestCase.java 522413 2007-03-26 04:34:15Z wglass $
    /// </version>
    [TestFixture]
    public class EvaluateContextTestCase
    {
        [Test]
        public virtual void testLocalscopePutDoesntLeakButGetDoes()
        {
            RuntimeInstance instance;

            instance = new RuntimeInstance();
            instance.SetProperty(RuntimeConstants.VM_CONTEXT_LOCALSCOPE, (object)true);
            instance.Init();

            VelocityContext base_Renamed = new VelocityContext();
            base_Renamed.Put("outsideVar", "value1");

            EvaluateContext evc = new EvaluateContext(new InternalContextAdapterImpl(base_Renamed), instance);
            evc.Put("newLocalVar", "value2");

            // New variable Put doesn't leak
            Assert.IsNull(base_Renamed.Get("newLocalVar"));
            Assert.AreEqual("value2", evc.Get("newLocalVar"));

            // But we can still Get to "outsideVar"
            Assert.AreEqual("value1", evc.Get("outsideVar"));

            // If we decide to try and set outsideVar it won't leak
            evc.Put("outsideVar", "value3");
            Assert.AreEqual("value3", evc.Get("outsideVar"));
            Assert.AreEqual("value1", base_Renamed.Get("outsideVar"));

            Assert.AreEqual(2, evc.Keys.Length);
        }

        /// <summary> Test that local context can be configured.</summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testSetLocalContext()
        {
            RuntimeInstance instance = new RuntimeInstance();
            instance.SetProperty(RuntimeConstants.EVALUATE_CONTEXT_CLASS, "NVelocity.Tests.Misc.TestContext;NVelocity.Tests");
            instance.Init();

            VelocityContext base_Renamed = new VelocityContext();
            base_Renamed.Put("outsideVar", "value1");
            EvaluateContext evc = new EvaluateContext(new InternalContextAdapterImpl(base_Renamed), instance);

            // original entry
            Assert.AreEqual(1, evc.Keys.Length);

            // original plus local entry
            evc.Put("test", "result");
            Assert.AreEqual(2, evc.Keys.Length);

            // local context is case insensitive, so the count remains the same
            evc.Put("TEST", "result");
            Assert.AreEqual(2, evc.Keys.Length);

            Assert.AreEqual("result", evc.Get("test"));
            Assert.AreEqual("result", evc.Get("TEst"));

            Assert.IsNull(evc.Get("OUTSIDEVAR"));
        }

        [Test]
        public virtual void testSetLocalContextWithErrors()
        {
            VelocityContext base_Renamed = new VelocityContext();

            try
            {
                // Initialize with bad class name
                RuntimeInstance instance = new RuntimeInstance();
                instance.SetProperty(RuntimeConstants.EVALUATE_CONTEXT_CLASS, "org.apache");
                instance.Init();
                EvaluateContext evc = new EvaluateContext(new InternalContextAdapterImpl(base_Renamed), instance);
                Assert.Fail("Expected an exception");
            }
            catch (System.Exception e)
            {
            }

            try
            {
                // Initialize with class not implementing Context
                RuntimeInstance instance = new RuntimeInstance();
                instance.SetProperty(RuntimeConstants.EVALUATE_CONTEXT_CLASS, typeof(EvaluateContextTestCase).FullName);
                instance.Init();
                EvaluateContext evc = new EvaluateContext(new InternalContextAdapterImpl(base_Renamed), instance);
                Assert.Fail("Expected an exception");
            }
            catch (System.Exception e)
            {
            }
        }
    }
}
