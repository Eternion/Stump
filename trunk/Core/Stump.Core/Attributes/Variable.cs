
using System;

namespace Stump.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Variable : Attribute
    {
        public Variable()
        {
            DefinableByConfig = true;
            DefinableRunning = false;
        }


        public Variable(bool definableByConfig)
        {
            DefinableByConfig = definableByConfig;
            DefinableRunning = false;
        }

        public Variable(bool definableByConfig, bool definableRunning)
        {
            DefinableByConfig = definableByConfig;
            DefinableRunning = definableRunning;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this variable can be set by the config
        /// </summary>
        /// <value><c>true</c> if this variable can be set by the config; otherwise, <c>false</c>.</value>
        public bool DefinableByConfig
        {
            get;
            set;
        }

        ///<summary>
        ///  ets or sets a value indicating whether this variable can be set when server is running
        ///</summary>
        ///<value><c>true</c> if this variable can be set when server is running; otherwise, <c>false</c>.</value>
        public bool DefinableRunning
        {
            get;
            set;
        }
    }
}