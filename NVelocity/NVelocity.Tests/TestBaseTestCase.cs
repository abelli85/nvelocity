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

    /// <summary> I keep breaking the getFileName method all the time...</summary>
    [TestFixture]
    public class TestBaseTestCase : BaseTestCase
    {
        [Test]
        public virtual void testGetFileName()
        {
            
            string fs = System.IO.Path.DirectorySeparatorChar.ToString();
           
            string pwd = System.Environment.CurrentDirectory;

            string root = new System.IO.FileInfo("/").FullName;

            Assert.AreEqual(pwd + fs + "baz" + fs + "foo.bar", getFileName("baz", "foo", "bar"));
            Assert.AreEqual(root + "baz" + fs + "foo.bar", getFileName(root + "baz", "foo", "bar"));
            Assert.AreEqual(root + "foo.bar", getFileName("baz", root + "foo", "bar"));
            Assert.AreEqual(root + "foo.bar", getFileName(root + "baz", root + "foo", "bar"));
            Assert.AreEqual("", getFileName(null, "", ""));
            Assert.AreEqual(root + "", getFileName("", "", ""));
            Assert.AreEqual(".x", getFileName(null, "", "x"));
            Assert.AreEqual(root + ".x", getFileName("", "", "x"));
            Assert.AreEqual("foo.bar", getFileName(null, "foo", "bar"));
            Assert.AreEqual(root + "foo.bar", getFileName(null, root + "foo", "bar"));
            Assert.AreEqual(root + "foo.bar", getFileName("", "foo", "bar"));
            Assert.AreEqual(root + "foo.bar", getFileName("", root + "foo", "bar"));
        }
    }
}
