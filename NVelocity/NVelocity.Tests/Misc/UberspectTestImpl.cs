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
    using NVelocity.Util.Introspection;

    /// <summary> A introspector that allows testing when methods are not found.</summary>
    public class UberspectTestImpl : UberspectImpl
    {

        public override IVelMethod GetMethod(object obj, string methodName, object[] args, Info i)
        {
            IVelMethod method = base.GetMethod(obj, methodName, args, i);

            if (method == null)
            {
                if (obj == null)
                    throw new UberspectTestException("Can't call method '" + methodName + "' on null object", i);
                else
                {
                    throw new UberspectTestException("Did not find method " + obj.GetType().FullName + "." + methodName, i);
                }
            }

            return method;
        }

        public override IVelPropertyGet GetPropertyGet(object obj, string identifier, Info i)
        {
            IVelPropertyGet propertyGet = base.GetPropertyGet(obj, identifier, i);

            if (propertyGet == null)
            {
                if (obj == null)
                    throw new UberspectTestException("Can't call getter '" + identifier + "' on null object", i);
                else
                {
                    throw new UberspectTestException("Did not find " + obj.GetType().FullName + "." + identifier, i);
                }
            }

            return propertyGet;
        }
    }
}
