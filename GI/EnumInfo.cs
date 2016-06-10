// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using GISharp.GI.Dynamic;
using GISharp.Runtime;

namespace GISharp.GI
{
    public sealed class EnumInfo : RegisteredTypeInfo, IMethodContainer, IDynamicMetaObjectProvider
    {
        InfoDictionary<ValueInfo> values;

        public InfoDictionary<ValueInfo> Values {
            get {
                if (values == null) {
                    values = new InfoDictionary<ValueInfo> (NValues, GetValue);
                }
                return values;
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
        static extern IntPtr g_enum_info_get_error_domain (IntPtr raw);

        public string ErrorDomain {
            get {
                IntPtr raw_ret = g_enum_info_get_error_domain (Handle);
                return MarshalG.Utf8PtrToString (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_enum_info_get_method (IntPtr raw, int index);

        public FunctionInfo GetMethod (int index)
        {
            IntPtr raw_ret = g_enum_info_get_method (Handle, index);
            return MarshalPtr<FunctionInfo> (raw_ret);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_enum_info_get_n_methods (IntPtr raw);

        int NMethods {
            get {
                return g_enum_info_get_n_methods (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_enum_info_get_n_values (IntPtr raw);

        int NValues {
            get {
                return g_enum_info_get_n_values (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern TypeTag g_enum_info_get_storage_type (IntPtr raw);

        public TypeTag StorageType {
            get {
                return g_enum_info_get_storage_type (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_enum_info_get_value (IntPtr raw, int index);

        public ValueInfo GetValue (int index)
        {
            IntPtr raw_ret = g_enum_info_get_value (Handle, index);
            return MarshalPtr<ValueInfo> (raw_ret);
        }

        public DynamicMetaObject GetMetaObject (Expression parameter)
        {
            return new EnumInfoDynamicMetaObject (parameter, this);
        }

        public EnumInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
