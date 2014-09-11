using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
    /// <summary>
    /// Provides enhanced application tracing functionality.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class can be used to provide a standardised form of tracing throughout the application which makes
    /// diagnosis of issues much easier, particularly on deployed systems. The most important methods from the 
    /// point of following program flow are <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace.TraceIn"/>,
    /// <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace.TraceOut"/> and 
    /// <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace.TraceError"/>. The other trace methods can also
    /// be used to provide information about actions being performed or the current state of the application.
    /// </para>
    /// <para>
    /// Verbose tracing modifies the output from the <b>TraceIn</b> and <b>TraceOut</b> methods. When the attached 
    /// Switch has verbose on, the parameters displayed by TraceIn and TraceOut will have any complex types expanded 
    /// using ToString() to show their state.  When verbose is off on the type name will be displayed.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example shows a method which is well instrumented using this class, which is accessed via
    /// the <see cref="Microsoft.Sdc.BizTalkOM.TraceManager"/> class:
    /// <code>
    /// public float Divide(float numerator, float denominator)
    /// {
    ///		//trace method entry with the arguments provided
    ///		TraceManager.SmartTrace.TraceIn(numerator, denominator);
    ///		float result = 0F;
    ///		try
    ///		{
    ///			result = numerator / denominator;	
    ///		}
    ///		catch (Exception ex)
    ///		{
    ///			//trace the error
    ///			TraceManager.SmartTrace.TraceError(ex);
    ///			throw;
    ///		}
    ///		//trace function exit with the return value
    ///		TraceManager.SmartTrace.TraceOut(result);
    ///		return result;
    /// }
    /// </code>
    /// </example>
    public sealed class SmartTrace
    {
        private SmartSwitch m_switch = null;
        private ILogger logger;
        internal bool isTracingOn { get; set; }

        #region Constructors

        /// <summary>
        /// Create a new <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/>.
        /// </summary>
        /// <param name="displayName">
        /// Name of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        internal SmartTrace(string displayName)
            : this(displayName, string.Empty)
        {
            logger = LoggerFactory.GetLogger();
        }

        /// <summary>
        /// Create a new <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/>.
        /// </summary>
        /// <param name="displayName">
        /// Name of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        /// <param name="description">
        /// Description of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        internal SmartTrace(string displayName, string description)
            : this(displayName, description, 0)
        {
        }

        /// <summary>
        /// Create a new <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/>.
        /// </summary>
        /// <param name="displayName">
        /// Name of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        /// <param name="description">
        /// Description of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        /// <param name="level">
        /// Initial <see cref="Microsoft.Sdc.BizTalkOM.TraceTypes"/> level to use.
        /// </param>
        internal SmartTrace(string displayName, string description, TraceTypes level)
            : this(displayName, description, (int)level)
        {
            logger = LoggerFactory.GetLogger();
        }

        /// <summary>
        /// Create a new <see cref="Microsoft.Sdc.BizTalkOM.SmartTrace"/>.
        /// </summary>
        /// <param name="displayName">
        /// Name of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        /// <param name="description">
        /// Description of the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> to use.
        /// </param>
        /// <param name="level">
        /// Initial trace level value to apply to the switch. This value is mapped to the 
        /// <see cref="Microsoft.Sdc.BizTalkOM.TraceTypes"/> enumeration.
        /// </param>
        internal SmartTrace(string displayName, string description, int level)
        {
            m_switch = new SmartSwitch(displayName, description, level);
            logger = LoggerFactory.GetLogger();
        }

        #endregion

        #region TraceIn methods

        public void TraceIn()
        {
            if (this.isTracingOn)
            {
                MethodBase method = GetCallingMethod();
                //	Build the trace out message, working out the name of the method being traced out.
                StringBuilder traceString = new StringBuilder("TraceIn : ");
                this.AppendMethodName(method, traceString);
                traceString.Append("()");
                //	Write the message to the listeners.
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// To be called on method entry. This overload is recommended for most purposes.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="parameters">
        /// The values of the parameters that were passed into the function that is calling this trace function.
        /// </param>
        public void TraceIn(params object[] parameters)
        {
            if (this.isTracingOn)
            {
                MethodBase method = GetCallingMethod();
                //	Build the name of the method being traced.
                StringBuilder traceString = new StringBuilder("TraceIn  : ");
                this.AppendMethodName(method, traceString);
                //	Build the parameter list.
                if (parameters.Length > 0)
                {
                    ParameterInfo[] formalParameters = method.GetParameters();
                    //	Output the name and value of each parameter (until we reach the end of
                    //	the parameters for the function, or the list of values passed in).
                    traceString.Append("(");
                    for (int i = 0; i < parameters.Length && i < formalParameters.Length; i++)
                    {
                        //	Put a comma after the previous parameter.
                        if (i != 0)
                        {
                            traceString.Append(", ");
                        }
                        this.AppendParameter(formalParameters[i].Name, parameters[i], traceString, Switch.TraceVerbose);
                    }
                    traceString.Append(")");
                }
                else
                {
                    //	No parameters to be included in the method signature.
                    traceString.Append("()");
                }
                //	Write the details to the trace listeners.
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// To be called on method entry. This overload allows you to explicitly control the names and values
        /// that will be traced.
        /// </summary>
        /// <param name="functionName">
        /// The name of the function that is doing the tracing.
        /// </param>
        /// <param name="stackValues">
        /// A <see cref="System.Collections.Specialized.NameValueCollection"/> to be included in the trace string 
        /// in a <c>name = value</c> format.
        /// </param>
        public void TraceIn(string functionName, NameValueCollection stackValues)
        {
            if (this.isTracingOn)
            {
                StringBuilder traceString = new StringBuilder("TraceIn : ");
                traceString.Append(functionName);
                if (stackValues != null)
                {
                    foreach (DictionaryEntry de in stackValues)
                    {
                        traceString.Append(" : ");
                        traceString.Append(de.Key);
                        traceString.Append(" = ");
                        traceString.Append(de.Value);
                    }
                }
                this.InternalWriteLine(traceString);
            }
        }

        #endregion

        #region TraceOut methods

        /// <summary>
        /// To be called when leaving a method. This overload is recommended for functions that have only input
        /// parameters and which do not have a return value.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        public void TraceOut()
        {
            if (this.isTracingOn)
            {
                MethodBase method = GetCallingMethod();
                //	Build the trace out message, working out the name of the method being traced out.
                StringBuilder traceString = new StringBuilder("TraceOut : ");
                this.AppendMethodName(method, traceString);
                traceString.Append("()");
                //	Write the message to the listeners.
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// To be called when leaving a method. This overload is recommended for functions that have only input
        /// parameters and which have a return value.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="returnValue">
        /// The return value of the method.
        /// </param>
        public void TraceOut(object returnValue)
        {
            if (this.isTracingOn)
            {
                MethodBase method = GetCallingMethod();
                //	Build the trace out message, working out the name of the method being traced out.
                StringBuilder traceString = new StringBuilder("TraceOut : ");
                this.AppendMethodName(method, traceString);
                this.AppendParameter("()=", returnValue, traceString, Switch.TraceVerbose);
                //	Write the message to the listeners.
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// To be called when leaving a method. This overload is recommended for functions that have <b>ref</b> or
        /// <b>out</b> parameters or parameters that are changed by the method.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="parameters">
        /// The values of the parameters that were passed into the function that is calling this trace function.
        /// </param>
        public void TraceOut(params object[] parameters)
        {
            if (this.isTracingOn)
            {
                if (parameters == null) parameters = new string[] { "NULL" };

                MethodBase method = GetCallingMethod();
                //	Build the trace out message, working out the name of the method being traced out.
                StringBuilder traceString = new StringBuilder("TraceOut : ");
                this.AppendMethodName(method, traceString);
                //	Build the parameter list.
                if (parameters.Length > 0)
                {
                    ParameterInfo[] formalParameters = method.GetParameters();
                    //	Output the name and value of each parameter (until we reach the end of
                    //	the parameters for the function, or the list of values passed in).
                    traceString.Append("(");
                    for (int i = 0; i < parameters.Length && i < formalParameters.Length; i++)
                    {
                        //	Put a comma after the previous parameter.
                        if (i != 0)
                        {
                            traceString.Append(", ");
                        }
                        this.AppendParameter(formalParameters[i].Name, parameters[i], traceString, Switch.TraceVerbose);
                    }
                    traceString.Append(")");
                }
                else
                {
                    //	No parameters to be included in the method signature.
                    traceString.Append("()");
                }
                //	Write the message to the listeners.
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// To be called when leaving a method. This overload allows you to explicitly control the names and values
        /// that will be traced.
        /// </summary>
        /// <param name="functionName">
        /// The name of the function that is doing the tracing.
        /// </param>
        /// <param name="stackValues">
        /// A <see cref="System.Collections.Specialized.NameValueCollection"/> to be included in the trace string 
        /// in a <c>name = value</c> format.
        /// </param>
        public void TraceOut(string functionName, NameValueCollection stackValues)
        {
            if (this.isTracingOn)
            {
                StringBuilder traceString = new StringBuilder("TraceOut : ");
                traceString.Append(functionName);
                if (stackValues != null)
                {
                    foreach (DictionaryEntry de in stackValues)
                    {
                        traceString.Append(" : ");
                        traceString.Append(de.Key);
                        traceString.Append(" = ");
                        traceString.Append(de.Value);
                    }
                }
                this.InternalWriteLine(traceString);
            }
        }

        #endregion

        #region TraceInfo methods

        /// <summary>
        /// Write an information message containing the passed text.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="message">
        /// The text of the message.
        /// </param>
        public void TraceInfo(string message)
        {
            //	Pass through to the generic method.
            TraceMsg((int)TraceTypes.Info, message);
        }

        /// <summary>
        /// Write an information message containing the passed arguments in the given format.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="format">
        /// Format to be used when writing the args to a string.
        /// </param>
        /// <param name="args">
        /// The arguments to insert into the format string.
        /// </param>
        public void TraceInfo(string format, params object[] args)
        {
            //	Pass through to the generic method.
            TraceMsg((int)TraceTypes.Info, format, args);
        }

        #endregion

        #region TraceWarning methods

        /// <summary>
        /// Write a Warning message containing the passed text.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="message">
        /// The text of the message.
        /// </param>
        public void TraceWarning(string message)
        {
            //	Pass through to the generic TraceMsg method.
            TraceMsg((int)TraceTypes.Warning, message);
        }

        /// <summary>
        /// Write a Warning message containing the passed arguments in the given format.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="format">
        /// Format to be used when writing the args to a string.
        /// </param>
        /// <param name="args">
        /// The arguments to insert into the format string.
        /// </param>
        public void TraceWarning(string format, params object[] args)
        {
            //	Pass through to the generic TraceMsg method.
            TraceMsg((int)TraceTypes.Warning, format, args);
        }

        #endregion

        #region TraceError methods

        /// <summary>
        /// Write an Error message containing the passed text.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="message">
        /// The text of the message.
        /// </param>
        public void TraceError(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
            //	Pass through to the generic TraceMsg method.
            TraceMsg((int)TraceTypes.Error, message);
        }

        /// <summary>
        /// Write an Error message containing the passed arguments in the given format.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="format">
        /// Format to be used when writing the args to a string.
        /// </param>
        /// <param name="args">
        /// The arguments to insert into the format string.
        /// </param>
        public void TraceError(string format, params object[] args)
        {
            //	Pass through to the generic TraceMsg method.
            TraceMsg((int)TraceTypes.Error, format, args);
        }

        /// <summary>
        /// Write an error message consisting of the details of an <see cref="System.Exception"/>.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="exception">
        /// The <see cref="System.Exception"/> to write the message about.
        /// </param>
        public void TraceError(Exception exception)
        {
            //  Pass through to the generic TraceMsg method with the details of the exception.
            TraceMsg((int)TraceTypes.Error, exception.ToString());
        }

        #endregion

        #region TraceMsg methods

        /// <summary>
        /// Will output the passed message to the trace listeners if the trace switch indicates that
        /// the message type is one that can be produced.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="traceType">
        /// Type of trace message.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        public void TraceMsg(int traceType, string message)
        {
            System.Diagnostics.Trace.WriteLine(message);

            //	If the particulare message is on, output it.
            if (0 != (Switch.Level & traceType))
            {
                MethodBase method = this.GetCallingMethod();
                StringBuilder traceString = null;
                //	Build the Info message.
                switch (traceType)
                {
                    case (int)TraceTypes.Error:
                        traceString = new StringBuilder("TraceErr : ");
                        break;
                    case (int)TraceTypes.Warning:
                        traceString = new StringBuilder("TraceWarn: ");
                        break;
                    case (int)TraceTypes.Info:
                        traceString = new StringBuilder("TraceInfo: ");
                        break;
                    default:
                        traceString = new StringBuilder("TraceDbg : ");
                        break;
                }
                this.AppendMethodName(method, traceString);
                traceString.Append(": ");
                //	Append the message.
                traceString.Append(message);
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// Will output the passed message to the trace listeners if the trace switch indicates that
        /// the message type is one that can be produced.
        /// </summary>
        /// <remarks>
        /// This method will look up the name of the function that is calling it by doing a stack walk.
        /// </remarks>
        /// <param name="traceType">
        /// Type of trace message.
        /// </param>
        /// <param name="format">
        /// Format to be used when writing the args to a string.
        /// </param>
        /// <param name="args">
        /// The arguments to insert into the format string.
        /// </param>
        public void TraceMsg(int traceType, string format, params object[] args)
        {
            //	If the particulare message is on, output it.
            if (0 != (Switch.Level & traceType))
            {
                MethodBase method = this.GetCallingMethod();
                StringBuilder traceString = null;
                //	Build the Info message.
                switch (traceType)
                {
                    case (int)TraceTypes.Error:
                        traceString = new StringBuilder("TraceErr : ");
                        break;
                    case (int)TraceTypes.Warning:
                        traceString = new StringBuilder("TraceWarn: ");
                        break;
                    case (int)TraceTypes.Info:
                        traceString = new StringBuilder("TraceInfo: ");
                        break;
                    default:
                        traceString = new StringBuilder("TraceDbg : ");
                        break;
                }
                this.AppendMethodName(method, traceString);
                traceString.Append(": ");
                //	Append the formatted argument list.
                try
                {
                    traceString.AppendFormat(format, args);
                }
                catch (ArgumentNullException ex)
                {
                    traceString.AppendFormat("** TRACE MESSAGE FORMATTING ERROR: {0} **", ex.Message);
                }
                catch (FormatException ex)
                {
                    traceString.AppendFormat("** TRACE MESSAGE FORMATTING ERROR: {0} **", ex.Message);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    traceString.AppendFormat("** TRACE MESSAGE FORMATTING ERROR: {0} **", ex.Message);
                }
                this.InternalWriteLine(traceString);
            }
        }

        /// <summary>
        /// Will output the passed message to the trace listeners if the trace switch indicates that
        /// the message type is one that can be produced.
        /// </summary>
        /// <param name="traceType">
        /// Type of trace message.
        /// </param>
        /// <param name="message">
        /// The message to send.
        /// </param>
        public void TraceMsg(TraceTypes traceType, string message)
        {
            TraceMsg((int)traceType, message);
        }

        /// <summary>
        /// Will output the passed message to the trace listeners if the trace switch indicates that
        /// the message type is one that can be produced.
        /// </summary>
        /// <param name="traceType">
        /// Type of trace message.
        /// </param>
        /// <param name="format">
        /// Format to be used when writing the args to a string.
        /// </param>
        /// <param name="args">
        /// The arguments to insert into the format string.
        /// </param>
        public void TraceMsg(TraceTypes traceType, string format, params object[] args)
        {
            TraceMsg((int)traceType, format, args);
        }

        #endregion

        #region Property accessors

        /// <summary>
        /// Gets the <see cref="Microsoft.Sdc.BizTalkOM.SmartSwitch"/> that this class uses to decide 
        /// whether to produce tracing messages.
        /// </summary>
        public SmartSwitch Switch
        {
            get
            {
                return m_switch;
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// This will move up the stack until we get to the method that called into this class.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Reflection.MethodBase"/> detailing the calling method.
        /// </returns>
        private MethodBase GetCallingMethod()
        {
            const int INDEX_CALLING_METHOD = 2;		// This will always be at least 2 calls deep.
            StackTrace stack = new StackTrace();
            MethodBase method = null;

            //	Move up the stack trace, until we leave this class and enter the calling one.
            //	(If something weird happens and we don't leave this class just go to the top).
            string thisClassName = this.GetType().Name;
            for (int offSet = INDEX_CALLING_METHOD; offSet < stack.FrameCount; offSet++)
            {
                method = stack.GetFrame(offSet).GetMethod();
                if (method.DeclaringType.Name != thisClassName)
                {
                    break;
                }
            }
            //	Return the method that called into this class.
            return method;
        }

        /// <summary>
        /// Will append the details of the passed method, to the passed <see cref="System.Text.StringBuilder"/>.
        /// The method name will be in the format <b>Namespace.Class.Method</b>.
        /// </summary>
        /// <param name="method">
        /// The method to append the name of.
        /// </param>
        /// <param name="traceString">
        /// The <see cref="System.Text.StringBuilder"/> to append it to.
        /// </param>
        private void AppendMethodName(MethodBase method, StringBuilder traceString)
        {
            //	If neccessary prefix the method name with the Hash of the current Thread.
            if (Switch.TraceThreadHash)
            {
                traceString.Append("[" + System.Threading.Thread.CurrentThread.GetHashCode() + "]");
            }
            if (Switch.TraceThreadName)
            {
                traceString.Append("[" + System.Threading.Thread.CurrentThread.Name + "]");
            }
            //	Write the method name including namespace.
            traceString.Append(method.DeclaringType.Namespace);
            traceString.Append(".");
            traceString.Append(method.DeclaringType.Name);
            traceString.Append(".");
            traceString.Append(method.Name);
        }

        /// <summary>
        /// Will append the details of the passed method parameter to the passed StringBuilder.
        /// It will be in the format "[ParameterName]=[ParameterValue]".
        /// </summary>
        /// <remarks>
        /// If the name of the parameter is "password" or "pwd" then the value will be changed to asterisks for
        /// security. These matches are not case sensitive.
        /// </remarks>
        /// <param name="parameterName">
        /// Name of the parameter to append.
        /// </param>
        /// <param name="parameter">
        /// Value of the parameter to append.
        /// </param>
        /// <param name="traceString">
        /// <see cref="System.Text.StringBuilder"/> to write to.
        /// </param>
        /// <param name="traceVerbose">
        /// Are we performing verbose tracing?
        /// </param>
        private void AppendParameter(string parameterName, object parameter, StringBuilder traceString,
            bool traceVerbose)
        {
            bool isPassword = false;		// Is the parameter a password?
            try
            {
                //	Work out whether this parameter is a Password.  
                if (string.Compare(parameterName.Trim(), "password", true, CultureInfo.InvariantCulture) == 0)
                {
                    isPassword = true;
                }
                else if (string.Compare(parameterName.Trim(), "pwd", true, CultureInfo.InvariantCulture) == 0)
                {
                    isPassword = true;
                }
                //	Append the name of the parameter.
                traceString.Append(parameterName + "=");
                //	Append the value of the parameter.
                if (null == parameter)
                {
                    //	Deal with a parameter that is null.
                    traceString.Append("<null>");
                }
                else
                {
                    //	Parameter isn't null.  Serialise it for inclusion in the message.
                    if (isPassword)
                    {
                        //	Parameter is a password.  Mask it for output.
                        traceString.Append("\"<******>\"");
                    }
                    else
                    {
                        if (traceVerbose)
                        {
                            //	Verbose tracing - ToString everything, including complex types.
                            if (parameter is string)
                            {
                                traceString.Append("\"" + parameter + "\"");
                            }
                            else
                            {
                                traceString.Append(parameter.ToString());
                            }
                        }
                        else
                        {
                            //	Only include the value of ValueTypes and strings.  Don't get the value of complex types.
                            if (parameter is string)
                            {
                                traceString.Append("\"" + parameter + "\"");
                            }
                            else if (parameter.GetType().IsValueType)
                            {
                                traceString.Append(parameter.ToString());
                            }
                            else
                            {
                                traceString.Append("<" + parameter.GetType().Name + ">");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                traceString.Append("Exception reading parameter. Details = " + ex.ToString());
            }
        }

        /// <summary>
        /// The final write of a trace string to the trace output.
        /// </summary>
        /// <param name="value">
        /// The <see cref="System.Text.StringBuilder"/> containing the trace string.
        /// </param>
        private void InternalWriteLine(StringBuilder value)
        {
            InternalWriteLine(value.ToString());
        }

        /// <summary>
        /// The final write of a trace string to the trace output.
        /// </summary>
        /// <param name="text">
        /// The string to write out.
        /// </param>
        private void InternalWriteLine(string text)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.Append(DateTime.UtcNow.ToString("u", CultureInfo.CurrentCulture));
            sb.Append(" : ");
            sb.Append(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            sb.Append(" : ");
            sb.Append(text);
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            this.logger.LogInfo(text);
        }

        #endregion
    }

}
