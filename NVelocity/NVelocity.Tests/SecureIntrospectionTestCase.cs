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
    using System.Linq;

    using NUnit.Framework;

    using App;
    using Context;
    using NVelocity.Util.Introspection;
    using Runtime;
    
    /// <summary> Checks that the secure introspector is working properly.
    /// 
    /// </summary>
    /// <author>  <a href="Will Glass-Husain">wglass@forio.com</a>
    /// </author>
    /// <version>  $Id: SecureIntrospectionTestCase.java 509094 2007-02-19 05:17:09Z wglass $
    /// </version>
    [TestFixture]
    public class SecureIntrospectionTestCase : BaseTestCase
    {
        virtual public string Property
        {
            get
            {
                return testProperty;
            }

            set
            {
                testProperty = value;
            }

        }
        virtual public System.Collections.ICollection Collection
        {
            get
            {
                
                System.Collections.Generic.HashSet<string> c = new System.Collections.Generic.HashSet<string>();
                c.Add("aaa");
                c.Add("bbb");
                c.Add("ccc");
           
                return c.ToArray();
            }

        }



        private string[] badTemplateStrings = new string[] { "$test.Class.Methods", "$test.Class.ClassLoader", "$test.Class.ClassLoader.loadClass('java.util.HashMap').newInstance().size()" };

        private string[] goodTemplateStrings = new string[] { "#foreach($item in $test.collection)$item#end", "$test.Class.Name", "#set($test.Property = 'abc')$test.Property", "$test.aTestMethod()" };

        /// <summary>  Test to see that "dangerous" methods are forbidden</summary>
        /// <exception cref="Exception">
        /// </exception>
        [Test]
        public virtual void testBadMethodCalls()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.UBERSPECT_CLASSNAME, typeof(SecureUberspector).FullName);
            ve.Init();

            /*
            * all of the following method calls should not work
            */
            doTestMethods(ve, badTemplateStrings, false);
        }

        /// <summary>  Test to see that "dangerous" methods are forbidden</summary>
        /// <exception cref="Exception">
        /// </exception>
        [Test]
        public virtual void testGoodMethodCalls()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.UBERSPECT_CLASSNAME, typeof(SecureUberspector).FullName);
            ve.Init();

            /*
            * all of the following method calls should not work
            */
            doTestMethods(ve, goodTemplateStrings, true);
        }

        private void doTestMethods(VelocityEngine ve, string[] templateStrings, bool shouldeval)
        {
            IContext c = new VelocityContext();
            c.Put("test", this);

            try
            {
                for (int i = 0; i < templateStrings.Length; i++)
                {
                    if (shouldeval && !doesStringEvaluate(ve, c, templateStrings[i]))
                    {
                        Assert.Fail("Should have evaluated: " + templateStrings[i]);
                    }

                    if (!shouldeval && doesStringEvaluate(ve, c, templateStrings[i]))
                    {
                        Assert.Fail("Should not have evaluated: " + templateStrings[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                
                Assert.Fail(e.ToString());
            }
        }

        private bool doesStringEvaluate(VelocityEngine ve, IContext c, string inputString)
        {
            // assume that an evaluation is bad if the input and result are the same (e.g. a bad reference)
            // or the result is an empty string (e.g. bad #foreach)
            
            System.IO.TextWriter w = new System.IO.StringWriter();
            ve.Evaluate(c, w, "foo", inputString);
          
            string result = w.ToString();
            return (result.Length > 0) && !result.Equals(inputString);
        }

        private string testProperty;

        public virtual int aTestMethod()
        {
            return 1;
        }
    }
}
