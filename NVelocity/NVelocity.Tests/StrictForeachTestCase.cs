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

    using Exception;
    using Runtime;

    /// <summary> This class tests support for strict foreach mode.</summary>
    [TestFixture]
    public class StrictForeachTestCase : BaseEvalTestCase
    {
        [SetUp]
        public void setUp()
        {
            base.setUp();
            engine.SetProperty(RuntimeConstants.SKIP_INVALID_ITERATOR, (object)false);
            context.Put("good", new GoodIterable());
            context.Put("bad", new BadIterable());
            context.Put("ugly", new UglyIterable());
        }

        [Test]
        public virtual void testGood()
        {
            try
            {
                evaluate("#foreach( $i in $good )$i#end");
            }
            catch (VelocityException ve)
            {
                Assert.Fail("Doing #foreach on $good should not have exploded!");
            }
        }

        [Test]
        public virtual void testBad()
        {
            try
            {
                evaluate("#foreach( $i in $bad )$i#end");
                Assert.Fail("Doing #foreach on $bad should have exploded!");
            }
            catch (VelocityException ve)
            {
                // success!
            }
        }

        [Test]
        public virtual void testUgly()
        {
            try
            {
                evaluate("#foreach( $i in $ugly )$i#end");
                Assert.Fail("Doing #foreach on $ugly should have exploded!");
            }
            catch (VelocityException ve)
            {
                // success!
            }
        }


        public class GoodIterable
        {
            public virtual System.Collections.IEnumerator iterator()
            {
                return new System.Collections.ArrayList().GetEnumerator();
            }
        }

        public class BadIterable
        {
            public virtual object iterator()
            {
                return new object();
            }
        }

        public class UglyIterable
        {
            public virtual System.Collections.IEnumerator iterator()
            {
                return null;
            }
        }
    }
}
