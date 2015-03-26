// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GI
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

#region Autogenerated code
    public partial class FieldInfo : GI.BaseInfo
    {

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_field_info_get_field (IntPtr raw, IntPtr mem, IntPtr value);

        public bool GetField (IntPtr mem, out GI.Argument value)
        {
            IntPtr native_value = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(GI.Argument)));
            bool raw_ret = g_field_info_get_field (Handle, mem, native_value);
            bool ret = raw_ret;
            value = GI.Argument.New (native_value);
            Marshal.FreeHGlobal (native_value);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_field_info_get_flags (IntPtr raw);

        public GI.FieldInfoFlags Flags {
            get {
                int raw_ret = g_field_info_get_flags (Handle);
                GI.FieldInfoFlags ret = (GI.FieldInfoFlags)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_field_info_get_offset (IntPtr raw);

        public int Offset {
            get {
                int raw_ret = g_field_info_get_offset (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_field_info_get_size (IntPtr raw);

        public int Size {
            get {
                int raw_ret = g_field_info_get_size (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_field_info_get_type (IntPtr raw);

        public GI.TypeInfo TypeInfo {
            get {
                IntPtr raw_ret = g_field_info_get_type (Handle);
                GI.TypeInfo ret = MarshalPtr<TypeInfo> (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_field_info_set_field (IntPtr raw, IntPtr mem, IntPtr value);

        public bool SetField (IntPtr mem, GI.Argument value)
        {
            IntPtr native_value = GLib.Marshaller.StructureToPtrAlloc (value);
            bool raw_ret = g_field_info_set_field (Handle, mem, native_value);
            bool ret = raw_ret;
            value = GI.Argument.New (native_value);
            Marshal.FreeHGlobal (native_value);
            return ret;
        }

        public FieldInfo (IntPtr raw) : base (raw)
        {
        }

#endregion
    }
}
