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

    using Commons.Collections;

    /// <summary> Tests for the Commons ExtendedProperties class. This is an identical
    /// copy of the ConfigurationTestCase, which will disappear when
    /// the Configuration class does
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: CommonsExtPropTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class CommonsExtPropTestCase : BaseTestCase
    {
        /// <summary> Comparison directory.</summary>
        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/configuration/Compare";

        /// <summary> Results directory.</summary>
        private static readonly string RESULTS_DIR =TemplateTestBase_Fields.TEST_RESULT_DIR + "/configuration";

        /// <summary> Test configuration</summary>
        private static readonly string TEST_CONFIG = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/configuration/test-config.properties";


        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testExtendedProperties()
        {
            assureResultsDirectoryExists(RESULTS_DIR);

            ExtendedProperties c = new ExtendedProperties(TEST_CONFIG);

            System.IO.TextWriter result = new System.IO.StreamWriter(getFileName(RESULTS_DIR, "output", "res"), false, System.Text.Encoding.Default);

            message(result, "Testing order of keys ...");
            showIterator(result, c.Keys.GetEnumerator());

            message(result, "Testing retrieval of CSV values ...");
            showVector(result, c.GetVector("resource.loader"));

            message(result, "Testing subset(prefix).getKeys() ...");
            ExtendedProperties subset = c.Subset("file.resource.loader");
            showIterator(result, subset.Keys.GetEnumerator());

            message(result, "Testing getVector(prefix) ...");
            showVector(result, subset.GetVector("path"));

            message(result, "Testing getString(key) ...");
            result.Write(c.GetString("config.string.value"));
            result.Write("\n\n");

            message(result, "Testing getBoolean(key) ...");
          
            result.Write(c.GetBoolean("config.boolean.value").ToString());
            result.Write("\n\n");

            message(result, "Testing getByte(key) ...");
            result.Write(((byte)c.GetByte("config.byte.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing getShort(key) ...");
            result.Write(((short)c.GetInteger("config.short.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing getInt(key) ...");
            result.Write(((System.Int32)c.GetInt("config.int.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing getLong(key) ...");
            result.Write(((long)c.GetLong("config.long.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing getFloat(key) ...");
          
            result.Write(((float)c.GetFloat("config.float.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing getDouble(key) ...");
            result.Write(((double)c.GetDouble("config.double.value")).ToString());
            result.Write("\n\n");

            message(result, "Testing escaped-comma scalar...");
            result.Write(c.GetString("escape.comma1"));
            result.Write("\n\n");

            message(result, "Testing escaped-comma vector...");
            showVector(result, c.GetVector("escape.comma2"));
            result.Write("\n\n");

            result.Flush();
            result.Close();

            if (!isMatch(RESULTS_DIR, COMPARE_DIR, "output", "res", "cmp",System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }

      
        private void showIterator(System.IO.TextWriter result, System.Collections.IEnumerator i)
        {
           
            while (i.MoveNext())
            {
                
                result.Write((string)i.Current);
                result.Write("\n");
            }
            result.Write("\n");
        }

       
        private void showVector(System.IO.TextWriter result, System.Collections.ArrayList v)
        {
            for (int j = 0; j < v.Count; j++)
            {
                result.Write((string)v[j]);
                result.Write("\n");
            }
            result.Write("\n");
        }

        
        private void message(System.IO.TextWriter result, string message)
        {
            result.Write("--------------------------------------------------\n");
            result.Write(message + "\n");
            result.Write("--------------------------------------------------\n");
            result.Write("\n");
        }
    }
}
