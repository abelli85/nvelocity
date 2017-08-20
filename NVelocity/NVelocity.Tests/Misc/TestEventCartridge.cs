namespace NVelocity.Tests.Misc
{
    using App.Event;
    using Context;
    using NUnit.Framework;
    using NVelocity.Util;
    using Runtime;

    public class TestEventCartridge : IReferenceInsertionEventHandler, INullSetEventHandler, IMethodExceptionEventHandler, IRuntimeServicesAware, IContextAware
    {
        virtual public IContext Context
        {
            set
            {
                this.context = value;
            }

        }
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

        /// <summary>  Event handler for when a reference is inserted into the output stream.</summary>
        public virtual object ReferenceInsert(string reference, object value_Renamed)
        {
            // as a test, make sure this EventHandler is initialized
            if (rs == null)
                Assert.Fail("Event handler not initialized!");


            /*
            *  if we have a value
            *  return a known value
            */
            string s = null;

            if (value_Renamed != null)
            {
                s = EventHandlingTestCase.REFERENCE_VALUE;
            }
            else
            {
                /*
                * we only want to deal with $floobie - anything
                *  else we let go
                */
                if (reference.Equals("$floobie"))
                {
                    s = EventHandlingTestCase.NO_REFERENCE_VALUE;
                }
            }
            return s;
        }

        /// <summary>  Event handler for when the right hand side of
        /// a #set() directive is null, which results in
        /// a Log message.  This method gives the application
        /// a chance to 'vote' on msg generation
        /// </summary>
        public virtual bool shouldLogOnNullSet(string lhs, string rhs)
        {
            // as a test, make sure this EventHandler is initialized
            if (rs == null)
                Assert.Fail("Event handler not initialized!");

            if (lhs.Equals("$settest"))
                return false;

            return true;
        }

        /// <summary>  Handles exceptions thrown during in-template method access</summary>
        public virtual object MethodException(System.Type claz, string method, System.Exception e)
        {
            // as a test, make sure this EventHandler is initialized
            if (rs == null)
                Assert.Fail("Event handler not initialized!");

            // only do processing if the switch is on
            if (context != null)
            {
                bool exceptionSwitch = context.ContainsKey("allow_exception");

                if (exceptionSwitch && method.Equals("throwException"))
                {
                    return "handler";
                }
                else
                    throw e;
            }
            else
                throw e;
        }

        internal IContext context;
    }
}
