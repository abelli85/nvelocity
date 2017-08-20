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

    using Runtime.Parser.Node;

    /// <summary> Checks that the equals method works correctly when caching method keys.
    /// 
    /// </summary>
    /// <author>  <a href="Will Glass-Husain">wglass@forio.com</a>
    /// </author>
    /// <version>  $Id: MethodCacheKeyTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class MethodCacheKeyTestCase 
    {
        [Test]
        public virtual void testMethodKeyCacheEquals()
        {
            System.Type[] elements1 = new System.Type[] { typeof(object) };
            ASTMethod.MethodCacheKey mck1 = new ASTMethod.MethodCacheKey("test", elements1);

            selfEqualsAssertions(mck1);

            System.Type[] elements2 = new System.Type[] { typeof(object) };
            ASTMethod.MethodCacheKey mck2 = new ASTMethod.MethodCacheKey("test", elements2);

            Assert.IsTrue(mck1.Equals(mck2));

            System.Type[] elements3 = new System.Type[] { typeof(string) };
            ASTMethod.MethodCacheKey mck3 = new ASTMethod.MethodCacheKey("test", elements3);

            Assert.IsFalse(mck1.Equals(mck3));

            System.Type[] elements4 = new System.Type[] { typeof(object) };
            ASTMethod.MethodCacheKey mck4 = new ASTMethod.MethodCacheKey("boo", elements4);

            Assert.IsFalse(mck1.Equals(mck4));

            /** check for potential NPE's **/
            System.Type[] elements5 = new Type[0];
            ASTMethod.MethodCacheKey mck5 = new ASTMethod.MethodCacheKey("boo", elements5);
            selfEqualsAssertions(mck5);

            System.Type[] elements6 = null;
            ASTMethod.MethodCacheKey mck6 = new ASTMethod.MethodCacheKey("boo", elements6);
            selfEqualsAssertions(mck6);

            System.Type[] elements7 = new System.Type[] { };
            ASTMethod.MethodCacheKey mck7 = new ASTMethod.MethodCacheKey("boo", elements7);
            selfEqualsAssertions(mck7);

            System.Type[] elements8 = new System.Type[] { null };
            ASTMethod.MethodCacheKey mck8 = new ASTMethod.MethodCacheKey("boo", elements8);
            selfEqualsAssertions(mck8);

            System.Type[] elements9 = new System.Type[] { typeof(object) };
            ASTMethod.MethodCacheKey mck9 = new ASTMethod.MethodCacheKey("boo", elements9);
            selfEqualsAssertions(mck9);
        }

        private void selfEqualsAssertions(ASTMethod.MethodCacheKey mck)
        {
            Assert.IsTrue(mck.Equals(mck));
            Assert.IsTrue(!mck.Equals(null));
            Assert.IsTrue(!mck.Equals((ASTMethod.MethodCacheKey)null));
        }
    }
}
