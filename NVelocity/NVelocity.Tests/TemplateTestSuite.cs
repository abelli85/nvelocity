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

    using App;
    using Runtime;

    using Misc;

    /// <summary> Test suite for Templates.
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
    /// <version>  $Id: TemplateTestSuite.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    
    public class TemplateTestSuite
    {
       
        private System.Collections.Specialized.NameValueCollection testProperties;

        /// <summary> Creates an instace of the Apache Velocity test suite.</summary>
        public TemplateTestSuite()
        {
            try
            {
                Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.FILE_RESOURCE_LOADER_PATH);

                Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

                Velocity.Init();

                
                testProperties = new System.Collections.Specialized.NameValueCollection();
               
                new System.IO.FileStream(TemplateTestBase_Fields.TEST_CASE_PROPERTIES, System.IO.FileMode.Open, System.IO.FileAccess.Read);
             
                testProperties = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
            }
            catch (System.Exception e)
            {
                System.Console.Error.WriteLine("Cannot setup TemplateTestSuite!");
                SupportClass.WriteStackTrace(e, Console.Error);
                System.Environment.Exit(1);
            }

            addTemplateTestCases();
        }

        /// <summary> Adds the template test cases to run to this test suite.  Template test
        /// cases are listed in the <code>TEST_CASE_PROPERTIES</code> file.
        /// </summary>
        private void addTemplateTestCases()
        {
            string template;
            for (int i = 1; ; i++)
            {
                template = testProperties.Get(getTemplateTestKey(i));

                if (template != null)
                {
                    System.Console.Out.WriteLine("Adding TemplateTestCase : " + template);
                    //addTest(new TemplateTestCase(template));
                }
                else
                {
                    // Assume we're done adding template test cases.
                    break;
                }
            }
        }

        /// <summary> Macro which returns the properties file key for the specified template
        /// test number.
        /// 
        /// </summary>
        /// <param name="nbr">The template test number to return a property key for.
        /// </param>
        /// <returns>    The property key.
        /// </returns>
        private static string getTemplateTestKey(int nbr)
        {
            return ("test.template." + nbr);
        }
    }
}
