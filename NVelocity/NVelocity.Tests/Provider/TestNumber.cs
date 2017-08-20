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
    using NVelocity.Util;

    /// <summary> Used for testing purposes to check that an object implementing TemplateNumber
    /// will be treated as a Number.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    public class TestNumber : ITemplateNumber
    {
        public virtual  System.ValueType AsNumber
        {
            get
            {
                return n;
            }

        }

        private System.ValueType n;

        public TestNumber(double val)
        {
            n = (double)val;
        }
    }
}
