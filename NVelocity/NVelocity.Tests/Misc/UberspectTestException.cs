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
    using System;

    using NVelocity.Util.Introspection;

    /// <summary> Exception that returns an Info object for testing after a introspection problem.
    /// This extends Error so that it will stop parsing and allow
    /// internal Info to be examined.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <author>  <a href="mailto:isidore@setgame.com">Llewellyn Falco</a>
    /// </author>
    /// <version>  $Id: UberspectTestException.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [Serializable]
    public class UberspectTestException : System.SystemException
    {
        virtual public Info Info
        {
            get
            {
                return info;
            }

        }
        public override string Message
        {
            get
            {
                return base.Message + "\n failed at " + info;
            }

        }

        internal Info info;

        public UberspectTestException(string message, Info i)
            : base(message)
        {
            info = i;
        }
    }
}
