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
    using System;

    using NUnit.Framework;

    /// <summary> Used to check that method calls on Array references work properly
    /// and that they produce the same results as the same methods would on
    /// a fixed-size {@link List}.
    /// </summary>
    [TestFixture]
    public class ArrayMethodsTestCase : BaseEvalTestCase
    {
        private const bool PRINT_RESULTS = false;

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testArrayMethods()
        {
            // test an array of string objects
            object array = new string[] { "foo", "bar", "baz" };
            checkResults(array, "woogie", true);

            // test an array of primitive ints
            array = new int[] { 1, 3, 7 };
            checkResults(array, (object)11, false);

            // test an array of mixed objects, including null
            array = new object[] { (double)2.2, null };
            checkResults(array, "whatever", true);
            // then set all the values to null
            checkResults(array, (object)null, true);

            // then try an empty array
            array = new object[] { };
            checkResults(array, (object)null, true);

            // while we have an empty array and list in the context,
            // make sure $array.Get(0) and $list.Get(0) throw
            // the same type of exception (MethodInvocationException)
          
            System.Exception lt = null;
           
            System.Exception at = null;
            try
            {
                evaluate("$list.get_item(0)");
            }
            catch (System.Exception t)
            {
                lt = t;
            }
            try
            {
                evaluate("$array.get_item(0)");
            }
            catch (System.Exception t)
            {
                at = t;
            }
            Assert.AreEqual(lt.GetType(), at.GetType());
        }

        private void checkResults(object array, object setme, bool compareToList)
        {
            context.Put("array", array);
            if (compareToList)
            {
                // create a list to match...
                context.Put("list", new System.Collections.ArrayList((System.Collections.ICollection)array));
            }

            // if the object to be set is null, then remove instead of Put
            if (setme != null)
            {
                context.Put("setme", setme);
            }
            else
            {
                context.Remove("setme");
            }

            if (PRINT_RESULTS)
            {
                Console.Out.WriteLine("Changing to an array of: " + array.GetType().GetElementType());
                Console.Out.WriteLine("Changing setme to: " + setme);
            }

            int size = ((System.Array)array).Length;
            checkResult("Count", System.Convert.ToString(size), compareToList);

            //bool isEmpty = (size == 0);
            //checkResult("isEmpty", System.Convert.ToString(isEmpty), compareToList);

            // check that the wrapping doesn't apply to java.lang.Object methods
            // such as toString() (for backwards compatibility).
            Assert.IsFalse(evaluate("$array").Equals(evaluate("$list")));

            for (int i = 0; i < size; i++)
            {
                // Put the index in the context, so we can try
                // both an explicit index and a reference index
                context.Put("index", (object)i);

                object value_Renamed = ((System.Array)array).GetValue(i);
                string get_Renamed = "get_item($index)";
                string set_Renamed = "set_item(" + i + ", $setme)";
                if (value_Renamed == null)
                {
                    checkEmptyResult(get_Renamed, compareToList);
                    // set should return null
                    checkEmptyResult(set_Renamed, compareToList);
                }
                else
                {
                    checkResult(get_Renamed, value_Renamed.ToString(), compareToList);
                    // set should return the old Get value
                   
                    checkResult(set_Renamed, string.Empty, compareToList);
                }

                // check that set() actually changed the value
                Assert.AreEqual(setme, ((System.Array)array).GetValue(i));

                // and check that Get() now returns setme
                if (setme == null)
                {
                    checkEmptyResult(get_Renamed, compareToList);
                }
                else
                {
                    checkResult(get_Renamed, setme.ToString(), compareToList);

                    // now check that contains() properly finds the new value
                    checkResult("Contains($setme)", "True", compareToList);
                }
            }
        }

        private void checkEmptyResult(string method, bool compareToList)
        {
            checkResult(method, "", compareToList);
        }

        private void checkResult(string method, string expected, bool compareToList)
        {
            string result = evaluate("$!array." + method);
            Assert.AreEqual(expected, result);

            string listResult = null;
            if (compareToList)
            {
                listResult = evaluate("$!list." + method);
                Assert.AreEqual(result, listResult);
            }

            if (PRINT_RESULTS)
            {
                Console.Out.WriteLine("    <$!array." + method + "> resolved to <" + result + ">");
                if (compareToList)
                {
                    System.Console.Out.WriteLine("    <$!list." + method + "> resolved to " + listResult + ">");
                }
            }
        }
    }
}
