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
    using NVelocity.Util.Introspection;


    /// <summary> Checks that arrays are cached correctly in the Introspector.
    /// 
    /// </summary>
    /// <author>  <a href="Alexey Pachenko">alex+news@olmisoft.com</a>
    /// </author>
    /// <version>  $Id: IntrospectionCacheDataTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class IntrospectionCacheDataTestCase 
    {

        private class CacheHitCountingVelocityContext : VelocityContext
        {
            public int cacheHit = 0;

            public override IntrospectionCacheData ICacheGet(object key)
            {
                IntrospectionCacheData result = base.ICacheGet(key);
                if (result != null)
                {
                    ++cacheHit;
                }
                return result;
            }
        }

        [Test]
        public virtual void testCache()
        {
            CacheHitCountingVelocityContext context = new CacheHitCountingVelocityContext();
            context.Put("this", this);
            System.IO.StringWriter w = new System.IO.StringWriter();
            Velocity.Evaluate(context, w, "test", "$this.exec('a')$this.exec('b')");
            Assert.AreEqual("[a][b]", w.ToString());
            Assert.IsTrue(context.cacheHit > 0);
        }


        /// <summary> For use when acting as a context reference.
        /// 
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual string exec(string value_Renamed)
        {
            return "[" + value_Renamed + "]";
        }
    }
}
