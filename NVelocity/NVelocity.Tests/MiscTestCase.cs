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
    using System.Collections.Generic;

    using NUnit.Framework;

    using NVelocity.Util;
    using Runtime;
    

    /// <summary> Test case for any miscellaneous stuff.  If it isn't big, and doesn't fit
    /// anywhere else, it goes here
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@apache.org">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: MiscTestCase.java 473760 2006-11-11 16:55:40Z wglass $
    /// </version>
    [TestFixture]
    public class MiscTestCase : BaseTestCase
    {
        [Test]
        public virtual void testRuntimeInstanceProperties()
        {
            // check that runtime instance properties can be set and retrieved
            RuntimeInstance ri = new RuntimeInstance();
            ri.SetProperty("baabaa.test", "the answer");
            Assert.AreEqual("the answer", ri.GetProperty("baabaa.test"));
        }

        [Test]
        public virtual void testStringUtils()
        {
            /*
            *  some StringUtils tests
            */

            string eol = "XY";

            string arg = "XY";
            string res = StringUtils.Chop(arg, 1, eol);
            Assert.IsTrue(res.Equals(""),"Test 1");

            arg = "X";
            res = StringUtils.Chop(arg, 1, eol);
            Assert.IsTrue(res.Equals(""),"Test 2");

            arg = "ZXY";
            res = StringUtils.Chop(arg, 1, eol);
            Assert.IsTrue(res.Equals("Z"),"Test 3");


            arg = "Hello!";
            res = StringUtils.Chop(arg, 2, eol);
            Assert.IsTrue(res.Equals("Hell"),"Test 4" );

            arg = null;
            res = StringUtils.NullTrim(arg);
            Assert.IsNull(arg);

            arg = " test ";
            res = StringUtils.NullTrim(arg);
            Assert.AreEqual("test", res);

            arg = "test";
            res = StringUtils.NullTrim(arg);
            Assert.AreEqual("test", res);

            IList<string> list = null;
            Assert.IsNull(StringUtils.TrimStrings(list));

            list = new List<string>();
            Assert.AreEqual(new System.Collections.ArrayList(), StringUtils.TrimStrings(list));

            list.Add("test");
            list.Add(" abc");
            StringUtils.TrimStrings(list);
            Assert.AreEqual("test", list[0]);
            Assert.AreEqual("abc", list[1]);
        }
    }
}
