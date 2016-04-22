// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using GISharp.Runtime;
using GISharp.GObject;

namespace GISharp.GI
{
    public class RegisteredTypeInfo : BaseInfo
    {
        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_registered_type_info_get_type_init (IntPtr raw);

        public string TypeInit {
            get {
                IntPtr raw_ret = g_registered_type_info_get_type_init (Handle);
                return MarshalG.Utf8PtrToString (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_registered_type_info_get_type_name (IntPtr raw);

        public string TypeName {
            get {
                IntPtr raw_ret = g_registered_type_info_get_type_name (Handle);
                return MarshalG.Utf8PtrToString (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern GType g_registered_type_info_get_g_type (IntPtr info);

        public GType GType {
            get {
                return g_registered_type_info_get_g_type (Handle);
            }
        }

        public RegisteredTypeInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
