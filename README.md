# nvelocity
The template engine NVelocitySL supports silverlight based on NVelocity from http://nvelocity.codeplex.com/.


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
