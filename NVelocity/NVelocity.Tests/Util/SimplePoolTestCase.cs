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

namespace NVelocity.Util
{
    using NUnit.Framework;

    /// <summary>  Simpletest for the SimplePool
	/// 
	/// </summary>
	/// <version>  $Id: SimplePoolTestCase.java 463298 2006-10-12 16:10:32Z henning $
	/// </version>
	[TestFixture]
    public class SimplePoolTestCase
	{
        [Test]
		public virtual void  testPool()
		{
			SimplePool<object> sp = new SimplePool<object>(10);
			
			for (int i = 0; i < 10; i++)
			{
				sp.Put((object) i);
			}
			
			for (int i = 9; i >= 0; i--)
			{
				System.Int32 obj = (System.Int32) sp.Get();
				
				Assert.IsTrue(i == obj);
			}
			
			object[] pool = sp.Pool;
			
			for (int i = 0; i < 10; i++)
			{
				Assert.IsTrue(pool[i] == null,"Pool not empty");
			}
		}
	}
}

