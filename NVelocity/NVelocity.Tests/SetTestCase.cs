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
    using Runtime;

    /// <summary> Test that an instance of a ResourceLoader can be successfully passed in.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: SetTestCase.java 599003 2007-11-28 13:55:49Z cbrisson $
    /// </version>
    [TestFixture]
    public class SetTestCase : BaseTestCase
    {
        /// <summary> VTL file extension.</summary>
        private const string TMPL_FILE_EXT = "vm";

        /// <summary> Comparison file extension.</summary>
        private const string CMP_FILE_EXT = "cmp";

        /// <summary> Comparison file extension.</summary>
        private const string RESULT_FILE_EXT = "res";

        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private static readonly string FILE_RESOURCE_LOADER_PATH = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/set";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string RESULTS_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/set";

        /// <summary> Results relative to the build directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/set/Compare";


        [SetUp]
        public void setUp()
        {
            assureResultsDirectoryExists(RESULTS_DIR);
        }


        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testSetNull()
        {
            /**
            * Check that #set does not accept nulls
            */

            VelocityEngine ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            checkTemplate(ve, "set1");

            /**
            * Check that setting the property is the same as the default
            */
            ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.SET_NULL_ALLOWED, "false");
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            checkTemplate(ve, "set1");

            /**
            * Check that #set can accept nulls, and has the correct behaviour for complex LHS
            */
            ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.SET_NULL_ALLOWED, "true");
            ve.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            checkTemplate(ve, "set2");
        }

        public virtual void checkTemplate(VelocityEngine ve, string templateName)
        {
            Template template;
            System.IO.FileStream fos;

            System.IO.StreamWriter fwriter;
            IContext context;

            template = ve.GetTemplate(getFileName(null, templateName, TMPL_FILE_EXT));


            fos = new System.IO.FileStream(getFileName(RESULTS_DIR, templateName, RESULT_FILE_EXT), System.IO.FileMode.Create);


            fwriter = new System.IO.StreamWriter(fos, System.Text.Encoding.Default);

            context = new VelocityContext();
            template.Merge(context, fwriter);
            fwriter.Flush();
            fwriter.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, templateName, RESULT_FILE_EXT, CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }
    }
}
