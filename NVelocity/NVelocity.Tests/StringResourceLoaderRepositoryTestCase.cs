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
    using Runtime;
    using Runtime.Log;
    using Runtime.Resource.Loader;
    using Runtime.Resource.Util;

    /// <summary> Tests ability to have multiple repositories in the same app.
    /// 
    /// </summary>
    /// <author>  Nathan Bubna
    /// </author>
    /// <version>  $Id: StringResourceLoaderRepositoryTestCase.java 479058 2006-11-25 00:26:32Z henning $
    /// </version>
    [TestFixture]
    public class StringResourceLoaderRepositoryTestCase 
    {
        private VelocityContext context;

        public void setUp()
        {
            Velocity.SetProperty(RuntimeConstants.RESOURCE_LOADER, "string");
            Velocity.AddProperty("string.resource.loader.class", typeof(StringResourceLoader).FullName);
            Velocity.AddProperty("string.resource.loader.modificationCheckInterval", "1");
            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(SystemLogChute).FullName);
            Velocity.Init();

            IStringResourceRepository repo = getRepo(null, null);
            repo.PutStringResource("foo", "This is $foo");
            repo.PutStringResource("bar", "This is $bar");

            context = new VelocityContext();
            context.Put("foo", "wonderful!");
            context.Put("bar", "horrible!");
            context.Put("woogie", "a woogie");
        }


        protected internal virtual VelocityEngine newStringEngine(string repoName, bool isStatic)
        {
            VelocityEngine engine = new VelocityEngine();
            engine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "string");
            engine.AddProperty("string.resource.loader.class", typeof(StringResourceLoader).FullName);
            if (repoName != null)
            {
                engine.AddProperty("string.resource.loader.repository.name", repoName);
            }
            if (!isStatic)
            {
                engine.AddProperty("string.resource.loader.repository.static", "false");
            }
            engine.AddProperty("string.resource.loader.modificationCheckInterval", "1");
            engine.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(SystemLogChute).FullName);
            return engine;
        }

        protected internal virtual IStringResourceRepository getRepo(string name, VelocityEngine engine)
        {
            if (engine == null)
            {
                if (name == null)
                {
                    return StringResourceLoader.GetRepository();
                }
                else
                {
                    return StringResourceLoader.GetRepository(name);
                }
            }
            else
            {
                if (name == null)
                {
                    return (IStringResourceRepository)engine.GetApplicationAttribute(StringResourceLoader.REPOSITORY_NAME_DEFAULT);
                }
                else
                {
                    return (IStringResourceRepository)engine.GetApplicationAttribute(name);
                }
            }
        }

        protected internal virtual string render(Template template)
        {
            System.IO.StringWriter out_Renamed = new System.IO.StringWriter();
            template.Merge(this.context, out_Renamed);
            return out_Renamed.ToString();
        }

        [Test]
        public virtual void testSharedRepo()
        {
            // this engine's string resource loader should share a repository
            // with the singleton's string resource loader
            VelocityEngine engine = newStringEngine(null, true);

            // Get and merge the same template from both runtimes with the same context
            string engineOut = render(engine.GetTemplate("foo"));
            string singletonOut = render(RuntimeSingleton.GetTemplate("foo"));

            // make sure they're equal
            Assert.AreEqual(engineOut, singletonOut);
        }

        [Test]
        public virtual void testAlternateStaticRepo()
        {
            VelocityEngine engine = newStringEngine("alternate.repo", true);
            // should be null be for Init
            IStringResourceRepository repo = getRepo("alternate.repo", null);
            Assert.IsNull(repo);
            engine.Init();
            // and not null after Init
            repo = getRepo("alternate.repo", null);
            Assert.IsNotNull(repo);
            repo.PutStringResource("foo", "This is NOT $foo");

            // Get and merge template with the same name from both runtimes with the same context
            string engineOut = render(engine.GetTemplate("foo"));
            string singletonOut = render(RuntimeSingleton.GetTemplate("foo"));

            // make sure they're NOT equal
            Assert.IsFalse(engineOut.Equals(singletonOut));
        }

        [Test]
        public virtual void testPreCreatedStaticRepo()
        {
            VelocityEngine engine = newStringEngine("my.repo", true);
            MyRepo repo = new MyRepo();
            repo.put("bar", "This is NOT $bar");
            StringResourceLoader.SetRepository("my.repo", repo);

            string out_Renamed = render(engine.GetTemplate("bar"));
            Assert.AreEqual(out_Renamed, "This is NOT horrible!");
        }

        [Test]
        public virtual void testAppRepo()
        {
            VelocityEngine engine = newStringEngine(null, false);
            engine.Init();

            IStringResourceRepository repo = getRepo(null, engine);
            Assert.IsNotNull(repo);
            repo.PutStringResource("woogie", "What is $woogie?");

            string out_Renamed = render(engine.GetTemplate("woogie"));
            Assert.AreEqual(out_Renamed, "What is a woogie?");
        }

        [Test]
        public virtual void testAlternateAppRepo()
        {
            VelocityEngine engine = newStringEngine("alternate.app.repo", false);
            engine.Init();

            IStringResourceRepository repo = getRepo("alternate.app.repo", engine);
            Assert.IsNotNull(repo);
            repo.PutStringResource("you/foo.vm", "You look $foo");

            string out_Renamed = render(engine.GetTemplate("you/foo.vm"));
            Assert.AreEqual(out_Renamed, "You look wonderful!");
        }

        [Test]
        public virtual void testPreCreatedAppRepo()
        {
            VelocityEngine engine = newStringEngine("my.app.repo", false);
            MyRepo repo = new MyRepo();
            repo.put("you/bar.vm", "You look $bar");
            engine.SetApplicationAttribute("my.app.repo", repo);

            string out_Renamed = render(engine.GetTemplate("you/bar.vm"));
            Assert.AreEqual(out_Renamed, "You look horrible!");
        }

        public class MyRepo : StringResourceRepositoryImpl
        {
            public virtual void put(string name, string template)
            {
                PutStringResource(name, template);
            }
        }
    }
}
