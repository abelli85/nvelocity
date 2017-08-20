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
    using App.Event;
    using Context;
    using NVelocity.Util;
    using NVelocity.Util.Introspection;
    using Runtime;


    /// <summary> Tests event handling for all event handlers except IIncludeEventHandler.  This is tested
    /// separately due to its complexity.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:geirm@optonline.net">Geir Magnusson Jr.</a>
    /// </author>
    /// <version>  $Id: InvalidEventHandlerTestCase.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    [TestFixture]
    public class InvalidEventHandlerTestCase 
    {
        [Test]
        public virtual void testManualEventHandlers()
        {
            TestEventCartridge te = new TestEventCartridge();

            /**
            * Test attaching the event cartridge to the context
            */
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            /*
            *  lets make a Context and Add the event cartridge
            */

            VelocityContext inner = new VelocityContext();

            /*
            *  Now make an event cartridge, register all the
            *  event handlers (at once) and attach it to the
            *  Context
            */

            EventCartridge ec = new EventCartridge();
            ec.AddEventHandler(te);
            ec.AttachToContext(inner);

            doTestInvalidReferenceEventHandler1(ve, inner);
            doTestInvalidReferenceEventHandler2(ve, inner);
            doTestInvalidReferenceEventHandler3(ve, inner);
            doTestInvalidReferenceEventHandler4(ve, inner);
        }

        /// <summary> Test assigning the event handlers via properties</summary>

        [Test]
        public virtual void testConfigurationEventHandlers()
        {
            VelocityEngine ve = new VelocityEngine();
            ve.SetProperty(RuntimeConstants.EVENTHANDLER_INVALIDREFERENCES, typeof(TestEventCartridge).FullName);

            ve.Init();
            doTestInvalidReferenceEventHandler1(ve, null);
            doTestInvalidReferenceEventHandler2(ve, null);
            doTestInvalidReferenceEventHandler3(ve, null);
            doTestInvalidReferenceEventHandler4(ve, null);
        }

        /// <summary> Test deeper structures</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="vc">
        /// </param>
        /// <throws>  Exception </throws>
        private void doTestInvalidReferenceEventHandler4(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);

            Tree test = new Tree();
            test.Field = "10";
            Tree test2 = new Tree();
            test2.Field = "12";
            test.Child = test2;

            context.Put("tree", test);
            string s;
            System.IO.TextWriter w;

            // show work fine
            s = "$tree.Field $tree.field $tree.child.Field";
            w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);

            s = "$tree.x $tree.field.x $tree.child.y $tree.child.Field.y";
            w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);
        }

        /// <summary> Test invalid #set</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="vc">
        /// </param>
        /// <throws>  Exception </throws>
        private void doTestInvalidReferenceEventHandler3(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);
            context.Put("a1", (object)5);
            context.Put("a4", (object)5);
            context.Put("b1", "abc");

            string s;
         
            System.IO.TextWriter w;

            // good object, bad right hand side
            s = "#set($xx = $a1.afternoon())";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // good object, bad right hand reference
            s = "#set($yy = $q1)";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }
        }

        /// <summary> Test invalid method calls</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="vc">
        /// </param>
        /// <throws>  Exception </throws>
        private void doTestInvalidReferenceEventHandler2(VelocityEngine ve, VelocityContext vc)
        {
            VelocityContext context = new VelocityContext(vc);
            context.Put("a1", (object)5);
            context.Put("a4", (object)5);
            context.Put("b1", "abc");

            string s;
          
            System.IO.TextWriter w;

            // good object, bad method
            s = "$a1.afternoon()";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // bad object, bad method -- fails on Get
            s = "$zz.daylight()";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // change result
            s = "$b1.baby()";
            w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);
           
            Assert.AreEqual("www", w.ToString());
        }

        /// <summary> Test invalid gets/references</summary>
        /// <param name="ve">
        /// </param>
        /// <param name="vc">
        /// </param>
        /// <throws>  Exception </throws>
        private void doTestInvalidReferenceEventHandler1(VelocityEngine ve, VelocityContext vc)
        {
            string result;

            VelocityContext context = new VelocityContext(vc);
            context.Put("a1", (object)5);
            context.Put("a4", (object)5);
            context.Put("b1", "abc");

            // normal - should be no calls to handler
            string s = "$a1 $a1.intValue() $b1 $b1.length() #set($c1 = '5')";
           
            System.IO.TextWriter w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);

            // good object, bad property
            s = "$a1.foobar";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // bad object, bad property            
            s = "$a2.foobar";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // bad object, no property            
            s = "$a3";
            w = new System.IO.StringWriter();
            try
            {
                ve.Evaluate(context, w, "mystring", s);
                Assert.Fail("Expected exception.");
            }
            catch (System.SystemException e)
            {
            }

            // good object, bad property; change the value
            s = "$a4.foobar";
            w = new System.IO.StringWriter();
            ve.Evaluate(context, w, "mystring", s);
            
            result = w.ToString();
            Assert.AreEqual("zzz", result);
        }



        /// <summary> Test assigning the event handlers via properties</summary>

        public class TestEventCartridge : IInvalidReferenceEventHandler, IRuntimeServicesAware
        {
            private IRuntimeServices rs;

            public TestEventCartridge()
            {
            }

            /// <summary> Required by EventHandler</summary>
            public virtual void SetRuntimeServices(IRuntimeServices rs)
            {
                // make sure this is only called once
                if (this.rs == null)
                    this.rs = rs;
                else
                    Assert.Fail("initialize called more than once.");
            }


            public virtual object InvalidGetMethod(IContext context, string reference, object object_Renamed, string property, Info info)
            {
                // as a test, make sure this EventHandler is initialized
                if (rs == null)
                    Assert.Fail("Event handler not initialized!");

                // good object, bad property
                if (reference.Equals("$a1.foobar"))
                {
                    Assert.AreEqual((object)5, object_Renamed);
                    Assert.AreEqual("foobar", property);
                    throw new System.SystemException("expected exception");
                }
                // bad object, bad property            
                else if (reference.Equals("$a2"))
                {
                    Assert.IsNull(object_Renamed);
                    Assert.IsNull(property);
                    throw new System.SystemException("expected exception");
                }
                // bad object, no property            
                else if (reference.Equals("$a3"))
                {
                    Assert.IsNull(object_Renamed);
                    Assert.IsNull(property);
                    throw new System.SystemException("expected exception");
                }
                // good object, bad property; change the value
                else if (reference.Equals("$a4.foobar"))
                {
                    Assert.AreEqual((object)5, object_Renamed);
                    Assert.AreEqual("foobar", property);
                    return "zzz";
                }
                // bad object, bad method -- fail on the object
                else if (reference.Equals("$zz"))
                {
                    Assert.IsNull(object_Renamed);
                    Assert.IsNull(property);
                    throw new System.SystemException("expected exception");
                }
                // pass q1 through
                else if (reference.Equals("$q1"))
                {

                }
                else if (reference.Equals("$tree.x"))
                {
                    Assert.AreEqual("x", property);
                }
                else if (reference.Equals("$tree.field.x"))
                {
                    Assert.AreEqual("x", property);
                }
                else if (reference.Equals("$tree.child.y"))
                {
                    Assert.AreEqual("y", property);
                }
                else if (reference.Equals("$tree.child.Field.y"))
                {
                    Assert.AreEqual("y", property);
                }
                else
                {
                    Assert.Fail("InvalidGetMethod: unexpected reference: " + reference);
                }
                return null;
            }

            public virtual object InvalidMethod(IContext context, string reference, object object_Renamed, string method, Info info)
            {
                // as a test, make sure this EventHandler is initialized
                if (rs == null)
                    Assert.Fail("Event handler not initialized!");

                // good reference, bad method
                if (object_Renamed.GetType().Equals(typeof(System.Int32)))
                {
                    Assert.AreEqual("$a1.afternoon()", reference);
                    Assert.AreEqual("afternoon", method);
                    throw new System.SystemException("expected exception");
                }
                else if (object_Renamed.GetType().Equals(typeof(string)) && "baby".Equals(method))
                {
                    return "www";
                }
                else
                {
                    Assert.Fail("Unexpected invalid method.  " + method);
                }

                return null;
            }


            public virtual bool InvalidSetMethod(IContext context, string leftreference, string rightreference, Info info)
            {

                // as a test, make sure this EventHandler is initialized
                if (rs == null)
                    Assert.Fail("Event handler not initialized!");

                // good object, bad method
                if (leftreference.Equals("xx"))
                {
                    Assert.AreEqual("q1.afternoon()", rightreference);
                    throw new System.SystemException("expected exception");
                }
                if (leftreference.Equals("yy"))
                {
                    Assert.AreEqual("$q1", rightreference);
                    throw new System.SystemException("expected exception");
                }
                else
                {
                    Assert.Fail("Unexpected left hand side.  " + leftreference);
                }

                return false;
            }
        }

        public class Tree
        {
            virtual public string Field
            {
                get
                {
                    return field;
                }

                set
                {
                    this.field = value;
                }

            }
            virtual public Tree Child
            {
                get
                {
                    return child;
                }

                set
                {
                    this.child = value;
                }

            }
            internal string field;
            internal Tree child;

            public Tree()
            {
            }

            public virtual string testMethod()
            {
                return "123";
            }
        }
    }
}
