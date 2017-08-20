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
    using System;
    using System.IO;

    using NUnit.Framework;

    using App;
    using Runtime;

    using Misc;

    /// <summary> Macro library inclution via the Template.merge method is tested using this
    /// class.
    /// </summary>
    [TestFixture]
    public class VMLibraryTestCase : BaseTestCase
    {
        /// <summary> This engine is used with local namespaces</summary>
        private VelocityEngine ve1 = new VelocityEngine();

        /// <summary> This engine is used with global namespaces</summary>
        private VelocityEngine ve2 = new VelocityEngine();


        private static readonly string RESULT_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/macrolibs";


        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/macrolibs/Compare";

        [SetUp]
        public void setUp()
        {
            /*
            *  setup local scope for templates
            */
            ve1.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, true);
            ve1.SetProperty("velocimacro.permissions.allow.inline.to.replace.global", false);
            /**
            * Turn on the cache
            */
            ve1.SetProperty("file.resource.loader.cache", (object)true);

            ve1.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            ve1.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            ve1.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateTestBase_Fields.TEST_COMPARE_DIR + "/macrolibs"));
            ve1.Init();

            /**
            * Set to global namespaces
            */
            ve2.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, false);
            ve2.SetProperty("velocimacro.permissions.allow.inline.to.replace.global", true);
            /**
            * Turn on the cache
            */
            ve2.SetProperty("file.resource.loader.cache", false);

            ve2.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            ve2.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            ve2.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateTestBase_Fields.TEST_COMPARE_DIR + "/macrolibs"));
            ve2.Init();
        }



        /// <summary> Runs the tests with local namespace.</summary>
        [Test]
        public virtual void testVelociMacroLibWithLocalNamespace()
        {
            assureResultsDirectoryExists(RESULT_DIR);
            /**
            * Clear the file before proceeding
            */
            System.IO.FileInfo file = new System.IO.FileInfo(getFileName(RESULT_DIR, "vm_library_local", TemplateTestBase_Fields.RESULT_FILE_EXT));
            
            if (System.IO.File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }

            /**
            * Create a file output stream for appending
            */

            System.IO.FileStream fos = System.IO.File.Open(getFileName(RESULT_DIR, "vm_library_local", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Append, System.IO.FileAccess.Write);

            System.Collections.IList templateList = new System.Collections.ArrayList();
            VelocityContext context = new VelocityContext();

            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            templateList.Add("vm_library1.vm");

            Template template = ve1.GetTemplate("vm_library_local.vm");
            template.Merge(context, writer, templateList);

            /**
            * remove the first template library and includes a new library
            * with a new definition for macros
            */
            templateList.RemoveAt(0);
            templateList.Add("vm_library2.vm");
            template = ve1.GetTemplate("vm_library_local.vm");
            template.Merge(context, writer, templateList);

            /*
            *Show that caching is working
            */
            Template t1 = ve1.GetTemplate("vm_library_local.vm");
            Template t2 = ve1.GetTemplate("vm_library_local.vm");

            Assert.AreEqual(t1, t2, "Both templates refer to the same object");

            /**
            * Remove the libraries
            */
            template = ve1.GetTemplate("vm_library_local.vm");
            template.Merge(context, writer);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, "vm_library_local", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }

        /// <summary> Runs the tests with global namespace.</summary>
        [Test]
        public virtual void testVelociMacroLibWithGlobalNamespace()
        {
            assureResultsDirectoryExists(RESULT_DIR);
            /**
            * Clear the file before proceeding
            */
            System.IO.FileInfo file = new System.IO.FileInfo(getFileName(RESULT_DIR, "vm_library_global", TemplateTestBase_Fields.RESULT_FILE_EXT));
       
            if (System.IO.File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }

            /**
            * Create a file output stream for appending
            */

            System.IO.FileStream fos = System.IO.File.Open(getFileName(RESULT_DIR, "vm_library_global", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Append, System.IO.FileAccess.Write);

            System.Collections.IList templateList = new System.Collections.ArrayList();
            VelocityContext context = new VelocityContext();

            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            templateList.Add("vm_library1.vm");

            Template template = ve1.GetTemplate("vm_library_global.vm");
            template.Merge(context, writer, templateList);

            /**
            * remove the first template library and includes a new library
            * with a new definition for macros
            */
            templateList.RemoveAt(0);
            templateList.Add("vm_library2.vm");
            template = ve1.GetTemplate("vm_library_global.vm");
            template.Merge(context, writer, templateList);

            /*
            *Show that caching is not working (We have turned off cache)
            */
            Template t1 = ve2.GetTemplate("vm_library_global.vm");
            Template t2 = ve2.GetTemplate("vm_library_global.vm");

            Assert.AreNotSame(t1, t2, "Defferent objects");

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, "vm_library_global", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }

        /// <summary> Runs the tests with global namespace.</summary>
        [Test]
        public virtual void testVelociMacroLibWithDuplicateDefinitions()
        {
            assureResultsDirectoryExists(RESULT_DIR);
            /**
            * Clear the file before proceeding
            */
            System.IO.FileInfo file = new System.IO.FileInfo(getFileName(RESULT_DIR, "vm_library_duplicate", TemplateTestBase_Fields.RESULT_FILE_EXT));

            if (File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }


            /**
            * Create a file output stream for appending
            */

            System.IO.FileStream fos = System.IO.File.Open(getFileName(RESULT_DIR, "vm_library_duplicate", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Append, System.IO.FileAccess.Write);

            System.Collections.IList templateList = new System.Collections.ArrayList();
            VelocityContext context = new VelocityContext();

            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            templateList.Add("vm_library1.vm");
            templateList.Add("vm_library2.vm");

            Template template = ve1.GetTemplate("vm_library.vm");
            template.Merge(context, writer, templateList);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, "vm_library_duplicate", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }

        /// <summary> Test whether the literal text is given if a definition cannot be
        /// found for a macro.
        /// 
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testMacrosWithNoDefinition()
        {
            assureResultsDirectoryExists(RESULT_DIR);


            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULT_DIR, "vm_library", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            VelocityContext context = new VelocityContext();

            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            Template template = ve1.GetTemplate("vm_library.vm");
            template.Merge(context, writer, null);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            /**
            * outputs the macro calls
            */
            if (!isMatch(RESULT_DIR, COMPARE_DIR, "vm_library", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }
    }
}
