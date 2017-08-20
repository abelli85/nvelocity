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

    using NVelocity.Util;
    using Runtime;
    using Runtime.Resource.Util;

    /// <summary> Base test case that provides a few utility methods for
    /// the rest of the tests.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:dlr@finemaltcoding.com">Daniel Rall</a>
    /// </author>
    /// <version>  $Id: BaseTestCase.java 689111 2008-08-26 15:27:06Z nbubna $
    /// </version>
    public abstract class BaseTestCase
    {
        /// <summary> Concatenates the file name parts together appropriately.
        /// 
        /// </summary>
        /// <returns> The full path to the file.
        /// </returns>
        protected internal static string getFileName(string dir, string base_Renamed, string ext)
        {
            return getFileName(dir, base_Renamed, ext, false);
        }

        protected internal static string getFileName(string dir, string base_Renamed, string ext, bool mustExist)
        {
            string filename = string.Empty;

            try
            {
                System.Text.StringBuilder buf = new System.Text.StringBuilder();

                buf.Append(base_Renamed);

                if (!string.IsNullOrEmpty(ext))
                {
                    buf.Append('.').Append(ext);
                }

                if (!string.IsNullOrEmpty(dir))
                {

                    filename = System.IO.Path.Combine(dir, buf.ToString());
                }
                else
                {
                    filename = buf.ToString();
                }

                if (mustExist)
                {
                    System.IO.FileInfo testFile = new System.IO.FileInfo(filename);

                    bool tmpBool;
                    if (System.IO.File.Exists(testFile.FullName))
                        tmpBool = true;
                    else
                        tmpBool = System.IO.Directory.Exists(testFile.FullName);
                    if (!tmpBool)
                    {
                        Assert.Fail("getFileName() result " + testFile.FullName + " does not exist!");
                    }

                    if (!System.IO.File.Exists(testFile.FullName))
                    {
                        Assert.Fail("getFileName() result " + testFile.FullName + " is not a file!");
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                Assert.Fail("IO Exception while running getFileName(" + dir + ", " + base_Renamed + ", " + ext + ", " + mustExist + "): " + e.Message);
            }

            return filename;
        }

        /// <summary> Assures that the results directory exists.  If the results directory
        /// cannot be created, fails the test.
        /// </summary>
        protected internal static void assureResultsDirectoryExists(string resultsDirectory)
        {
            System.IO.FileInfo dir = new System.IO.FileInfo(resultsDirectory);
            bool tmpBool;
            if (System.IO.File.Exists(dir.FullName))
                tmpBool = true;
            else
                tmpBool = System.IO.Directory.Exists(dir.FullName);
            if (!tmpBool)
            {
                string msg = "Template results directory (" + resultsDirectory + ")does not exist";
                RuntimeSingleton.Log.Info(msg);

                try
                {
                    System.IO.Directory.CreateDirectory(dir.FullName);

                    RuntimeSingleton.Log.Info("Created template results directory");
                    //caveman hack to Get gump to give more Info
                    System.Console.Out.WriteLine("Created template results directory: " + resultsDirectory);
                }
                catch
                {
                    string errMsg = "Unable to create template results directory";
                    RuntimeSingleton.Log.Warn(errMsg);
                    //caveman hack to Get gump to give more Info
                    System.Console.Out.WriteLine(errMsg);
                    Assert.Fail(errMsg);
                }
            }
        }


        /// <summary> Normalizes lines to account for platform differences.  Macs use
        /// a single \r, DOS derived operating systems use \r\n, and Unix
        /// uses \n.  Replace each with a single \n.
        /// 
        /// </summary>
        /// <author>  <a href="mailto:rubys@us.ibm.com">Sam Ruby</a>
        /// </author>
        /// <returns> source with all line terminations changed to Unix style
        /// </returns>
        protected internal virtual string normalizeNewlines(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "\r[\r]?[\n]", "\n");
        }

        /// <summary> Returns whether the processed template matches the
        /// content of the provided comparison file.
        /// 
        /// </summary>
        /// <returns> Whether the output matches the contents
        /// of the comparison file.
        /// 
        /// </returns>
        /// <exception cref="Exception">Test failure condition.
        /// </exception>
        protected internal virtual bool isMatch(string resultsDir, string compareDir, string baseFileName, string resultExt, string compareExt,System.Text.Encoding encoding)
        {
            string result = StringUtils.FileContentsToString(getFileName(resultsDir, baseFileName, resultExt, true),encoding);

            return isMatch(result, compareDir, baseFileName, compareExt,encoding);
        }


        protected internal virtual string getFileContents(string dir, string baseFileName, string ext,System.Text.Encoding encoding)
        {
            return StringUtils.FileContentsToString(getFileName(dir, baseFileName, ext, true),encoding);
        }

        /// <summary> Returns whether the processed template matches the
        /// content of the provided comparison file.
        /// 
        /// </summary>
        /// <returns> Whether the output matches the contents
        /// of the comparison file.
        /// 
        /// </returns>
        /// <exception cref="Exception">Test failure condition.
        /// </exception>
        protected internal virtual bool isMatch(string result, string compareDir, string baseFileName, string compareExt, System.Text.Encoding encoding)
        {
            string compare = StringUtils.FileContentsToString(getFileName(compareDir, baseFileName, compareExt, true), encoding);

            /*
            *  normalize each wrt newline
            */

            return normalizeNewlines(result).Equals(normalizeNewlines(compare));
        }

        /// <summary> Turns a base file name into a test case name.
        /// 
        /// </summary>
        /// <param name="s">The base file name.
        /// </param>
        /// <returns>  The test case name.
        /// </returns>
        protected internal static string getTestCaseName(string s)
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder();
            name.Append(System.Char.ToUpper(s[0]));
            name.Append(s.Substring(1, (s.Length) - (1)).ToLower());
            return name.ToString();
        }
    }
}
