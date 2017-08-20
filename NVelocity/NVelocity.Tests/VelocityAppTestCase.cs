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

    using Misc;

    /// <summary> This class is intended to test the app.Velocity.java class.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <author>  <a href="mailto:jon@latchkey.com">Jon S. Stevens</a>
    /// </author>
    /// <version>  $Id: VelocityAppTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class VelocityAppTestCase : BaseTestCase
    {
        private System.IO.StringWriter compare1 = new System.IO.StringWriter();
        private string input1 = "My name is $name -> $Floog";
        private string result1 = "My name is jason -> floogie woogie";

   
        [SetUp]
        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.FILE_RESOURCE_LOADER_PATH);

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testVelocityApp()
        {
            VelocityContext context = new VelocityContext();
            context.Put("name", "jason");
            context.Put("Floog", "floogie woogie");

            Velocity.Evaluate(context, compare1, "evaltest", input1);

            /*
            *            @todo FIXME: Not tested right now.
            *
            *            StringWriter result2 = new StringWriter();
            *            Velocity.MergeTemplate("mergethis.vm",  context, result2);
            *
            *            StringWriter result3 = new StringWriter();
            *            Velocity.invokeVelocimacro("floog", "test", new String[2],
            *                                        context, result3);
            */
            if (!result1.Equals(compare1.ToString()))
            {
                Assert.Fail("Output incorrect.");
            }
        }
    }
}
