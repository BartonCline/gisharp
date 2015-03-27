// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GI
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

#region Autogenerated code
    public partial class StructInfo : GI.RegisteredTypeInfo, IMethodContainer
    {

        InfoCollection<FieldInfo> fields;

        public InfoCollection<FieldInfo> Fields {
            get {
                if (fields == null) {
                    fields = new InfoCollection<FieldInfo> (() => NFields, GetField);
                }
                return fields;
            }
        }

        InfoCollection<FunctionInfo> methods;

        public InfoCollection<FunctionInfo> Methods {
            get {
                if (methods == null) {
                    methods = new InfoCollection<FunctionInfo> (() => NMethods, GetMethod);
                }
                return methods;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_find_method (IntPtr raw, IntPtr name);

        public GI.FunctionInfo FindMethod (string name)
        {
            IntPtr native_name = MarshalG.StringToUtf8Ptr (name);
            IntPtr raw_ret = g_struct_info_find_method (Handle, native_name);
            GI.FunctionInfo ret = MarshalPtr<FunctionInfo> (raw_ret);
            MarshalG.Free (native_name);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern UIntPtr g_struct_info_get_alignment (IntPtr raw);

        public ulong Alignment {
            get {
                UIntPtr raw_ret = g_struct_info_get_alignment (Handle);
                ulong ret = (ulong)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_get_field (IntPtr raw, int index);

        public GI.FieldInfo GetField (int index)
        {
            IntPtr raw_ret = g_struct_info_get_field (Handle, index);
            GI.FieldInfo ret = MarshalPtr<FieldInfo> (raw_ret);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_struct_info_get_method (IntPtr raw, int index);

        public GI.FunctionInfo GetMethod (int index)
        {
            IntPtr raw_ret = g_struct_info_get_method (Handle, index);
            GI.FunctionInfo ret = MarshalPtr<FunctionInfo> (raw_ret);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_struct_info_get_n_fields (IntPtr raw);

        protected int NFields {
            get {
                int raw_ret = g_struct_info_get_n_fields (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_struct_info_get_n_methods (IntPtr raw);

        protected int NMethods {
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
                ulong ret = (ulong)raw_ret;
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

        public bool IsGtypeStruct {
            get {
                bool raw_ret = g_struct_info_is_gtype_struct (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        public StructInfo (IntPtr raw) : base (raw)
        {
        }

#endregion
    }
}
