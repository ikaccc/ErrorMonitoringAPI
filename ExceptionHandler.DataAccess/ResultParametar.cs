using System;

namespace ExceptionHandler.DataAccess
{
    /// <summary>
    /// Declare the column name or number of the result
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ResultParameter : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance uses name or column.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is name; otherwise, <c>false</c>.
        /// </value>
        public bool IsName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ResultParameter(string name)
        {
            Name = name;
            IsName = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultParameter"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public ResultParameter(int column)
        {
            Column = column;
            IsName = false;
        }
    }
}