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

    using Misc;

    /// <summary> Test case for including macro libraries via the #parse method.</summary>
    [TestFixture]
    public class ParseWithMacroLibsTestCase : BaseTestCase
    {

        private static readonly string RESULT_DIR = TemplateTestBase_Fields.TEST_RESULT_DIR + "/parsemacros";


        private static readonly string COMPARE_DIR = TemplateTestBase_Fields.TEST_COMPARE_DIR + "/parsemacros/Compare";

        [Test]
        public virtual void testParseMacroLocalCacheOn()
        {
            /*
            *  local scope, cache on
            */
            VelocityEngine ve = createEngine(true, true);

            // render twice to make sure there is no difference with cached templates
            testParseMacro(ve, "vm_library1.vm", "parseMacro1_1", false);
            testParseMacro(ve, "vm_library1.vm", "parseMacro1_1", false);

            // run again with different macro library
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_1b", false);
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_1b", false);
        }

        /// <summary> Runs the tests with global namespace.</summary>
        [Test]
        public virtual void testParseMacroLocalCacheOff()
        {
            /*
            *  local scope, cache off
            */
            VelocityEngine ve = createEngine(false, true);

            testParseMacro(ve, "vm_library1.vm", "parseMacro1_2", true);

            // run again with different macro library
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_2b", true);
        }

        [Test]
        public virtual void testParseMacroGlobalCacheOn()
        {
            /*
            *  global scope, cache on
            */
            VelocityEngine ve = createEngine(true, false);

            // render twice to make sure there is no difference with cached templates
            testParseMacro(ve, "vm_library1.vm", "parseMacro1_3", false);
            testParseMacro(ve, "vm_library1.vm", "parseMacro1_3", false);

            // run again with different macro library
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_3b", false);
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_3b", false);
        }

        [Test]
        public virtual void testParseMacroGlobalCacheOff()
        {
            /*
            *  global scope, cache off
            */
            VelocityEngine ve = createEngine(false, false);

            testParseMacro(ve, "vm_library1.vm", "parseMacro1_4", true);

            // run again with different macro library
            testParseMacro(ve, "vm_library2.vm", "parseMacro1_4b", true);
        }

        /// <summary> Test #parse with macros.  Can be used to test different engine configurations</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="outputBaseFileName">
        /// </param>
        /// <param name="testCachingOff">
        /// </param>
        /// <throws>  Exception </throws>
        private void testParseMacro(VelocityEngine ve, string includeFile, string outputBaseFileName, bool testCachingOff)
        {
            assureResultsDirectoryExists(RESULT_DIR);

       
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULT_DIR, outputBaseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            VelocityContext context = new VelocityContext();
            context.Put("includefile", includeFile);

        
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            Template template = ve.GetTemplate("parseMacro1.vm");
            template.Merge(context, writer);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, outputBaseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                string result = getFileContents(RESULT_DIR, outputBaseFileName, TemplateTestBase_Fields.RESULT_FILE_EXT, System.Text.Encoding.Default);
                string compare = getFileContents(COMPARE_DIR, outputBaseFileName, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default);

                string msg = "Processed template did not match expected output\n" + "-----Result-----\n" + result + "----Expected----\n" + compare + "----------------";

                Assert.Fail(msg);
            }

            /*
            * Show that caching is turned off
            */
            if (testCachingOff)
            {
                Template t1 = ve.GetTemplate("parseMacro1.vm");
                Template t2 = ve.GetTemplate("parseMacro1.vm");

                Assert.AreNotSame(t1, t2, "Different objects");
            }
        }

        /// <summary> Return and Initialize engine</summary>
        /// <returns>
        /// </returns>
        private VelocityEngine createEngine(bool cache, bool local)
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, (object)true);
            ve.SetProperty("velocimacro.permissions.allow.inline.to.replace.global", (object)local);
            ve.SetProperty("file.resource.loader.cache", (object)cache);
            ve.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);
            ve.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            ve.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.TEST_COMPARE_DIR + "/parsemacros");
            ve.Init();

            return ve;
        }


        /// <summary> Test whether the literal text is given if a definition cannot be
        /// found for a macro.
        /// 
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testParseMacrosWithNoDefinition()
        {
            /*
            *  ve1: local scope, cache on
            */
            VelocityEngine ve1 = new VelocityEngine();

            ve1.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, (object)true);
            ve1.SetProperty("velocimacro.permissions.allow.inline.to.replace.global", (object)false);
            ve1.SetProperty("file.resource.loader.cache", (object)true);
            ve1.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);
            ve1.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            ve1.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.TEST_COMPARE_DIR + "/parsemacros");
            ve1.Init();

            assureResultsDirectoryExists(RESULT_DIR);

           
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULT_DIR, "parseMacro2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            VelocityContext context = new VelocityContext();

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            Template template = ve1.GetTemplate("parseMacro2.vm");
            template.Merge(context, writer);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, "parseMacro2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }


        /// <summary> Test that if a macro is duplicated, the second one takes precendence
        /// 
        /// </summary>
        /// <throws>  Exception </throws>
        [Test]
        public virtual void testDuplicateDefinitions()
        {
            /*
            *  ve1: local scope, cache on
            */
            VelocityEngine ve1 = new VelocityEngine();

            ve1.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, (object)true);
            ve1.SetProperty("velocimacro.permissions.allow.inline.to.replace.global", (object)false);
            ve1.SetProperty("file.resource.loader.cache", (object)true);
            ve1.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);
            ve1.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            ve1.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.TEST_COMPARE_DIR + "/parsemacros");
            ve1.Init();

            assureResultsDirectoryExists(RESULT_DIR);

         
            System.IO.FileStream fos = new System.IO.FileStream(getFileName(RESULT_DIR, "parseMacro3", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            VelocityContext context = new VelocityContext();

          
            System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.StreamWriter(fos, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos, System.Text.Encoding.Default).Encoding);

            Template template = ve1.GetTemplate("parseMacro3.vm");
            template.Merge(context, writer);

            /**
            * Write to the file
            */
            writer.Flush();
            writer.Close();

            if (!isMatch(RESULT_DIR, COMPARE_DIR, "parseMacro3", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT, System.Text.Encoding.Default))
            {
                Assert.Fail("Processed template did not match expected output");
            }
        }
    }
}
