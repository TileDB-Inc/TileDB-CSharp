using System;
using System.Diagnostics;

namespace TileDB.Interop
{
    /// <summary>Defines the type of a member as it was used in the native signature.</summary>
    [System.AttributeUsage(System.AttributeTargets.Struct | System.AttributeTargets.Enum | System.AttributeTargets.Property | System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    [System.Diagnostics.Conditional("DEBUG")]
    internal sealed partial class NativeTypeNameAttribute : System.Attribute
    {
        private readonly string _name;

        /// <summary>Initializes a new instance of the <see cref="NativeTypeNameAttribute" /> class.</summary>
        /// <param name="name">The name of the type that was used in the native signature.</param>
        public NativeTypeNameAttribute(string name)
        {
            _name = name;
        }

        /// <summary>Gets the name of the type that was used in the native signature.</summary>
        public string Name => _name;
    }
}
