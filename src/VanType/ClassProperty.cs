namespace VanType
{
    using System;

    public struct ClassProperty
    {
        public ClassProperty(Type classType, string propertyName)
        {
            ClassType = classType ?? throw new ArgumentNullException(nameof(classType));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        public Type ClassType;
        public string PropertyName;
    }
}
