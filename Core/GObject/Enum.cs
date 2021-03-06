using System;
using System.Runtime.InteropServices;
using GISharp.Runtime;

namespace GISharp.GObject
{
    [AttributeUsage (AttributeTargets.Field)]
    public class EnumValueAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Nick { get; private set; }

        public EnumValueAttribute (string name = null, string nick = null)
        {
            Name = name;
            Nick = nick;
        }
    }

    public static class EnumExtensions
    {
        public static string GetValueName (System.Enum @enum)
        {
            var gtype = @enum.GetType ();
            throw new NotImplementedException ();
        }
    }

    public static class Enum
    {
        /// <summary>
        /// This function is meant to be called from the `complete_type_info`
        /// function of a #GTypePlugin implementation, as in the following
        /// example:
        /// </summary>
        /// <remarks>
        /// |[&lt;!-- language="C" --&gt;
        /// static void
        /// my_enum_complete_type_info (GTypePlugin     *plugin,
        ///                             GType            g_type,
        ///                             GTypeInfo       *info,
        ///                             GTypeValueTable *value_table)
        /// {
        ///   static const GEnumValue values[] = {
        ///     { MY_ENUM_FOO, "MY_ENUM_FOO", "foo" },
        ///     { MY_ENUM_BAR, "MY_ENUM_BAR", "bar" },
        ///     { 0, NULL, NULL }
        ///   };
        /// 
        ///   g_enum_complete_type_info (type, info, values);
        /// }
        /// ]|
        /// </remarks>
        /// <param name="gEnumType">
        /// the type identifier of the type being completed
        /// </param>
        /// <param name="info">
        /// the #GTypeInfo struct to be filled in
        /// </param>
        /// <param name="constValues">
        /// An array of #GEnumValue structs for the possible
        ///  enumeration values. The array is terminated by a struct with all
        ///  members being 0.
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_enum_complete_type_info (
            /* <type name="GType" type="GType" managed-name="GType" /> */
            /* transfer-ownership:none */
            GType gEnumType,
            /* <type name="TypeInfo" type="GTypeInfo*" managed-name="TypeInfo" /> */
            /* direction:out caller-allocates:0 transfer-ownership:full */
            out TypeInfo info,
            /* <type name="EnumValue" type="const GEnumValue*" managed-name="EnumValue" /> */
            /* transfer-ownership:none */
            IntPtr constValues);

        /// <summary>
        /// This function is meant to be called from the `complete_type_info`
        /// function of a #GTypePlugin implementation, as in the following
        /// example:
        /// </summary>
        /// <remarks>
        /// |[&lt;!-- language="C" --&gt;
        /// static void
        /// my_enum_complete_type_info (GTypePlugin     *plugin,
        ///                             GType            g_type,
        ///                             GTypeInfo       *info,
        ///                             GTypeValueTable *value_table)
        /// {
        ///   static const GEnumValue values[] = {
        ///     { MY_ENUM_FOO, "MY_ENUM_FOO", "foo" },
        ///     { MY_ENUM_BAR, "MY_ENUM_BAR", "bar" },
        ///     { 0, NULL, NULL }
        ///   };
        /// 
        ///   g_enum_complete_type_info (type, info, values);
        /// }
        /// ]|
        /// </remarks>
        /// <param name="gEnumType">
        /// the type identifier of the type being completed
        /// </param>
        /// <param name="info">
        /// the #GTypeInfo struct to be filled in
        /// </param>
        /// <param name="constValues">
        /// An array of #GEnumValue structs for the possible
        ///  enumeration values. The array is terminated by a struct with all
        ///  members being 0.
        /// </param>
        static void CompleteTypeInfo (GType gEnumType, out TypeInfo info, EnumValue[] constValues)
        {
            var constValues_ = GMarshal.CArrayToPtr<EnumValue> (constValues, nullTerminated: true);
            g_enum_complete_type_info (gEnumType, out info, constValues_);
        }

        /// <summary>
        /// Returns the #GEnumValue for a value.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="value">
        /// the value to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue for @value, or %NULL
        ///          if @value is not a member of the enumeration
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="EnumValue" type="GEnumValue*" managed-name="EnumValue" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_enum_get_value (
            /* <type name="EnumClass" type="GEnumClass*" managed-name="EnumClass" /> */
            /* transfer-ownership:none */
            IntPtr enumClass,
            /* <type name="gint" type="gint" managed-name="Gint" /> */
            /* transfer-ownership:none */
            int value);

        /// <summary>
        /// Returns the #GEnumValue for a value.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="value">
        /// the value to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue for @value, or %NULL
        ///          if @value is not a member of the enumeration
        /// </returns>
        public static EnumValue GetValue (EnumClass enumClass, int value)
        {
            if (enumClass == null) {
                throw new ArgumentNullException (nameof (enumClass));
            }

            var ret_ = g_enum_get_value (enumClass.Handle, value);
            var ret = Marshal.PtrToStructure<EnumValue> (ret_);

            return ret;
        }

        /// <summary>
        /// Looks up a #GEnumValue by name.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="name">
        /// the name to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue with name @name,
        ///          or %NULL if the enumeration doesn't have a member
        ///          with that name
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="EnumValue" type="GEnumValue*" managed-name="EnumValue" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_enum_get_value_by_name (
            /* <type name="EnumClass" type="GEnumClass*" managed-name="EnumClass" /> */
            /* transfer-ownership:none */
            IntPtr enumClass,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr name);

        /// <summary>
        /// Looks up a #GEnumValue by name.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="name">
        /// the name to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue with name @name,
        ///          or %NULL if the enumeration doesn't have a member
        ///          with that name
        /// </returns>
        public static EnumValue GetValueByName (EnumClass enumClass, string name)
        {
            if (enumClass == null) {
                throw new ArgumentNullException (nameof (enumClass));
            }
            if (name == null) {
                throw new ArgumentNullException (nameof (name));
            }

            var name_ = GMarshal.StringToUtf8Ptr (name);
            var ret_ = g_enum_get_value_by_name (enumClass.Handle, name_);
            GMarshal.Free (name_);
            var ret = Marshal.PtrToStructure<EnumValue> (ret_);

            return ret;
        }

        /// <summary>
        /// Looks up a #GEnumValue by nickname.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="nick">
        /// the nickname to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue with nickname @nick,
        ///          or %NULL if the enumeration doesn't have a member
        ///          with that nickname
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="EnumValue" type="GEnumValue*" managed-name="EnumValue" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_enum_get_value_by_nick (
            /* <type name="EnumClass" type="GEnumClass*" managed-name="EnumClass" /> */
            /* transfer-ownership:none */
            IntPtr enumClass,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr nick);

        /// <summary>
        /// Looks up a #GEnumValue by nickname.
        /// </summary>
        /// <param name="enumClass">
        /// a #GEnumClass
        /// </param>
        /// <param name="nick">
        /// the nickname to look up
        /// </param>
        /// <returns>
        /// the #GEnumValue with nickname @nick,
        ///          or %NULL if the enumeration doesn't have a member
        ///          with that nickname
        /// </returns>
        public static EnumValue GetValueByNick (EnumClass enumClass, string nick)
        {
            if (enumClass == null) {
                throw new ArgumentNullException (nameof (enumClass));
            }
            if (nick == null) {
                throw new ArgumentNullException (nameof (nick));
            }

            var nick_ = GMarshal.StringToUtf8Ptr (nick);
            var ret_ = g_enum_get_value_by_nick (enumClass.Handle, nick_);
            GMarshal.Free (nick_);
            var ret = Marshal.PtrToStructure<EnumValue> (ret_);

            return ret;
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern GType g_enum_register_static (IntPtr typeName, IntPtr values);

        public static GType RegisterStatic (string typeName, EnumValue[] values)
        {
            GType.AssertGTypeName (typeName);
            var typeName_ = GMarshal.StringToUtf8Ptr (typeName);
            var values_ = GMarshal.CArrayToPtr<EnumValue> (values, nullTerminated: true);
            var ret = g_enum_register_static (typeName_, values_);
            // values are never freed for the liftime of the program
            return ret;
        }
    }

    /// <summary>
    /// A structure which contains a single enum value, its name, and its
    /// nickname.
    /// </summary>
    public struct EnumValue
    {
        /// <summary>
        /// the enum value
        /// </summary>
        public int Value;

        /// <summary>
        /// the name of the value
        /// </summary>
        public IntPtr ValueName;

        /// <summary>
        /// the nickname of the value
        /// </summary>
        public IntPtr ValueNick;
    }
}
