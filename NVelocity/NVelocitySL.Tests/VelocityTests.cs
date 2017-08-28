using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using WalkReader.Model;
using Commons.Collections;
using NVelocity.Runtime.Resource.Util;
using NVelocity.Runtime.Resource.Loader;

namespace NVelocitySL.Tests
{
    /// <summary>
    /// These test cases show how to use NVelocity to merge template.
    /// Please note that the test case <see cref="#TestMergeWithoutDefault"/>
    /// is recommended. Other test cases may run properly under NUnit
    /// but may perform abnormally under silverlight xap.
    /// </summary>
    [TestClass]
    public class VelocityTests
    {
        private readonly string WALK_LIST_VM = "./Report/walk_list.vm";

        /// <summary>
        /// This test case shows initialization with specified 
        /// </summary>
        [TestMethod]
        public void TestInit()
        {
            var ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
            ve.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "UTF-8");
            ve.Init();
        }

        /// <summary>
        /// This test case shows initialization with specified properties file.
        /// </summary>
        [TestMethod]
        public void TestMerge()
        {
            VelocityContext ctx = InitContext();

            var ve = new VelocityEngine();
            ve.Init("./Defaults/nvelocity.properties");
            var temp = ve.GetTemplate(WALK_LIST_VM);
            var tw = new StringWriter();
            temp.Merge(ctx, tw);
            Debug.WriteLine(tw.ToString());
        }

        /// <summary>
        /// This usage is recommended because in silverlight, loading
        /// packaged properties will result in SecurityException.
        /// under silverlight xap, please use resource file with uri like:
        /// '/{assemblyName};component/{folder}/nvelocity.properties'. So get
        /// stream in silverlight:
        /// <code>var streamProps = Application.GetResourceStream(new Uri(
        /// "/{assemblyName};component/{folder}/nvelocity.properties",
        /// UriKind.Relative)).Stream;</code>
        /// </summary>
        [TestMethod]
        public void TestMergeWithoutDefault()
        {
            VelocityContext ctx = InitContext();
            
            // load stream for properties
            var streamProps = new FileStream("./Defaults/nvelocity.properties", FileMode.Open);
            var extProps = new ExtendedProperties();
            extProps.Load(streamProps);

            var ve = new VelocityEngine();
            ve.Init(extProps, false);

            // load template stream
            var body = new StreamReader(new FileStream(WALK_LIST_VM, FileMode.Open)).ReadToEnd();
            StringResourceLoader.GetRepository().PutStringResource("testVm", body);

            var temp = ve.GetTemplate("testVm");
            var tw = new StringWriter();
            temp.Merge(ctx, tw);
            Debug.WriteLine(tw.ToString());
        }

        /// <summary>
        /// This test case shows initialization without properties file, which
        /// means loading default properties file packaged with this assembly.
        /// </summary>
        [TestMethod]
        public void TestMergeDefault()
        {
            VelocityContext ctx = InitContext();

            var ve = new VelocityEngine();
            ve.Init();
            var temp = ve.GetTemplate(WALK_LIST_VM);
            var tw = new StringWriter();
            temp.Merge(ctx, tw);
            Debug.WriteLine(tw.ToString());
        }

        private static VelocityContext InitContext()
        {
            var mlist = new List<Meter>();
            var m1 = new Meter();
            m1.id = 201;
            m1.routeId = 202;
            m1.sequence = 2;
            m1.meterId = "20201";
            m1.street = "测试街道";
            mlist.Add(m1);

            var ctx = new VelocityContext();
            ctx.Put("Name", "China");
            ctx.Put("meterCount", 3);
            ctx.Put("meterList", mlist);
            return ctx;
        }
    }
}
