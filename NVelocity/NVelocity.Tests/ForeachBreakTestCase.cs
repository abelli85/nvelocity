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

    /// <summary> This class tests the break directive within Foreach loop.</summary>
    [TestFixture]
    public class ForeachBreakTestCase : BaseEvalTestCase
    {
        
        /// <summary> Tests break directive with a couple of iterations.</summary>
        [Test]
        public virtual void testConditionalBreakDirective()
        {
            assertEvalEquals("1, 2, 3, 4, 5", "#foreach($i in [1..10])$i#if($i > 4)#break#end, #end");
        }

        /// <summary> Tests break directive with immediate break.</summary>
        [Test]
        public virtual void testUnconditionalBreakDirective()
        {
            assertEvalEquals("1", "#foreach($i in [1..5])$i#break #end");
        }

        [Test]
        public virtual void testNestedForeach()
        {
            assertEvalEquals("~~~, ~~, ~, ", "#foreach($i in [1..3])#foreach($j in [2..4])#if($i*$j >= 8)#break#end~#end, #end");
        }
    }
}
