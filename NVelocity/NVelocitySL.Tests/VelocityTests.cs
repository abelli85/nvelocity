using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using WalkReader.Model;

namespace NVelocitySL.Tests
{
    [TestClass]
    public class VelocityTests
    {
        private readonly string WALK_LIST_VM = "./Report/walk_list.vm";

        [TestMethod]
        public void TestMethod1()
        {
            var ve = new VelocityEngine();
            ve.AddProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
            ve.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "UTF-8");
            ve.Init();

            var mlist = new List<Dictionary<object, object>>();
            var ctx = new VelocityContext();
            ctx.Put("meterList", mlist);
            ctx.Put("meterCount", 3);

            //var temp = ve.GetTemplate()
        }

        public void TestVelocity()
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

            var ve = new VelocityEngine();
            ve.Init();
            var temp = ve.GetTemplate(WALK_LIST_VM);
            var tw = new StringWriter();
            temp.Merge(ctx, tw);
            Debug.WriteLine(tw.ToString());
        }
    }
}
