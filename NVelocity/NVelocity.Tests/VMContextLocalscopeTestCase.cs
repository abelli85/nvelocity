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

    /// <summary> Tests scope of velocimacros with localscope setting. 
    /// 
    /// </summary>
    /// <author>  <a href="mailto:stephenh@chase3000.com">Stephen Habermann</a>
    /// </author>
    /// <version>  $Id: VMContextLocalscopeTestCase.java 685433 2008-08-13 04:18:27Z nbubna $
    /// </version>
    [TestFixture]
    public class VMContextLocalscopeTestCase 
    {
        private RuntimeInstance instance;

        [SetUp]
        public void setUp()
        {
            this.instance = new RuntimeInstance();
            this.instance.SetProperty(RuntimeConstants.VM_CONTEXT_LOCALSCOPE, true);
            this.instance.Init();
        }

        [Test]
        public virtual void testLocalscopePutDoesntLeakButGetDoes()
        {
            VelocityContext base_Renamed = new VelocityContext();
            base_Renamed.Put("outsideVar", "value1");

            ProxyVMContext vm = new ProxyVMContext(new InternalContextAdapterImpl(base_Renamed), this.instance, true);
            vm.Put("newLocalVar", "value2");

            // New variable Put doesn't leak
            Assert.IsNull(base_Renamed.Get("newLocalVar"));
            Assert.AreEqual("value2", vm.Get("newLocalVar"));

            // But we can still Get to "outsideVar"
            Assert.AreEqual("value1", vm.Get("outsideVar"));

            // If we decide to try and set outsideVar it won't leak
            vm.Put("outsideVar", "value3");
            Assert.AreEqual("value3", vm.Get("outsideVar"));
            Assert.AreEqual("value1", base_Renamed.Get("outsideVar"));
        }
    }
}
