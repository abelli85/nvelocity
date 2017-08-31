# nvelocity
The template engine NVelocitySL supports silverlight based on NVelocity from http://nvelocity.codeplex.com/ and velocity from apache.

NVelocitySL is expected to be used in silverlight to fill Excel spreadsheet 2003 XML files. The following mistakes are made mostly:

1. incorrect data format, e.g. DateTime is usually to be mistaken. It is recommended to use 'String' instead of 'DateTime'.
2. incorrect NumberFormat string. '#' should be removed to avoid NVelocity parsing.
3. ExpandedRowCount property in Table element.

Whatever, Excel spreadsheet 2003 XML format is a sensitive specification. It's recommended to compose and fill the template carefully. 


````C#

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


The default nvelocity.properties can be put under your project and loading at runtime.


```INI
	# Licensed to the Apache Software Foundation (ASF) under one
	# or more contributor license agreements.  See the NOTICE file
	# distributed with this work for additional information
	# regarding copyright ownership.  The ASF licenses this file
	# to you under the Apache License, Version 2.0 (the
	# "License"); you may not use this file except in compliance
	# with the License.  You may obtain a copy of the License at
	#
	#   http://www.apache.org/licenses/LICENSE-2.0
	#
	# Unless required by applicable law or agreed to in writing,
	# software distributed under the License is distributed on an
	# "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
	# KIND, either express or implied.  See the License for the
	# specific language governing permissions and limitations
	# under the License.    
	
	# ----------------------------------------------------------------------------
	# R U N T I M E  L O G
	# ----------------------------------------------------------------------------
	
	# ----------------------------------------------------------------------------
	#  default LogChute to use: default: AvalonLogChute, Log4JLogChute, CommonsLogLogChute, ServletLogChute, JdkLogChute
	# ----------------------------------------------------------------------------
	
	#runtime.log.logsystem.class = org.apache.velocity.runtime.log.AvalonLogChute,org.apache.velocity.runtime.log.Log4JLogChute,org.apache.velocity.runtime.log.CommonsLogLogChute,org.apache.velocity.runtime.log.ServletLogChute,org.apache.velocity.runtime.log.JdkLogChute
	
	# ---------------------------------------------------------------------------
	# This is the location of the Velocity Runtime log.
	# ----------------------------------------------------------------------------
	
	runtime.log = velocity.log
	
	# ----------------------------------------------------------------------------
	# This controls whether invalid references are logged.
	# ----------------------------------------------------------------------------
	
	runtime.log.invalid.references = true
	
	# ----------------------------------------------------------------------------
	# T E M P L A T E  E N C O D I N G
	# ----------------------------------------------------------------------------
	
	# input.encoding=ISO-8859-1
	# output.encoding=ISO-8859-1
	input.encoding=UTF-8
	output.encoding=UTF-8
	
	# ----------------------------------------------------------------------------
	# F O R E A C H  P R O P E R T I E S
	# ----------------------------------------------------------------------------
	# These properties control how the counter is accessed in the #foreach
	# directive. By default the reference $velocityCount and $velocityHasNext
	# will be available in the body of the #foreach directive.
	# The default starting value for $velocityCount is 1.
	# ----------------------------------------------------------------------------
	
	directive.foreach.counter.name = velocityCount
	directive.foreach.counter.initial.value = 1
	directive.foreach.maxloops = -1
	
	directive.foreach.iterator.name = velocityHasNext
	
	# ----------------------------------------------------------------------------
	# S E T  P R O P E R T I E S
	# ----------------------------------------------------------------------------
	# These properties control the behavior of #set.
	# For compatibility, the default behavior is to disallow setting a reference
	# to null.  This default may be changed in a future version.
	# ----------------------------------------------------------------------------
	
	directive.set.null.allowed = false
	
	# ----------------------------------------------------------------------------
	# I N C L U D E  P R O P E R T I E S
	# ----------------------------------------------------------------------------
	# These are the properties that governed the way #include'd content
	# is governed.
	# ----------------------------------------------------------------------------
	
	directive.include.output.errormsg.start = <!-- include error :
	directive.include.output.errormsg.end   =  see error log -->
	
	# ----------------------------------------------------------------------------
	# P A R S E  P R O P E R T I E S
	# ----------------------------------------------------------------------------
	
	directive.parse.max.depth = 10
	
	# ----------------------------------------------------------------------------
	# T E M P L A T E  L O A D E R S
	# ----------------------------------------------------------------------------
	#
	#
	# ----------------------------------------------------------------------------
	
	resource.loader = file,string
	
	file.resource.loader.description = Velocity File Resource Loader
	file.resource.loader.class = NVelocity.Runtime.Resource.Loader.FileResourceLoader
	file.resource.loader.path = .
	file.resource.loader.cache = false
	file.resource.loader.modificationCheckInterval = 0
	
	string.resource.loader.description = Velocity String Resource Loader
	string.resource.loader.class = NVelocity.Runtime.Resource.Loader.StringResourceLoader
	string.resource.loader.path = .
	string.resource.loader.cache = false
	string.resource.loader.modificationCheckInterval = 0

	# ----------------------------------------------------------------------------
	# VELOCIMACRO PROPERTIES
	# ----------------------------------------------------------------------------
	# global : name of default global library.  It is expected to be in the regular
	# template path.  You may remove it (either the file or this property) if
	# you wish with no harm.
	# ----------------------------------------------------------------------------
	# velocimacro.library = VM_global_library.vm
	
	velocimacro.permissions.allow.inline = true
	velocimacro.permissions.allow.inline.to.replace.global = false
	velocimacro.permissions.allow.inline.local.scope = false
	
	velocimacro.context.localscope = false
	velocimacro.max.depth = 20
	
	# ----------------------------------------------------------------------------
	# VELOCIMACRO STRICT MODE
	# ----------------------------------------------------------------------------
	# if true, will throw an exception for incorrect number 
	# of arguments.  false by default (for backwards compatibility)
	# but this option will eventually be removed and will always
	# act as if true
	# ----------------------------------------------------------------------------
	velocimacro.arguments.strict = false
	
	
	# ----------------------------------------------------------------------------
	# INTERPOLATION
	# ----------------------------------------------------------------------------
	# turn off and on interpolation of references and directives in string
	# literals.  ON by default :)
	# ----------------------------------------------------------------------------
	runtime.interpolate.string.literals = true
	
	
	# ----------------------------------------------------------------------------
	# RESOURCE MANAGEMENT
	# ----------------------------------------------------------------------------
	# Allows alternative ResourceManager and ResourceCache implementations
	# to be plugged in.
	# ----------------------------------------------------------------------------
	resource.manager.class = NVelocity.Runtime.Resource.ResourceManagerImpl
	resource.manager.cache.class = NVelocity.Runtime.Resource.ResourceCacheImpl
	
	# ----------------------------------------------------------------------------
	# PARSER POOL
	# ----------------------------------------------------------------------------
	# Selects a custom factory class for the parser pool.  Must implement
	# ParserPool.  parser.pool.size is used by the default implementation
	# ParserPoolImpl
	# ----------------------------------------------------------------------------
	
	parser.pool.class = NVelocity.Runtime.ParserPoolImpl
	parser.pool.size = 40
	
	
	# ----------------------------------------------------------------------------
	# EVENT HANDLER
	# ----------------------------------------------------------------------------
	# Allows alternative event handlers to be plugged in.  Note that each
	# class property is actually a comma-separated list of classes (which will
	# be called in order).
	# ----------------------------------------------------------------------------
	# eventhandler.referenceinsertion.class =
	# eventhandler.nullset.class =
	# eventhandler.methodexception.class =
	# eventhandler.include.class =
	
	
	# ----------------------------------------------------------------------------
	# EVALUATE
	# ----------------------------------------------------------------------------
	# Evaluate VTL dynamically in template.  Select a class for the Context
	# ----------------------------------------------------------------------------
	
	directive.evaluate.context.class = NVelocity.VelocityContext
	
	
	# ----------------------------------------------------------------------------
	# PLUGGABLE INTROSPECTOR
	# ----------------------------------------------------------------------------
	# Allows alternative introspection and all that can of worms brings.
	# ----------------------------------------------------------------------------
	
	runtime.introspector.uberspect = NVelocity.Util.Introspection.UberspectImpl
	
	
	# ----------------------------------------------------------------------------
	# SECURE INTROSPECTOR
	# ----------------------------------------------------------------------------
	# If selected, prohibits methods in certain classes and packages from being 
	# accessed.
	# ----------------------------------------------------------------------------
	
	#introspector.restrict.packages = java.lang.reflect
	
	# The two most dangerous classes
	
	#introspector.restrict.classes = java.lang.Class
	#introspector.restrict.classes = java.lang.ClassLoader
	                
	# Restrict these for extra safety
	
	#introspector.restrict.classes = java.lang.Compiler
	#introspector.restrict.classes = java.lang.InheritableThreadLocal
	#introspector.restrict.classes = java.lang.Package
	#introspector.restrict.classes = java.lang.Process
	#introspector.restrict.classes = java.lang.Runtime
	#introspector.restrict.classes = java.lang.RuntimePermission
	#introspector.restrict.classes = java.lang.SecurityManager
	#introspector.restrict.classes = java.lang.System
	#introspector.restrict.classes = java.lang.Thread
	#introspector.restrict.classes = java.lang.ThreadGroup
	#introspector.restrict.classes = java.lang.ThreadLocal
	
	
