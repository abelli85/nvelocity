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

    using App;

    /// <summary> Test resource caching related issues.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@apache.org">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: ResourceCachingTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class ResourceCachingTestCase : BaseTestCase
    {
        /// <summary> Path for templates. This property will override the
        /// value in the default velocity properties file.
        /// </summary>
        private const string FILE_RESOURCE_LOADER_PATH = "test/resourcecaching";


        /// <summary> Tests for fix of bug VELOCITY-98 where a #include followed by #parse
        /// of the same file throws ClassCastException when caching is on.
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testIncludeParseCaching()
        {

            VelocityEngine ve = new VelocityEngine();

            ve.SetProperty("file.resource.loader.cache", "true");
            ve.SetProperty("file.resource.loader.path", FILE_RESOURCE_LOADER_PATH);
            ve.Init();

            Template template = ve.GetTemplate("testincludeparse.vm");

           
            System.IO.TextWriter writer = new System.IO.StringWriter();

            VelocityContext context = new VelocityContext();

            // will produce a ClassCastException if Velocity-98 is not solved
            template.Merge(context, writer);
            writer.Flush();
            writer.Close();
        }
    }
}
