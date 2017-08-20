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

    /// <summary> This class tests the #define directive</summary>
    [TestFixture]
    public class DefineTestCase : BaseEvalTestCase
    {
        protected internal virtual string defAndEval(string block)
        {
            return defAndEval("def", block);
        }

        protected internal virtual string defAndEval(string key, string block)
        {
            return evaluate("#define( $" + key + " )" + block + "#end$" + key);
        }

        [Test]
        public virtual void testSimple()
        {
            Assert.AreEqual("abc", defAndEval("abc"));
            assertEvalEquals("abc abc abc", "#define( $a )abc#end$a $a $a");
        }

        [Test]
        public virtual void testNotSimple()
        {
            Assert.AreEqual("true", defAndEval("#if( $def )true#end"));
            Assert.AreEqual("123", defAndEval("#foreach( $i in [1..3] )$i#end"));
            Assert.AreEqual("hello world", defAndEval("#macro( test )hello world#end#test()"));
        }

        [Test]
        public virtual void testOverridingDefinitionInternally()
        {
            assertEvalEquals("trueFalse", "#define( $or )true#set( $or = false )#end$or$or");
        }

        [Test]
        public virtual void testLateBinding()
        {
            context.Put("baz", "foo");
            assertEvalEquals("foobar", "#define( $lb )$baz#end${lb}#set( $baz = 'bar' )${lb}");
        }

        [Test]
        public virtual void testRerendering()
        {
            context.Put("inc", new Inc());
            assertEvalEquals("1 2 3", "#define( $i )$inc#end$i $i $i");
        }

        [Test]
        public virtual void testAssignation()
        {
            assertEvalEquals("[][hello]", "#define( $orig )hello#end[#set( $assig = $orig )][$assig]");
        }

        [Test]
        public virtual void testNonRenderingUsage()
        {
            string template = "#define($foo)\n" + " foo_contents\n" + "#end\n" + "#if ($foo)\n" + " found foo\n" + "#end";
            assertEvalEquals(" found foo\n", template);
        }

        [Test]
        public virtual void testRecursionLimit()
        {
            try
            {
                assertEvalEquals("$r", "#define( $r )$r#end$r");
            }
            catch (System.ApplicationException e)
            {
                Assert.Fail("Infinite recursion should not be possible.");
            }
            catch (System.Exception t)
            {
                Assert.Fail("Recursion should not have thrown an exception");
            }

        }

        [Test]
        public virtual void testThingsOfQuestionableMorality()
        {
            // redefining $foo within $foo
            Assert.AreEqual("foobar", defAndEval("foo", "foo#define( $foo )bar#end$foo"));
        }


        public class Inc
        {
            internal int foo = 1;
            public override string ToString()
            {
                return System.Convert.ToString(foo++);
            }
        }
    }
}
