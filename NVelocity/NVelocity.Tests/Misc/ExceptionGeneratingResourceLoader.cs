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

namespace NVelocity.Tests.Misc
{
    using Commons.Collections;
    using Runtime.Resource;
    using Runtime.Resource.Loader;

    /// <summary> Resource Loader that always throws an exception.  Used to test
    /// that RuntimeExceptions are passed through.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ExceptionGeneratingResourceLoader.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    public class ExceptionGeneratingResourceLoader : ResourceLoader
    {

        public override void Init(ExtendedProperties configuration)
        {
        }

        public override System.IO.Stream GetResourceStream(string source)
        {
            throw new System.SystemException("exception");
        }

        public override bool IsSourceModified(Resource resource)
        {
            return false;
        }

        public override long GetLastModified(Resource resource)
        {
            return 0;
        }
    }
}
