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

    using App;
    using Runtime;
    using Runtime.Resource.Loader;

    using Misc;

    /// <summary> Test the resource exists method
    /// 
    /// </summary>
    /// <version>  $Id: ResourceExistsTestCase.java 687191 2008-08-19 23:02:41Z nbubna $
    /// </version>
    [TestFixture]
    public class ResourceExistsTestCase : BaseTestCase
    {
        private VelocityEngine velocity;
        private string path = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/resourceexists";
        private TestLogChute logger = new TestLogChute();

        [SetUp]
        public void setUp()
        {
            try
            {
                velocity = new VelocityEngine();
                velocity.SetProperty("resource.loader", "file,string");
                velocity.SetProperty("file.resource.loader.path", path);
                velocity.SetProperty("string.resource.loader.class", typeof(StringResourceLoader).FullName);

                // parameters instance of logger
                logger.on();
                velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM, logger);
                velocity.SetProperty("runtime.log.logsystem.test.level", "debug");
            }
            catch (System.Exception e)
            {
                
                System.Console.Out.WriteLine("exception via gump: " + e);
                SupportClass.WriteStackTrace(e, Console.Error);
                System.Console.Out.WriteLine("log: " + logger.Log);
            }
        }

        [Test]
        public virtual void testFileResourceExists()
        {
            try
            {
                if (!velocity.ResourceExists("testfile.vm"))
                {
                    string msg = "testfile.vm was not found in path " + path;
                    System.Console.Out.WriteLine(msg);
                    System.Console.Out.WriteLine("Log was: " + logger.Log);
                    path = path + "/testfile.vm";
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    bool tmpBool;
                    if (System.IO.File.Exists(file.FullName))
                        tmpBool = true;
                    else
                        tmpBool = System.IO.Directory.Exists(file.FullName);
                    if (tmpBool)
                    {
                        System.Console.Out.WriteLine("file system found " + path);
                    }
                    else
                    {
                        System.Console.Out.WriteLine(file + " could not be found as a file");
                    }
                    Assert.Fail(msg);
                }
                if (velocity.ResourceExists("nosuchfile.vm"))
                {
                    string msg = "nosuchfile.vm should not have been found in path " + path;
                    System.Console.Out.WriteLine(msg);
                    Assert.Fail(msg);
                }
            }
            catch (System.Exception e)
            {
                //UPGRADE_TODO: 在 .NET 中，方法“java.lang.Throwable.toString”的等效项可能返回不同的值。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.Console.Out.WriteLine("exception via gump: " + e);
                SupportClass.WriteStackTrace(e, Console.Error);
                System.Console.Out.WriteLine("log: " + logger.Log);
            }
        }

        [Test]
        public virtual void testStringResourceExists()
        {
            try
            {
                Assert.IsFalse(velocity.ResourceExists("foo.vm"));
                StringResourceLoader.GetRepository().PutStringResource("foo.vm", "Make it so!");
                Assert.IsTrue(velocity.ResourceExists("foo.vm"));
            }
            catch (System.Exception e)
            {
                
                System.Console.Out.WriteLine("exception via gump: " + e);
                SupportClass.WriteStackTrace(e, Console.Error);
                System.Console.Out.WriteLine("log: " + logger.Log);
            }
        }
    }
}
