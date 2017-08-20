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

    using NVelocity.Util.Introspection;
    using Runtime;

    using Misc;

    [TestFixture]
    public class UberspectorTestCase : BaseTestCase
    {
        private RuntimeInstance ri;

        [SetUp]
        public void setUp()
        {
            ri = new RuntimeInstance();
            ri.Init();
        }

        [Test]
        public virtual void testNullObjects()
        {
            // How about some null objects... Gee, I'm mean. ;-)
            IUberspect u = ri.Uberspect;

            IVelPropertyGet getter = u.GetPropertyGet((object)null, "foo", null);
            Assert.IsNull(getter);

            IVelPropertySet setter = u.GetPropertySet((object)null, "foo", typeof(object), null);
            Assert.IsNull(setter);
        }

        [Test]
        public virtual void testEmptyPropertyGetter()
        {
            IUberspect u = ri.Uberspect;

            System.Collections.IDictionary map = new System.Collections.Hashtable();

            IVelPropertyGet getter = u.GetPropertyGet(map, "", null);

            // Don't screw up on empty properties. That should map to Get("")
            Assert.IsNotNull(getter);
            Assert.AreEqual("Get", getter.MethodName, "Found wrong method");
        }

        [Test]
        public virtual void testEmptyPropertySetter()
        {
            IUberspect u = ri.Uberspect;

            System.Collections.IDictionary map = new System.Collections.Hashtable();

            IVelPropertySet setter = u.GetPropertySet(map, "", typeof(object), null);

            // Don't screw up on empty properties. That should map to Put("", Object)
            Assert.IsNotNull(setter);
            Assert.AreEqual("Put", setter.MethodName, "Found wrong method");
        }

        [Test]
        public virtual void testNullPropertyGetter()
        {
            IUberspect u = ri.Uberspect;
            GetPutObject gpo = new GetPutObject();

            System.Collections.IDictionary map = new System.Collections.Hashtable();

            IVelPropertyGet getter = u.GetPropertyGet(gpo, null, null);

            // Don't screw up on null properties. That should map to Get() on the GPO.
            Assert.IsNotNull(getter);
            Assert.AreEqual("Get", getter.MethodName, "Found wrong method");

            // And should be null on a Map which does not have a Get()
            getter = u.GetPropertyGet(map, null, null);
            Assert.IsNull(getter);
        }

        [Test]
        public virtual void testNullPropertySetter()
        {
            IUberspect u = ri.Uberspect;
            GetPutObject gpo = new GetPutObject();

            System.Collections.IDictionary map = new System.Collections.Hashtable();

            // Don't screw up on null properties. That should map to Put() on the GPO.
            IVelPropertySet setter = u.GetPropertySet(gpo, null, "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("Put", setter.MethodName, "Found wrong method");

            // And should be null on a Map which does not have a Put()
            setter = u.GetPropertySet(map, null, "", null);
            Assert.IsNull(setter);
        }

        [Test]
        public virtual void testNullParameterType()
        {
            IVelPropertySet setter;

            IUberspect u = ri.Uberspect;
            UberspectorTestObject uto = new UberspectorTestObject();

            // setRegular()
            setter = u.GetPropertySet(uto, "Regular", (object)null, null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("set_Regular", setter.MethodName, "Found wrong method");

            // SetAmbigous() - String and StringBuffer available
            setter = u.GetPropertySet(uto, "Ambigous", (object)null, null);
            Assert.IsNull(setter);

            // SetAmbigous() - same with Object?
            setter = u.GetPropertySet(uto, "Ambigous", new object(), null);
            Assert.IsNull(setter);
        }

        [Test]
        public virtual void testMultipleParameterTypes()
        {
            IVelPropertySet setter;

            IUberspect u = ri.Uberspect;
            UberspectorTestObject uto = new UberspectorTestObject();

            // SetAmbigous() - String
            setter = u.GetPropertySet(uto, "Ambigous", "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setAmbigous", setter.MethodName, "Found wrong method");

            // SetAmbigous() - StringBuffer
            setter = u.GetPropertySet(uto, "Ambigous", new System.Text.StringBuilder(), null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setAmbigous", setter.MethodName, "Found wrong method");
        }

        [Test]
        public virtual void testRegularGetters()
        {
            IVelPropertyGet getter;

            IUberspect u = ri.Uberspect;
            UberspectorTestObject uto = new UberspectorTestObject();

            // getRegular()
            getter = u.GetPropertyGet(uto, "Regular", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("getRegular", getter.MethodName, "Found wrong method");

            // Lowercase regular
            getter = u.GetPropertyGet(uto, "regular", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("getRegular", getter.MethodName, "Found wrong method");

            // lowercase: getpremium()
            getter = u.GetPropertyGet(uto, "premium", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("getpremium", getter.MethodName, "Found wrong method");

            // test uppercase: getpremium()
            getter = u.GetPropertyGet(uto, "Premium", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("getpremium", getter.MethodName, "Found wrong method");
        }

        [Test]
        public virtual void testBooleanGetters()
        {
            IVelPropertyGet getter;

            IUberspect u = ri.Uberspect;
            UberspectorTestObject uto = new UberspectorTestObject();

            // getRegular()
            getter = u.GetPropertyGet(uto, "RegularBool", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("isRegularBool", getter.MethodName, "Found wrong method");

            // Lowercase regular
            getter = u.GetPropertyGet(uto, "regularBool", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("isRegularBool", getter.MethodName, "Found wrong method");

            // lowercase: getpremiumBool()
            getter = u.GetPropertyGet(uto, "premiumBool", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("ispremiumBool", getter.MethodName, "Found wrong method");

            // test uppercase: ()
            getter = u.GetPropertyGet(uto, "PremiumBool", null);
            Assert.IsNotNull(getter);
            Assert.AreEqual("ispremiumBool", getter.MethodName, "Found wrong method");
        }

        [Test]
        public virtual void testRegularSetters()
        {
            IVelPropertySet setter;

            IUberspect u = ri.Uberspect;
            UberspectorTestObject uto = new UberspectorTestObject();

            // setRegular()
            setter = u.GetPropertySet(uto, "Regular", "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setRegular", setter.MethodName, "Found wrong method");

            // Lowercase regular
            setter = u.GetPropertySet(uto, "regular", "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setRegular", setter.MethodName, "Found wrong method");

            // lowercase: setpremium()
            setter = u.GetPropertySet(uto, "premium", "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setpremium", setter.MethodName, "Found wrong method");

            // test uppercase: getpremium()
            setter = u.GetPropertySet(uto, "Premium", "", null);
            Assert.IsNotNull(setter);
            Assert.AreEqual("setpremium", setter.MethodName, "Found wrong method");
        }


        /*
        *
        *    public void testMapGetSet()
        *        throws Exception
        *    {
        *        IUberspect u = ri.getUberspect();
        *        Map map = new HashMap();
        *
        *        IVelPropertyGet getter = u.GetPropertyGet(map, "", null);
        *        IVelPropertySet setter = u.GetPropertySet(map, "", Object.class, null);
        *
        *        Assert.IsNotNull("Got a null getter", getter);
        *        Assert.IsNotNull("Got a null setter", setter);
        *
        *        Assert.AreEqual("Got wrong getter", "foo", getter.getMethodName());
        *        Assert.AreEqual("Got wrong setter", "bar", setter.getMethodName());
        *    }
        */
    }
}
