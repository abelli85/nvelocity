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
    using Misc;
    using NVelocity.Util.Introspection;
    using Runtime;

    /// <summary> Test that the Info class in the Introspector holds the correct information.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <author>  <a href="mailto:isidore@setgame.com">Llewellyn Falco</a>
    /// </author>
    /// <version>  $Id: InfoTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class InfoTestCase : BaseTestCase
    {
        internal VelocityEngine ve;

        [SetUp]
        public void setUp()
        {
            ve = new VelocityEngine();
            ve.SetProperty("runtime.introspector.uberspect", "NVelocity.Tests.Misc.UberspectTestImpl;NVelocity.Tests");

            ve.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, "test/info");

            ve.Init();
        }


        [Test]
        public virtual void testInfoProperty()
        {
            // check property
            checkInfo("info1.vm", 1, 7);
        }

        [Test]
        public virtual void testInfoMethod()
        {
            // check method
            checkInfo("info2.vm", 1, 7);
        }

        public virtual void checkInfo(string templateName, int expectedLine, int expectedCol)
        {
            IContext context = new VelocityContext();
            System.IO.TextWriter writer = new System.IO.StringWriter();
            Template template = ve.GetTemplate(templateName, "UTF-8");
            Info info = null;

            context.Put("main", this);

            try
            {
                template.Merge(context, writer);
                writer.Flush();
                Assert.Fail("Uberspect should have thrown an exception");
            }
            catch (UberspectTestException E)
            {
                info = E.Info;
            }
            finally
            {
                writer.Close();
            }
            assertInfoEqual(info, templateName, expectedLine, expectedCol);
        }

        private void assertInfoEqual(Info i, string name, int line, int column)
        {
            Assert.AreEqual(name, i.TemplateName, "Template Name");
            Assert.AreEqual(line, i.Line, "Template Line");
            Assert.AreEqual(column, i.Column, "Template Column");
        }
    }
}
