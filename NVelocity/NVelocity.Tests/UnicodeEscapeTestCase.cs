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
    using Runtime.Parser.Node;

    /// <summary> Test Case for <a href="https://issues.apache.org/jira/browse/VELTOOLS-520">Velocity Tools Issue 520</a>.</summary>
    [TestFixture]
    public class UnicodeEscapeTestCase : BaseTestCase
    {
        private VelocityEngine engine;
        private VelocityContext context;

        [SetUp]
        public void setUp()
        {
            engine = new VelocityEngine();
            context = new VelocityContext();
        }

        [TearDown]
        public  void tearDown()
        {
            engine = null;
            context = null;
        }

        private string evaluate(string template)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            engine.Evaluate(context, writer, "test", template);
            return writer.ToString();
        }

        [Test]
        public virtual void testUnicodeEscape()
        {
            Assert.AreEqual("a", evaluate("#set($v = \"\\u0061\")$v"));
        }

        private void assertUnescape(string expected, string escaped)
        {
            string unescaped = ASTStringLiteral.Unescape(escaped);
            Assert.AreEqual(expected, unescaped);
            if (escaped.Equals(expected))
            {
                // checking that no new string allocated, for perfomance
                Assert.AreSame(unescaped, escaped);
            }
        }

        [Test]
        public virtual void testASTStringLiteralUnescape()
        {
            assertUnescape("", "");
            assertUnescape("bebe", "bebe");
            assertUnescape("as\\nsd", "as\\nsd");
            assertUnescape("a", "\\u0061");
            assertUnescape("abc", "\\u0061bc");
            assertUnescape("\u0061bc\u0064", "\\u0061bc\\u0064");
            assertUnescape("z\u0061bc\u0064f", "z\\u0061bc\\u0064f");
        }
    }
}
