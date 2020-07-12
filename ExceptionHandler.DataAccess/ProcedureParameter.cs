using System;

namespace ExceptionHandler.DataAccess
{
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class ProcedureParameter : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProcedureParameter"/> is output.
        /// </summary>
        /// <value>
        ///   <c>true</c> if output; otherwise, <c>false</c>.
        /// </value>
        public bool Output { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcedureParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="output">if set to <c>true</c> [output].</param>
        public ProcedureParameter(string name, bool output = false)
        {
            Name = name;
            Output = output;
        }
    }
}
