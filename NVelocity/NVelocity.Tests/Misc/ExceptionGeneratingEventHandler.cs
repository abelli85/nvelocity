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
    using App.Event;

    /// <summary> Event handlers that always throws an exception.  Used to test
    /// that RuntimeExceptions are passed through.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ExceptionGeneratingEventHandler.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    public class ExceptionGeneratingEventHandler : IIncludeEventHandler, IMethodExceptionEventHandler, INullSetEventHandler, IReferenceInsertionEventHandler
    {

        public virtual string IncludeEvent(string includeResourcePath, string currentResourcePath, string directiveName)
        {
            throw new System.SystemException("exception");
        }

        public virtual object MethodException(System.Type claz, string method, System.Exception e)
        {
            throw new System.SystemException("exception");
        }

        public virtual bool shouldLogOnNullSet(string lhs, string rhs)
        {
            throw new System.SystemException("exception");
        }

        public virtual object ReferenceInsert(string reference, object value_Renamed)
        {
            throw new System.SystemException("exception");
        }
    }
}
