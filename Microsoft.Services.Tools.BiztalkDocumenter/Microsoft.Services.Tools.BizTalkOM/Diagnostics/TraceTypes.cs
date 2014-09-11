using System;

namespace Microsoft.Services.Tools.BizTalkOM.Diagnostics
{
	/// <summary>
	/// These represent the binary switch values used to turn on/off the various types of trace message.
	/// </summary>
	[Flags]
	public enum TraceTypes
	{
		/// <summary>
		/// No Tracing.
		/// </summary>
		Off					= 0,
		/// <summary>
		/// Trace in and out of methods and code blocks.
		/// </summary>
		InOut				= 1,
		/// <summary>
		/// Trace output of error conditions.
		/// </summary>
		Error				= 2,
		/// <summary>
		/// Trace output of warning conditions.
		/// </summary>
		Warning				= 4,
		/// <summary>
		/// Trace output of general information.
		/// </summary>
		Info				= 8,		
		/// <summary>
		/// Includes the .NET Thread name with the message output.
		/// </summary>
		IncludeThreadName	= 128,
		/// <summary>
		/// Include the .Net Thread Hash with the message output.
		/// </summary>
		IncludeThreadHash	= 256,
		/// <summary>
		/// Trace output that is considered highly verbose.
		///	Also modifies the tracing produced from TraceIn/TraceOut so that reference types are expanded.  
		/// </summary>
		Verbose				= 512,
	}
}
