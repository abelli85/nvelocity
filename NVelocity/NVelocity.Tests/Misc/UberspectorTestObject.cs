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

namespace NVelocity.Tests.Misc
{
    public class UberspectorTestObject
    {
        /// <returns> the regular
        /// </returns>
        /// <param name="regular">the regular to set
        /// </param>
        public virtual string Regular { get; set; }

        /// <returns> the regularBool
        /// </returns>
        /// <param name="regularBool">the regularBool to set
        /// </param>
        public virtual bool RegularBool { get; set; }

        public virtual string Premium { get; set; }

        public virtual bool PremiumBool { get; set; }

        public virtual string Ambigous { get; set; }

        /// <param name="ambigous">the ambigous to set
        /// </param>
        public virtual void SetAmbigous(System.Text.StringBuilder ambigous)
        {
            this.Ambigous = ambigous.ToString();
        }
    }
}
