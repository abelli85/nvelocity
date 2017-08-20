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

    /// <summary> Used to check that vararg method calls on references work properly</summary>
    [TestFixture]
    public class VarargMethodsTestCase : BaseEvalTestCase
    {
        override protected internal VelocityContext Context
        {
            set
            {
                value.Put("nice", new NiceTool());
                value.Put("nasty", new NastyTool());
                value.Put("objects", new object[] { this, typeof(VelocityContext) });
                value.Put("strings", new string[] { "one", "two" });
                value.Put("doubles", new double[] { 1.5, 2.5 });
                value.Put("float", (object)1f);
                value.Put("ints", new int[] { 1, 2 });
            }

        }
      
        [Test]
        public virtual void testStrings()
        {
            assertEvalEquals("onetwo", "$nice.var($strings)");
            //assertEvalEquals("onetwo", "$nice.var('one','two')");
            //assertEvalEquals("one", "$nice.var('one')");
            //assertEvalEquals("", "$nice.var()");
        }

        [Test]
        public virtual void testDoubles()
        {
            assertEvalEquals("4", "$nice.Add($doubles)");
            
            //assertEvalEquals("3", "$nice.Add(1,2)");
            //assertEvalEquals("1", "$nice.Add(1)");
            //assertEvalEquals("0", "$nice.Add()");
        }

        [Test]
        public virtual void testFloatToDoubleVarArg()
        {
            assertEvalEquals("1.0", "$nice.Add($float)");
        }

        [Test]
        public virtual void testStringVsStrings()
        {
            assertEvalEquals("onlyone", "$nasty.var('one')");
            //assertEvalEquals("onlynull", "$nasty.var($null)");
            //assertEvalEquals("", "$nasty.var()");
        }

        [Test]
        public virtual void testIntVsDoubles()
        {
            assertEvalEquals("1", "$nasty.Add(1)");
            assertEvalEquals("1.0", "$nasty.Add(1.0)");
            assertEvalEquals("3.0", "$nasty.Add(1.0,2)");
        }

        [Test]
        public virtual void testInts()
        {
            assertEvalEquals("3", "$nasty.Add($ints)");
            //assertEvalEquals("3", "$nasty.Add(1,2)");
            //assertEvalEquals("1", "$nasty.Add(1)");
            //// Add(int[]) wins because it is "more specific"
            //assertEvalEquals("0", "$nasty.Add()");
        }

        [Test]
        public virtual void testStringsVsObjectsAKASubclassVararg()
        {
            assertEvalEquals("objects", "$nice.test($objects)");
            //assertEvalEquals("objects", "$nice.test($nice,$nasty,$ints)");
            //assertEvalEquals("strings", "$nice.test('foo')");
        }

        [Test]
        public virtual void testObjectVarArgVsObjectEtc()
        {
            assertEvalEquals("object,string", "$nasty.test($nice,'foo')");
        }

        [Test]
        public virtual void testObjectVarArgVsObjectVelocity605()
        {
            assertEvalEquals("string", "$nasty.test('joe')");
            assertEvalEquals("object", "$nasty.test($nice)");
        }

        [Test]
        public virtual void testNoArgs()
        {
            assertEvalEquals("noargs", "$nasty.test()");
        }

        [Test]
        public virtual void testPassingArrayToVarArgVelocity642()
        {
            assertEvalEquals("[one, two]", "$nasty.test642($strings)");
            assertEvalEquals("[1, 2]", "#set( $list = [1..2] )$nasty.test642($list.toArray())");
        }

        [Test]
        public virtual void testNullToPrimitiveVarArg()
        {
            assertEvalEquals("int[]", "$nasty.test649($null)");
        }

        [Test]
        public virtual void testVelocity651()
        {
            assertEvalEquals("String,List", "$nasty.test651('test',['TEST'])");
        }


        public class NiceTool
        {
            public virtual string var(string[] ss)
            {
                System.Text.StringBuilder out_Renamed = new System.Text.StringBuilder();
                for (int i = 0; i < ss.Length; i++)
                {
                    out_Renamed.Append(ss[i]);
                }
                return out_Renamed.ToString();
            }

            public virtual double add(double[] dd)
            {
                double total = 0;
                for (int i = 0; i < dd.Length; i++)
                {
                    total += dd[i];
                }
                return total;
            }

            public virtual string test(object[] oo)
            {
                return "objects";
            }

            public virtual string test(string[] oo)
            {
                return "strings";
            }
        }

        public class NastyTool : NiceTool
        {
            public virtual string var(string s)
            {
                return "only" + s;
            }

            public virtual int add(int[] ii)
            {
                int total = 0;
                for (int i = 0; i < ii.Length; i++)
                {
                    total += ii[i];
                }
                return total;
            }

            public virtual int add(int i)
            {
                return i;
            }

            public virtual string test()
            {
                return "noargs";
            }

            public virtual object test(object arg)
            {
                return "object";
            }

            public virtual object test(string arg)
            {
                return "string";
            }

            public virtual string test(object[] array)
            {
                return "object[]";
            }

            public virtual string test(object object_Renamed, string property)
            {
                return "object,string";
            }

            public virtual string test642(object[] array)
            {
                //JDK5: return Arrays.deepToString(array);
                if (array == null)
                {
                    return null;
                }
                System.Text.StringBuilder o = new System.Text.StringBuilder("[");
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                    {
                        o.Append(", ");
                    }
                    o.Append(System.Convert.ToString(array[i]));
                }
                o.Append("]");
                return o.ToString();
            }

            public virtual string test649(int[] array)
            {
                return "int[]";
            }

            public virtual string test651(string s, string s2, object[] args)
            {
                return "String,String,Object[]";
            }

            public virtual string test651(string s, System.Collections.IList l)
            {
                return "String,List";
            }
        }
    }
}
