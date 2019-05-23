namespace VanType
{
    using System;

    /// <summary>
    /// The class property.
    /// </summary>
    public struct ClassProperty
    {
        /// <summary>
        /// Gets or sets the class type.
        /// </summary>
        public Type ClassType;

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassProperty"/> structure.
        /// </summary>
        /// <param name="classType">The type of the class.</param>
        /// <param name="propertyName">The property name.</param>
        public ClassProperty(Type classType, string propertyName)
        {
            ClassType = classType ?? throw new ArgumentNullException(nameof(classType));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }
    }
}
