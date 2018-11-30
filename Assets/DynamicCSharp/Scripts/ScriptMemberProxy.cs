﻿using System.Reflection;

namespace DynamicCSharp
{
    /// <summary>
    /// Represents a member group collection that allows access to all members of a specific type.
    /// The members data may be accessed via its member name which means that non-concrete communication is possible.
    /// </summary>
    /// <seealso cref="FieldProxy"/>"/>
    /// <seealso cref="PropertyProxy"/>
    public interface IMemberProxy
    {
        // Properties
        /// <summary>
        /// Attempt to read from or write to a member of the managed type.
        /// </summary>
        /// <param name="name">The name of the member to find</param>
        /// <returns>The value for the specified member</returns>
        object this[string name] { get; set; }
    }

    /// <summary>
    /// A <see cref="FieldProxy"/> allows access to the fields of a type using a string identifier to lookup the field name"/> 
    /// </summary>
    public sealed class FieldProxy : IMemberProxy
    {
        // Private
        private ScriptProxy owner = null;

        // Properties
        /// <summary>
        /// Attempt to read from or write to a field on the managed type with the specified name.
        /// The object value will need to be cast to the desired type when reading the value.
        /// </summary>
        /// <param name="name">The name of the field to modify</param>
        /// <returns>The value of the field witht he specified name</returns>
        /// <exception cref="TargetException">The target field could not be found on the managed type</exception>
        public object this[string name]
        {
            get
            {
                // Try to get a field with the specified name
                FieldInfo info = owner.ScriptType.FindCachedField(name);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", owner.ScriptType, name));

                // Attempt to get the field value
                return info.GetValue(owner.Instance);
            }

            set
            {
                // Try to get a field with the specified name
                FieldInfo info = owner.ScriptType.FindCachedField(name);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", owner.ScriptType, name));

                // Attempt to set the value
                info.SetValue(owner.Instance, value);
            }
        }

        // Constructor
        internal FieldProxy(ScriptProxy owner)
        {
            this.owner = owner;
        }
    }

    /// <summary>
    /// A <see cref="PropertyProxy"/> allows access to the properties of a type using a string identifier to lookup the field name"/> 
    /// </summary>
    public sealed class PropertyProxy : IMemberProxy
    {
        // Private
        private ScriptProxy owner = null;

        // Properties
        /// <summary>
        /// Attempt to read from or write to a field on the managed type with the specified name.
        /// Any exceptions thrown by the set or get accessor will not be handled.
        /// The object value will need to be cast to the desired type when reading the value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The value of the field witht he specified name</returns>
        /// <exception cref="TargetException">The target property could not be found on the managed type</exception>
        /// <exception cref="TargetException">Attempted to read from the property but a get accessor could not be found</exception>
        /// <exception cref="TargetException">Attempted to write to the property but a set accessor could not be found</exception>
        public object this[string name]
        {
            get
            {
                // Try to get a field with the specified name
                PropertyInfo info = owner.ScriptType.FindCachedProperty(name);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a property called '{1}'", owner.ScriptType, name));

                // Check for a getter
                if (info.CanRead == false)
                    throw new TargetException(string.Format("The property '{0}' was found but it does not define a get accessor", name));

                // Attempt to get the field value
                return info.GetValue(owner.Instance, null);
            }

            set
            {
                // Try to get a field with the specified name
                PropertyInfo info = owner.ScriptType.FindCachedProperty(name);

                // Check for field not found
                if (info == null)
                    throw new TargetException(string.Format("Type '{0}' does not define a property called '{1}'", owner.ScriptType, name));

                // Check for setter
                if (info.CanWrite == false)
                    throw new TargetException(string.Format("The property '{0}' was found but it does not define a set accessor", name));

                // Attempt to set the value
                info.SetValue(owner.Instance, value, null);
            }
        }

        // Constructor
        internal PropertyProxy(ScriptProxy owner)
        {
            this.owner = owner;
        }
    }
}
