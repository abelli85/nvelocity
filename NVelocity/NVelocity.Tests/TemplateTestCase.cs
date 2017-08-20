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

    using Provider;

    /// <summary> Easily Add test cases which Evaluate templates and check their output.
    /// 
    /// NOTE:
    /// This class DOES NOT extend RuntimeTestCase because the TemplateTestSuite
    /// already initializes the Velocity runtime and adds the template
    /// test cases. Having this class extend RuntimeTestCase causes the
    /// Runtime to be initialized twice which is not good. I only discovered
    /// this after a couple hours of wondering why all the properties
    /// being setup were ending up as Vectors. At first I thought it
    /// was a problem with the Configuration class, but the Runtime
    /// was being initialized twice: so the first time the property
    /// is seen it's stored as a String, the second time it's seen
    /// the Configuration class makes a Vector with both Strings.
    /// As a result all the getBoolean(property) calls were failing because
    /// the Configurations class was trying to create a Boolean from
    /// a Vector which doesn't really work that well. I have learned
    /// my lesson and now have to Add some code to make sure the
    /// Runtime isn't initialized more then once :-)
    /// 
    /// </summary>
    /// <author>  <a href="mailto:dlr@finemaltcoding.com">Daniel Rall</a>
    /// </author>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <author>  <a href="mailto:jon@latchkey.com">Jon S. Stevens</a>
    /// </author>
    /// <version>  $Id: TemplateTestCase.java 685369 2008-08-12 23:35:12Z nbubna $
    /// </version>
    [TestFixture]
    public class TemplateTestCase : BaseTestCase
    {
        /// <summary> The base file name of the template and comparison file (i.e. array for
        /// array.vm and array.cmp).
        /// </summary>
        protected internal string baseFileName;

        private TestProvider provider;
        private System.Collections.ArrayList al;
        private System.Collections.Hashtable h;
        private VelocityContext context;
        private VelocityContext context1;
        private VelocityContext context2;
        private System.Collections.ArrayList vec;

        /// <summary> Creates a new instance.
        /// 
        /// </summary>
        /// <param name="baseFileName">The base name of the template and comparison file to
        /// use (i.e. array for array.vm and array.cmp).
        /// </param>
        public TemplateTestCase(string baseFileName)
        {
            this.baseFileName = baseFileName;
        }


        /// <summary> Sets up the test.</summary>
       [SetUp]
        public void setUp()
        {
            provider = new TestProvider();
            al = provider.Customers;
            h = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

            h["Bar"] = "this is from a hashtable!";
            h["Foo"] = "this is from a hashtable too!";

            /*
            *  lets set up a vector of objects to test late introspection. See ASTMethod.java
            */

            vec = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            vec.Add(new System.Text.StringBuilder("string1").ToString());
            vec.Add(new System.Text.StringBuilder("string2").ToString());

            /*
            *  set up 3 chained contexts, and Add our data
            *  throught the 3 of them.
            */

            context2 = new VelocityContext();
            context1 = new VelocityContext(context2);
            context = new VelocityContext(context1);

            context.Put("provider", provider);
            context1.Put("name", "jason");
            context1.Put("name2", new System.Text.StringBuilder("jason"));
            context1.Put("name3", new System.Text.StringBuilder("geoge"));
            context2.Put("providers", provider.Customers2);
            context.Put("list", al);
            context1.Put("hashtable", h);
           
            context2.Put("hashmap", new System.Collections.Hashtable());
            context2.Put("search", provider.Search);
            context.Put("relatedSearches", provider.RelSearches);
            context1.Put("searchResults", provider.RelSearches);
            context2.Put("stringarray", provider.Array);
            context.Put("vector", vec);
            context.Put("mystring", new System.Text.StringBuilder().ToString());
            context.Put("Floog", "floogie woogie");
            context.Put("boolobj", new BoolObj());

            /*
            *  we want to make sure we test all types of iterative objects
            *  in #foreach()
            */

            object[] oarr = new object[] { "a", "b", "c", "d" };
            int[] intarr = new int[] { 10, 20, 30, 40, 50 };

            context.Put("collection", vec);
            context2.Put("iterator", vec.GetEnumerator());
            context1.Put("map", h);
            context.Put("obarr", oarr);
            context.Put("enumerator", vec.GetEnumerator());
            context.Put("intarr", intarr);

            // Add some Numbers
            context.Put("int1", (object)1000);
            context.Put("long1", (object)10000000000L);
            context.Put("float1", (object)1000.1234);
            context.Put("double1", (object)10000000000d);

            // Add a TemplateNumber
            context.Put("templatenumber1", new TestNumber(999.125));

            /**
            * Test #foreach() with a list containing nulls
            */
            System.Collections.ArrayList nullList = new System.Collections.ArrayList();
            nullList.Add("a");
            nullList.Add("b");
            nullList.Add(null);
            nullList.Add("d");
            context.Put("nullList", nullList);

            // test silent references with a null tostring
            context.Put("nullToString", new NullToStringObject());
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public void runTest()
        {
            Template template = RuntimeSingleton.GetTemplate(getFileName(null, baseFileName, TemplateTestBase_Fields.TMPL_FILE_EXT));

            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /* Get the file to write to */
          
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, baseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            /* process the template */
            template.Merge(context, writer);

            /* close the file */
            writer.Flush();
            writer.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, baseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template " + getFileName(TemplateTestBase_Fields.RESULT_DIR, baseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT) + " did not match expected output");
            }
        }
    }
}
