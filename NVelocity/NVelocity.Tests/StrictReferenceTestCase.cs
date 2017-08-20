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

    using Exception;
    using Runtime;

    /// <summary> Test strict reference mode turned on by the velocity property
    /// runtime.references.strict
    /// </summary>
    [TestFixture]
    public class StrictReferenceTestCase : BaseEvalTestCase
    {
       [SetUp]
        public void setUp()
        {
            base.setUp();
            engine.SetProperty(RuntimeConstants.RUNTIME_REFERENCES_STRICT, (object)true);
            context.Put("NULL", (object)null);
            context.Put("bar", (object)null);
            context.Put("TRUE", (object)true);
        }


        /// <summary> Test the modified behavior of #if in strict mode.  Mainly, that
        /// single variables references in #if statements use non strict rules
        /// </summary>
        [Test]
        public virtual void testIfStatement()
        {
            Fargo fargo = new Fargo();
            fargo.next = new Fargo();
            context.Put("fargo", fargo);
            assertEvalEquals("", "#if($bogus)xxx#end");
            assertEvalEquals("xxx", "#if($fargo)xxx#end");
            assertEvalEquals("", "#if( ! $fargo)xxx#end");
            assertEvalEquals("xxx", "#if($bogus || $fargo)xxx#end");
            assertEvalEquals("", "#if($bogus && $fargo)xxx#end");
            assertEvalEquals("", "#if($fargo != $NULL && $bogus)xxx#end");
            assertEvalEquals("xxx", "#if($fargo == $NULL || ! $bogus)xxx#end");
            assertEvalEquals("xxx", "#if(! $bogus1 && ! $bogus2)xxx#end");
            assertEvalEquals("xxx", "#if($fargo.prop == \"propiness\" && ! $bogus && $bar == $NULL)xxx#end");
            assertEvalEquals("", "#if($bogus && $bogus.foo)xxx#end");

            assertMethodEx("#if($bogus.foo)#end");
            assertMethodEx("#if(!$bogus.foo)#end");
        }


        /// <summary> We make sure that variables can actuall hold null
        /// values.
        /// </summary>
        [Test]
        public virtual void testAllowNullValues()
        {
            evaluate("$bar");
            assertEvalEquals("true", "#if($bar == $NULL)true#end");
            assertEvalEquals("true", "#set($foobar = $NULL)#if($foobar == $NULL)true#end");
            assertEvalEquals("13", "#set($list = [1, $NULL, 3])#foreach($item in $list)#if($item != $NULL)$item#end#end");
        }

        /// <summary> Test that variables references that have not been defined throw exceptions </summary>
        [Test]
        public virtual void testStrictVariableRef()
        {
            // We expect a Method exception on the following
            assertMethodEx("$bogus");
            assertMethodEx("#macro(test)$bogus#end #test()");

            assertMethodEx("#set($bar = $bogus)");

            assertMethodEx("#if($bogus == \"bar\") #end");
            assertMethodEx("#if($bogus != \"bar\") #end");
            assertMethodEx("#if(\"bar\" == $bogus) #end");
            assertMethodEx("#if($bogus > 1) #end");
            assertMethodEx("#foreach($item in $bogus)#end");

            // make sure no exceptions are thrown here    
            evaluate("#set($foo = \"bar\") $foo");
            evaluate("#macro(test1 $foo1) $foo1 #end #test1(\"junk\")");
            evaluate("#macro(test2) #set($foo2 = \"bar\") $foo2 #end #test2()");
        }

        /// <summary> Test that exceptions are thrown when methods are called on
        /// references that contains objects that do not contains those
        /// methods.
        /// </summary>
        [Test]
        public virtual void testStrictMethodRef()
        {
            Fargo fargo = new Fargo();
            fargo.next = new Fargo();
            context.Put("fargo", fargo);

            // Mainly want to make sure no exceptions are thrown here
            assertEvalEquals("propiness", "$fargo.prop");
            assertEvalEquals("$fargo.nullVal", "$fargo.nullVal");
            assertEvalEquals("", "$!fargo.nullVal");
            assertEvalEquals("propiness", "$fargo.next.prop");

            assertMethodEx("$fargo.foobar");
            assertMethodEx("$fargo.next.foobar");
            assertMethodEx("$fargo.foobar()");
            assertMethodEx("#set($fargo.next.prop = $TRUE)");
            assertMethodEx("$fargo.next.setProp($TRUE)");
        }

        /// <summary> Make sure exceptions are thrown when when we attempt to call
        /// methods on null values.
        /// </summary>
        [Test]
        public virtual void testStrictMethodOnNull()
        {
            Fargo fargo = new Fargo();
            fargo.next = new Fargo();
            context.Put("fargo", fargo);

            assertVelocityEx("$NULL.bogus");
            assertVelocityEx("$fargo.nullVal.bogus");
            assertVelocityEx("$fargo.next.nullVal.bogus");
            assertVelocityEx("#if (\"junk\" == $fargo.nullVal.bogus)#end");
            assertVelocityEx("#if ($fargo.nullVal.bogus > 2)#end");
            assertVelocityEx("#set($fargo.next.nullVal.bogus = \"junk\")");
            assertVelocityEx("#set($foo = $NULL.bogus)");
            assertVelocityEx("#foreach($item in $fargo.next.nullVal.bogus)#end");

            evaluate("$fargo.Prop");
            assertVelocityEx("#set($fargo.Prop = $NULL)$fargo.Prop.Next");

            // make sure no exceptions are thrown here
            evaluate("$fargo.next.next");
            evaluate("$fargo.next.nullVal");
            evaluate("#foreach($item in $fargo.nullVal)#end");
        }

        /// <summary> Make sure undefined macros throw exceptions</summary>
        [Test]
        public virtual void testMacros()
        {
            assertParseEx("#bogus()");
            assertParseEx("#bogus (  )");
            assertParseEx("#bogus( $a )");
            assertParseEx("abc#bogus ( $a )a ");

            assertEvalEquals(" true ", "#macro(test1) true #end#test1()");
            assertEvalEquals(" true ", "#macro(test2 $a) $a #end#test2 ( \"true\")");
            assertEvalEquals("#CCFFEE", "#CCFFEE");
            assertEvalEquals("#F - ()", "#F - ()");
            assertEvalEquals("#F{}", "#F{}");
        }


        /// <summary> Assert that we Get a MethodInvocationException when calling Evaluate</summary>
        public virtual void assertMethodEx(string template)
        {
            assertEvalException(template, typeof(MethodInvocationException));
        }

        /// <summary> Assert that we Get a VelocityException when calling Evaluate</summary>
        public virtual void assertVelocityEx(string template)
        {
            assertEvalException(template, typeof(VelocityException));
        }

        /// <summary> Assert that we Get a MethodInvocationException when calling Evaluate</summary>
        public virtual void assertParseEx(string template)
        {
            assertEvalException(template, typeof(ParseErrorException));
        }


        public class Fargo
        {
            virtual public string Prop
            {
                get
                {
                    return prop;
                }

                set
                {
                    this.prop = prop;
                }

            }
            virtual public string NullVal
            {
                get
                {
                    return null;
                }

            }
            virtual public Fargo Next
            {
                get
                {
                    return next;
                }

            }
            internal string prop = "propiness";
            internal Fargo next = null;
        }
    }
}
