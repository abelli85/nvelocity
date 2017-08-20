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

namespace NVelocity.Tests.Provider
{
    /// <summary> Provides overloaded methods for testing method execution within a foreach</summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ForeachMethodCallHelper.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    public class ForeachMethodCallHelper
    {
        public virtual string GetFoo(System.Int32 v)
        {
            return "int " + v;
        }
        public virtual string GetFoo(string v)
        {
            return "str " + v;
        }
    }
}
