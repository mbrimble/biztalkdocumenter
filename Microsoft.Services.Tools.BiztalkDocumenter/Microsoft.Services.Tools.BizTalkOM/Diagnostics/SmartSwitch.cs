using System.Diagnostics;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    /// <summary>
    /// This is a replacement for the default <see cref="System.Diagnostics.TraceSwitch"/> class.  This class
    /// publishes the Level as an int rather than as an enum as with the default implementation.  This allows us 
    /// to use any value for the tracing level rather than the restricted 5 values of the default class.
    /// </summary>
    public class SmartSwitch : Switch
    {
        #region "private Declarations"

        private int traceLevel = 0;

        //	These are updated whenever the Level property is set
        //	so that they do not need to be dynamically evaluated
        //	every time their equivalent public accessor is called.
        private bool isTraceInOut = false;
        private bool isTraceError = false;
        private bool isTraceWarning = false;
        private bool isTraceInfo = false;
        private bool isTraceVerbose = false;
        private bool isTraceThreadHash = false;
        private bool isTraceThreadName = false;

        #endregion

        #region "Constructors"

        /// <summary>
        /// Creates a new SmartSwitch.
        /// </summary>
        /// <param name="displayName">The name of the Switch.</param>
        /// <param name="description">The description of the Switch.</param>
        public SmartSwitch(
            string displayName,
            string description)
            : base(displayName, description)
        {
            //	Get the Trace Level from the base class to force it to initialise it.
            this.Level = base.SwitchSetting;
        }

        /// <summary>
        /// Creates a new SmartSwitch, initialising its level.
        /// </summary>
        /// <param name="displayName">The name of the Switch.</param>
        /// <param name="description">The description of the Switch.</param>
        /// <param name="level">The initial level of the switch.</param>
        public SmartSwitch(
            string displayName,
            string description,
            int level)
            : base(displayName, description)
        {
            //	Store the initial level.  This will also update it in the base class.
            this.Level = level;
        }

        /// <summary>
        /// Creates a new SmartSwitch, initialising its level.
        /// </summary>
        /// <param name="displayName">The name of the Switch.</param>
        /// <param name="description">The description of the Switch.</param>
        /// <param name="level">The initial level of the switch.</param>
        public SmartSwitch(
            string displayName,
            string description,
            TraceTypes level)
            : base(displayName, description)
        {
            //	Store the initial level.  This will also update it in the base class.
            this.Level = (int)level;
        }

        #endregion

        #region "Events"

        /// <summary>
        /// This event occurs whenever the SwitchSetting is changed in the base class, including
        /// when it is initially read from the config file, or initialised during construction.
        /// </summary>
        protected override void OnSwitchSettingChanged()
        {
            //	Synchronise our local trace level with that from the base class.
            traceLevel = base.SwitchSetting;
        }

        #endregion

        #region "public Property Accessors"

        /// <summary>
        /// The current trace switch level.  Unlike the default TraceSwitch class
        /// this property is a raw int for this class.  This allows us the set the 
        /// level to any value, rather than the 0 - 4 allowed by the default TraceSwitch.
        /// </summary>
        public int Level
        {
            get
            {
                //	Read from the local copy of the trace level.
                return traceLevel;
            }
            set
            {
                //	Write the new value to the base class.  We will update
                //	our local copy in the OnSwitchSettingChanged event.
                base.SwitchSetting = value;

                //	Calculate the boolean switches for the standard message types.
                isTraceInOut = (0 != (value & (int)TraceTypes.InOut));
                isTraceError = (0 != (value & (int)TraceTypes.Error));
                isTraceWarning = (0 != (value & (int)TraceTypes.Warning));
                isTraceInfo = (0 != (value & (int)TraceTypes.Info));
                isTraceVerbose = (0 != (value & (int)TraceTypes.Verbose));
                isTraceThreadHash = (0 != (value & (int)TraceTypes.IncludeThreadHash));
                isTraceThreadName = (0 != (value & (int)TraceTypes.IncludeThreadName));
            }
        }

        /// <summary>
        /// Is Method/Code Block In/Out Tracing on?
        /// </summary>
        public bool TraceInOut
        {
            get
            {
                return isTraceInOut;
            }
        }

        /// <summary>
        /// Is error tracing on?
        /// </summary>
        public bool TraceError
        {
            get
            {
                return isTraceError;
            }
        }

        /// <summary>
        /// Is warning tracing on?
        /// </summary>
        public bool TraceWarning
        {
            get
            {
                return isTraceWarning;
            }
        }

        /// <summary>
        /// Is Info tracing on?
        /// </summary>
        public bool TraceInfo
        {
            get
            {
                return isTraceInfo;
            }
        }

        /// <summary>
        /// Is Verbose tracing on?
        /// </summary>
        public bool TraceVerbose
        {
            get
            {
                return isTraceVerbose;
            }
        }

        /// <summary>
        /// Is the Thread Hash included in the trace?
        /// </summary>
        public bool TraceThreadHash
        {
            get
            {
                return isTraceThreadHash;
            }
        }

        /// <summary>
        /// Is the thread name included in the trace?
        /// </summary>
        public bool TraceThreadName
        {
            get
            {
                return isTraceThreadName;
            }
        }


        #endregion
    }
}
