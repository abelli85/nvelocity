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

namespace NVelocity.Tests.View
{
    using System;

    using Runtime;
    using Runtime.Parser.Node;
    using Runtime.Visitor;

    /// <summary> Simple class for dumping the AST for a template.
    /// Good for debugging and writing new directives.
    /// </summary>
    public class TemplateNodeView
    {
        /// <summary> Root of the AST node structure that results from
        /// parsing a template.
        /// </summary>
        private SimpleNode document;

        /// <summary> Visitor used to traverse the AST node structure
        /// and produce a visual representation of the
        /// node structure. Very good for debugging and
        /// writing new directives.
        /// </summary>
        private NodeViewMode visitor;

        /// <summary> Default constructor: sets up the Velocity
        /// Runtime, creates the visitor for traversing
        /// the node structure and then produces the
        /// visual representation by the visitation.
        /// </summary>
        public TemplateNodeView(string template)
        {
            try
            {
                RuntimeSingleton.Init("velocity.properties");

                System.IO.StreamReader isr = new System.IO.StreamReader(new System.IO.FileStream(template, System.IO.FileMode.Open, System.IO.FileAccess.Read), System.Text.Encoding.GetEncoding(RuntimeSingleton.GetString(RuntimeConstants.INPUT_ENCODING)));

                System.IO.StreamReader br = new System.IO.StreamReader(isr.BaseStream, isr.CurrentEncoding);

                document = RuntimeSingleton.Parse(br, template);

                visitor = new NodeViewMode();
                visitor.Context = null;
                visitor.Writer = new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Text.Encoding.Default);
                document.Accept(visitor, (object)null);
            }
            catch (System.Exception e)
            {
                System.Console.Out.WriteLine(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }
        }
    }
}
