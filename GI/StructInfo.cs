// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Runtime.InteropServices;

using GISharp.Runtime;

namespace GISharp.GI
{
    public sealed class StructInfo : RegisteredTypeInfo, IMethodContainer
    {

        InfoDictionary<FieldInfo> fields;

        public InfoDictionary<FieldInfo> Fields {
            get {
                if (fields == null) {
                    fields = new InfoDictionary<FieldInfo> (NFields, GetField);
                }
                return fields;
            }
        }

        InfoDictionary<FunctionInfo> methods;

        public InfoDictionary<FunctionInfo> Methods {
            get {
                if (methods == null) {
                    methods = new InfoDictionary<FunctionInfo> (NMethods, GetMethod);
                }
                return methods;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_find_method (IntPtr raw, IntPtr name);

        /// <summary>
        /// Finds the method.
        /// </summary>
        /// <returns>The method.</returns>
        /// <param name="name">Name.</param>
        /// <remarks>
        /// This seems to be unreliable. It causes a crash when struct is GObject.ObjectClass
        /// and cannot find methods in GObject.Closure
        [Obsolete ("Not really obsolete, but unreliable.")]
        public FunctionInfo FindMethod (string name)
        {
            IntPtr native_name = MarshalG.StringToUtf8Ptr (name);
            IntPtr raw_ret = g_struct_info_find_method (Handle, native_name);
            var ret = MarshalPtr<FunctionInfo> (raw_ret);
            MarshalG.Free (native_name);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern UIntPtr g_struct_info_get_alignment (IntPtr raw);

        public ulong Alignment {
            get {
                return (ulong)g_struct_info_get_alignment (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_get_field (IntPtr raw, int index);

        public FieldInfo GetField (int index)
        {
            IntPtr raw_ret = g_struct_info_get_field (Handle, index);
            return MarshalPtr<FieldInfo> (raw_ret);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_get_method (IntPtr raw, int index);

        public FunctionInfo GetMethod (int index)
        {
            IntPtr raw_ret = g_struct_info_get_method (Handle, index);
            return MarshalPtr<FunctionInfo> (raw_ret);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_struct_info_get_n_fields (IntPtr raw);

        int NFields {
            get {
                int raw_ret = g_struct_info_get_n_fields (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_struct_info_get_n_methods (IntPtr raw);

        int NMethods {
            get {
                int raw_ret = g_struct_info_get_n_methods (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern UIntPtr g_struct_info_get_size (IntPtr raw);

        public ulong Size {
            get {
                UIntPtr raw_ret = g_struct_info_get_size (Handle);
                var ret = (ulong)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_struct_info_is_foreign (IntPtr raw);

        public bool IsForeign {
            get {
                bool raw_ret = g_struct_info_is_foreign (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_struct_info_is_gtype_struct (IntPtr raw);

        public bool IsGTypeStruct {
            get {
                bool raw_ret = g_struct_info_is_gtype_struct (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        public StructInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
