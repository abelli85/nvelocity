using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using NVelocity.App;
using NVelocity.App.Event;
using NVelocity.Context;
using NVelocity.Exception;
using NVelocity.Runtime;
using NVelocity.Runtime.Log;
using NVelocity.Util;

using NVelocity.Tests.Misc;
using NVelocity.Tests.Provider;

namespace NVelocity.Tests
{
    /// <summary> Tests if the VM template-locality is working.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <author>  <a href="mailto:dlr@collab.net">Daniel Rall</a>
    /// </author>
    /// <version>  $Id: InlineScopeVMTestCase.java 704299 2008-10-14 03:13:16Z nbubna $
    /// </version>
    [TestFixture]
    public class InlineScopeVMTestCase : BaseTestCase
    {
       [SetUp]
        public void setUp()
        {
            /*
            *  do our properties locally, and just override the ones we want
            *  changed
            */

            Velocity.SetProperty(RuntimeConstants.VM_PERM_ALLOW_INLINE_REPLACE_GLOBAL, "true");

            Velocity.SetProperty(RuntimeConstants.VM_PERM_INLINE_LOCAL, "true");

            Velocity.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateTestBase_Fields.FILE_RESOURCE_LOADER_PATH);

            Velocity.SetProperty(RuntimeConstants.RUNTIME_LOG_LOGSYSTEM_CLASS, typeof(TestLogChute).FullName);

            Velocity.Init();
        }

        /// <summary> Runs the test.</summary>
        [Test]
        public virtual void testInlineScopeVM()
        {
            assureResultsDirectoryExists(TemplateTestBase_Fields.RESULT_DIR);

            /*
            * Get the template and the output. Do them backwards.
            * vm_test2 uses a local VM and vm_test1 doesn't
            */

            Template template2 = RuntimeSingleton.GetTemplate(getFileName(null, "vm_test2", TemplateTestBase_Fields.TMPL_FILE_EXT));

            Template template1 = RuntimeSingleton.GetTemplate(getFileName(null, "vm_test1", TemplateTestBase_Fields.TMPL_FILE_EXT));

            //UPGRADE_TODO: 构造函数“java.io.FileOutputStream.FileOutputStream”被转换为具有不同行为的 'System.IO.FileStream.FileStream'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
            System.IO.FileStream fos1 = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "vm_test1", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            //UPGRADE_TODO: 构造函数“java.io.FileOutputStream.FileOutputStream”被转换为具有不同行为的 'System.IO.FileStream.FileStream'。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
            System.IO.FileStream fos2 = new System.IO.FileStream(getFileName(TemplateTestBase_Fields.RESULT_DIR, "vm_test2", TemplateTestBase_Fields.RESULT_FILE_EXT), System.IO.FileMode.Create);

            //UPGRADE_ISSUE: “java.io.Writer”和“System.IO.StreamWriter”之间的类层次结构差异可能导致编译错误。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_WARNING: 在目标代码中，至少有一个表达式被使用了多次。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamWriter writer1 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos1, System.Text.Encoding.Default).Encoding);
            //UPGRADE_ISSUE: “java.io.Writer”和“System.IO.StreamWriter”之间的类层次结构差异可能导致编译错误。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
            //UPGRADE_WARNING: 在目标代码中，至少有一个表达式被使用了多次。 "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            System.IO.StreamWriter writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(fos2, System.Text.Encoding.Default).Encoding);

            /*
            *  Put the Vector into the context, and merge both
            */

            VelocityContext context = new VelocityContext();

            template1.Merge(context, writer1);
            writer1.Flush();
            writer1.Close();

            template2.Merge(context, writer2);
            writer2.Flush();
            writer2.Close();

            if (!isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "vm_test1", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default) || !isMatch(TemplateTestBase_Fields.RESULT_DIR, TemplateTestBase_Fields.COMPARE_DIR, "vm_test2", TemplateTestBase_Fields.RESULT_FILE_EXT, TemplateTestBase_Fields.CMP_FILE_EXT,System.Text.Encoding.Default))
            {
                Assert.Fail("Output incorrect.");
            }
        }
    }
}
