﻿/*
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
    using System;

    /// <summary> Any object in the context which implements TemplateNumber will be treated
    /// as a number for the purposes of arithmetic operations and comparison.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <since> 1.5
    /// </since>
    public interface ITemplateNumber
    {
        /// <summary> Returns a Number that can be used in a template.</summary>
        /// <returns> A Number that can be used in a template.
        /// </returns>
        ValueType AsNumber
        {
            get;

        }
    }
}
