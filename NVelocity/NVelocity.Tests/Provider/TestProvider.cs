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

namespace NVelocity.Tests.Provider
{
    /// <summary> This class is used by the testbed. Instances of the class
    /// are fed into the context that is set before the AST
    /// is traversed and dynamic content generated.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:jvanzyl@apache.org">Jason van Zyl</a>
    /// </author>
    /// <version>  $Id: TestProvider.java 463298 2006-10-12 16:10:32Z henning $
    /// </version>
    public class TestProvider
    {
        public virtual string Name
        {
            get
            {
                return "jason";
            }

        }
        virtual public System.Collections.ArrayList Stack
        {
            get
            {
                System.Collections.ArrayList stack = new System.Collections.ArrayList();
                stack.Add("stack element 1");
                stack.Add("stack element 2");
                stack.Add("stack element 3");
                return stack;
            }

        }
        virtual public System.Collections.IList EmptyList
        {
            get
            {
                System.Collections.IList list = new System.Collections.ArrayList();
                return list;
            }

        }
        virtual public System.Collections.IList List
        {
            get
            {
                System.Collections.IList list = new System.Collections.ArrayList();
                list.Add("list element 1");
                list.Add("list element 2");
                list.Add("list element 3");

                return list;
            }

        }
        virtual public System.Collections.Hashtable Search
        {
            get
            {
                System.Collections.Hashtable h = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                h["Text"] = "this is some text";
                h["EscText"] = "this is escaped text";
                h["Title"] = "this is the title";
                h["Index"] = "this is the index";
                h["URL"] = "http://periapt.com";

                System.Collections.ArrayList al = new System.Collections.ArrayList();
                al.Add(h);

                h["RelatedLinks"] = al;

                return h;
            }

        }
        virtual public System.Collections.Hashtable Hashtable
        {
            get
            {
                System.Collections.Hashtable h = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                h["key0"] = "value0";
                h["key1"] = "value1";
                h["key2"] = "value2";

                return h;
            }

        }
        virtual public System.Collections.ArrayList RelSearches
        {
            get
            {
                System.Collections.ArrayList al = new System.Collections.ArrayList();
                al.Add(Search);

                return al;
            }

        }
        virtual public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
            }

        }
        virtual public object[] Menu
        {
            get
            {
                //ArrayList al = new ArrayList();
                object[] menu = new object[3];
                for (int i = 0; i < 3; i++)
                {
                    System.Collections.Hashtable item = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
                    item["id"] = "item" + System.Convert.ToString(i + 1);
                    item["name"] = "name" + System.Convert.ToString(i + 1);
                    item["label"] = "label" + System.Convert.ToString(i + 1);
                    //al.Add(item);
                    menu[i] = item;
                }

                //return al;
                return menu;
            }

        }
        virtual public System.Collections.ArrayList Customers
        {
            get
            {
                System.Collections.ArrayList list = new System.Collections.ArrayList();

                list.Add("ArrayList element 1");
                list.Add("ArrayList element 2");
                list.Add("ArrayList element 3");
                list.Add("ArrayList element 4");

                return list;
            }

        }
        virtual public System.Collections.ArrayList Customers2
        {
            get
            {
                System.Collections.ArrayList list = new System.Collections.ArrayList();

                list.Add(new TestProvider());
                list.Add(new TestProvider());
                list.Add(new TestProvider());
                list.Add(new TestProvider());

                return list;
            }

        }
        virtual public System.Collections.ArrayList Vector
        {
            get
            {
                System.Collections.ArrayList list = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                list.Add("vector element 1");
                list.Add("vector element 2");

                return list;
            }

        }
        virtual public string[] Array
        {
            get
            {
                string[] strings = new string[2];
                strings[0] = "first element";
                strings[1] = "second element";
                return strings;
            }

        }
        virtual public bool StateTrue
        {
            get
            {
                return true;
            }

        }
        virtual public bool StateFalse
        {
            get
            {
                return false;
            }

        }
        virtual public Person Person
        {
            // These two are for testing subclasses.


            get
            {
                return new Person();
            }

        }
        virtual public Child Child
        {
            get
            {
                return new Child();
            }

        }
        virtual public System.Boolean State
        {
            /*
            * This can't have the signature
            *
            *    public void setState(boolean state)
            *
            *    or dynamically invoking the method
            *    doesn't work ... you would have to
            *    Put a wrapper around a method for a
            *    real boolean property that takes a
            *    Boolean object if you wanted this to
            *    work. Not really sure how useful it
            *    is anyway. Who cares about boolean
            *    values you can just set a variable.
            *
            */


            set
            {
            }

        }
        virtual public System.Int32 BangStart
        {
            set
            {
                System.Console.Out.WriteLine("SetBangStart() : called with val = " + value);
                stateint = value;
            }

        }
        virtual public string Foo
        {
            get
            {
                throw new System.Exception("From getFoo()");
            }

        }
        virtual public string Throw
        {
            get
            {
                throw new System.Exception("From getThrow()");
            }

        }
        internal string title = "lunatic";
        internal bool state;
        internal object ob = null;

        public static string PUB_STAT_STRING = "Public Static String";

        internal int stateint = 0;

        public virtual object Me()
        {
            return this;
        }

        public override string ToString()
        {
            return ("test provider");
        }

        public virtual bool TheAPLRules()
        {
            return true;
        }

        public virtual string ObjectArrayMethod(object[] o)
        {
            return "result of objectArrayMethod";
        }

        public virtual string Concat(object[] strings)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            for (int i = 0; i < strings.Length; i++)
            {
                result.Append((string)strings[i]).Append(' ');
            }

            return result.ToString();
        }

        public virtual string Concat(System.Collections.IList strings)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            for (int i = 0; i < strings.Count; i++)
            {
                result.Append((string)strings[i]).Append(' ');
            }

            return result.ToString();
        }

        public virtual string ObjConcat(System.Collections.IList objects)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            for (int i = 0; i < objects.Count; i++)
            {
                result.Append(objects[i]).Append(' ');
            }

            return result.ToString();
        }

        public virtual string parse(string a, object o, string c, string d)
        {
            return a + o.ToString() + c + d;
        }

        public virtual string Concat(string a, string b)
        {
            return a + b;
        }

        public virtual string ShowPerson(Person person)
        {
            return person.Name;
        }

        /// <summary> Chop i characters off the end of a string.
        /// 
        /// </summary>
        /// <param name="string">String to Chop.
        /// </param>
        /// <param name="i">Number of characters to Chop.
        /// </param>
        /// <returns> String with processed answer.
        /// </returns>
        public virtual string Chop(string string_Renamed, int i)
        {
            return (string_Renamed.Substring(0, (string_Renamed.Length - i) - (0)));
        }

        public virtual bool AllEmpty(object[] list)
        {
            int size = list.Length;

            for (int i = 0; i < size; i++)
            {
                if (list[i].ToString().Length > 0)
                    return false;
            }

            return true;
        }
        public virtual System.Int32 Bang()
        {
            System.Console.Out.WriteLine("Bang! : " + stateint);
            System.Int32 ret = (System.Int32)stateint;
            stateint++;
            return ret;
        }

        /// <summary> Test the ability of vel to use a Get(key)
        /// method for any object type, not just one
        /// that implements the Map interface.
        /// </summary>
        public virtual string Get(string key)
        {
            return key;
        }

        /// <summary> Test the ability of vel to use a Put(key)
        /// method for any object type, not just one
        /// that implements the Map interface.
        /// </summary>
        public virtual string Put(string key, object o)
        {
            ob = o;
            return key;
        }
    }
}
