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

namespace NVelocity.Tests.Misc
{
    using Context;

    /// <summary> Used for testing EvaluateContext.  For testing purposes, this is a case insensitive
    /// context.  
    /// 
    /// </summary>
    /// <author>  <a href="mailto:wglass@forio.com">Will Glass-Husain</a>
    /// </author>
    /// <version>  $Id: TestContext.java 522413 2007-03-26 04:34:15Z wglass $
    /// </version>
    public class TestContext : IContext
    {
        virtual public object[] Keys
        {
            get
            {
                object[] keys = new object[originalKeys.Values.Count];
                originalKeys.Values.CopyTo(keys, 0);

                return keys;
            }

        }
        internal IContext innerContext = new VelocityContext();
     
        internal System.Collections.IDictionary originalKeys = new System.Collections.Hashtable();

        public virtual bool ContainsKey(object key)
        {
            return innerContext.ContainsKey(NormalizeKey(key));
        }

        public virtual object Get(string key)
        {
            return innerContext.Get(NormalizeKey(key));
        }

        public virtual object Put(string key, object value_Renamed)
        {
            string normalizedKey = NormalizeKey(key);
            originalKeys[key] = normalizedKey;
            return innerContext.Put(normalizedKey, value_Renamed);
        }

        public virtual object Remove(object key)
        {
            originalKeys.Remove(key);
            return innerContext.Remove(NormalizeKey(key));
        }

        private string NormalizeKey(object key)
        {
            if (key == null)
            {
                return null;
            }
            else
            {
                if (key.ToString() == null)
                {
                    return null;
                }
                else
                {
                    return key.ToString().ToUpper();
                }
            }
        }
    }
}
