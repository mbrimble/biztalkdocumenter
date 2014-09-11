using System;
using System.Linq;

namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    public enum ParserError
    {
        None,
        ArgumentNotLocated,
        UnableToParse
    }

    /// <summary>
    /// The arg parser.
    /// </summary>
    public class ArgParser
    {
        /// <summary>
        /// The input arguments.
        /// </summary>
        private string[] inArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgParser"/> class.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public ArgParser(string[] args)
        {
            this.inArgs = args;
        }

        /// <summary>
        /// Checks if the specified argument exists.
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <returns>
        /// The exists.
        /// </returns>
        public bool Exists(string argName)
        {
            string argument = (from arg in this.inArgs
                               where arg.Contains(argName)
                               select arg).FirstOrDefault();

            if (String.IsNullOrEmpty(argument))
                return false;
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if the argument (or its corresponding abbreviation) exists.
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="argAbbreviation">
        /// The arg abbreviation.
        /// </param>
        /// <returns>
        /// The exists.
        /// </returns>
        public bool Exists(string argName, string argAbbreviation)
        {
            // Without the prefix, the abbreviation could match
            // any parameter (potentially) and any part of the name
            // of a parameter. The prefix eliminates this error. 
            argAbbreviation = EnsurePrefix(argAbbreviation);
            bool argExists = this.Exists(argName);
            if (!argExists)
            {
                argExists = this.Exists(argAbbreviation);
            }

            return argExists;
        }


        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <returns>
        /// The get value.
        /// </returns>
        public string GetValue(string argName, out ParserError error)
        {
            string value = null;
            error = ParserError.None;

            string argument = (from arg in this.inArgs
                               where arg.Contains(argName)
                               select arg).FirstOrDefault();

            if (String.IsNullOrEmpty(argument))
            {
                error = ParserError.ArgumentNotLocated;
            }
            else
            {
                try
                {
                    value = argument.Replace("/", String.Empty);
                    int firstColonIndex = value.IndexOf(":", 0);
                    if (firstColonIndex < 1)
                    {
                        throw new FormatException("Could not locate : to delimit parameter name from value.");
                    }
                    value = value.Substring(firstColonIndex + 1, value.Length - (firstColonIndex + 1));
                }
                catch (Exception e)
                {
                    error = ParserError.UnableToParse;
                    value = null;
                }
            }

            return value;
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="argName">
        /// The arg name.
        /// </param>
        /// <param name="argAbbreviation">
        /// The arg abbreviation.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <returns>
        /// The get value.
        /// </returns>
        public string GetValue(string argName, string argAbbreviation, out ParserError error)
        {
            argAbbreviation = EnsurePrefix(argAbbreviation);
            string value = this.GetValue(argName, out error);
            if (value == null)
            {
                value = this.GetValue(argAbbreviation, out error);
            }

            return value;
        }

        /// <summary>
        /// The ensure prefix.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The ensure prefix.
        /// </returns>
        private static string EnsurePrefix(string source)
        {
            if (!source.StartsWith("/"))
            {
                source = "/" + source;
            }

            return source;
        }
    }
}
