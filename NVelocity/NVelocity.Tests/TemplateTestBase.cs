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
    /// <summary> This is a base interface that contains a bunch of static final
    /// strings that are of use when testing templates.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jon@latchkey.com">Jon S. Stevens</a>
    /// </author>
    /// <version>  $Id: TemplateTestBase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    public struct TemplateTestBase_Fields
    {
        /// <summary> Directory relative to the distribution root, where the
        /// values to Compare test results to are stored.
        /// </summary>
        public const string TEST_COMPARE_DIR = "test";
        /// <summary> Directory relative to the distribution root, where the
        /// test cases should Put their output
        /// </summary>
        public const string TEST_RESULT_DIR = "build.test";
        /// <summary> VTL file extension.</summary>
        public const string TMPL_FILE_EXT = "vm";
        /// <summary> Comparison file extension.</summary>
        public const string CMP_FILE_EXT = "cmp";
        /// <summary> Comparison file extension.</summary>
        public const string RESULT_FILE_EXT = "res";
        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        public readonly static string FILE_RESOURCE_LOADER_PATH;
        /// <summary> Properties file that lists which template tests to run.</summary>
        public readonly static string TEST_CASE_PROPERTIES;
        /// <summary> Results relative to the build directory.</summary>
        public readonly static string RESULT_DIR;
        /// <summary> Results relative to the build directory.</summary>
        public readonly static string COMPARE_DIR;

        static TemplateTestBase_Fields()
        {
            FILE_RESOURCE_LOADER_PATH = TEST_COMPARE_DIR + "/templates";
            TEST_CASE_PROPERTIES = FILE_RESOURCE_LOADER_PATH + "/templates.properties";
            RESULT_DIR = TEST_RESULT_DIR + "/templates";
            COMPARE_DIR = FILE_RESOURCE_LOADER_PATH + "/Compare";
        }
    }
}
