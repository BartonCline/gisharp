// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;

namespace GISharp.GI
{
    /// <summary>
    /// The type of a <see cref="BaseInfo"/> struct.
    /// </summary>
    public enum InfoType
    {
        /// <summary>
        /// invalid type
        /// </summary>
        Invalid,

        /// <summary>
        /// function, see <see cref="FunctionInfo"/>
        /// </summary>
        Function,

        /// <summary>
        /// callback, see <see cref="FunctionInfo"/>
        /// </summary>
        Callback,

        /// <summary>
        /// struct, see <see cref="StructInfo"/>
        /// </summary>
        Struct,

        /// <summary>
        /// boxed, see <see cref="StructInfo"/> or <see cref="UnionInfo"/>
        /// </summary>
        Boxed,

        /// <summary>
        /// enum, see <see cref="EnumInfo"/>
        /// </summary>
        Enum,

        /// <summary>
        /// flags, see <see cref="EnumInfo"/>
        /// </summary>
        Flags,

        /// <summary>
        /// object, see <see cref="ObjectInfo"/>
        /// </summary>
        Object,

        /// <summary>
        /// interface, see <see cref="InterfaceInfo"/>
        /// </summary>
        Interface,

        /// <summary>
        /// constant, see <see cref="ConstantInfo"/>
        /// </summary>
        Constant,

        /// <summary>
        /// deleted, was <c>ErrorDomain</c>
        /// </summary>
        [Obsolete]
        Invalid0,

        /// <summary>
        /// union, see <see cref="UnionInfo"/>
        /// </summary>
        Union,

        /// <summary>
        /// enum value, see <see cref="ValueInfo"/>
        /// </summary>
        Value,

        /// <summary>
        /// signal, see <see cref="SignalInfo"/>
        /// </summary>
        Signal,

        /// <summary>
        /// virtual function, see <see cref="VFuncInfo"/>
        /// </summary>
        VFunc,

        /// <summary>
        /// GObject property, see <see cref="PropertyInfo"/>
        /// </summary>
        Property,

        /// <summary>
        /// struct or union field, see <see cref="FieldInfo"/>
        /// </summary>
        Field,

        /// <summary>
        /// argument of a function or callback, see <see cref="ArgInfo"/>
        /// </summary>
        Arg,

        /// <summary>
        /// type information, see <see cref="TypeInfo"/>
        /// </summary>
        Type,

        /// <summary>
        /// unresolved type, a type which is not present in the typelib, or any of its dependencies.
        /// </summary>
        Unresolved,
    }
}
