// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GISharp.Core;

namespace GISharp.GI
{
    public class EnumInfo : GISharp.GI.RegisteredTypeInfo, IMethodContainer
    {
        InfoCollection<ValueInfo> values;

        public InfoCollection<ValueInfo> Values {
            get {
                if (values == null) {
                    values = new InfoCollection<ValueInfo> (() => NValues, GetValue);
                }
                return values;
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
        static extern IntPtr g_enum_info_get_error_domain (IntPtr raw);

        public string ErrorDomain {
            get {
                IntPtr raw_ret = g_enum_info_get_error_domain (Handle);
                string ret = MarshalG.Utf8PtrToString (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_enum_info_get_method (IntPtr raw, int index);

        public GISharp.GI.FunctionInfo GetMethod (int index)
        {
            IntPtr raw_ret = g_enum_info_get_method (Handle, index);
            GISharp.GI.FunctionInfo ret = MarshalPtr<FunctionInfo> (raw_ret);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_enum_info_get_n_methods (IntPtr raw);

        protected int NMethods {
            get {
                int raw_ret = g_enum_info_get_n_methods (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_enum_info_get_n_values (IntPtr raw);

        protected int NValues {
            get {
                int raw_ret = g_enum_info_get_n_values (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_enum_info_get_storage_type (IntPtr raw);

        public GISharp.GI.TypeTag StorageType {
            get {
                int raw_ret = g_enum_info_get_storage_type (Handle);
                GISharp.GI.TypeTag ret = (GISharp.GI.TypeTag)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_enum_info_get_value (IntPtr raw, int index);

        public GISharp.GI.ValueInfo GetValue (int index)
        {
            IntPtr raw_ret = g_enum_info_get_value (Handle, index);
            GISharp.GI.ValueInfo ret = MarshalPtr<ValueInfo> (raw_ret);
            return ret;
        }

        public EnumInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
