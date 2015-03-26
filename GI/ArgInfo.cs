// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GI
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

#region Autogenerated code
    public partial class ArgInfo : GI.BaseInfo
    {

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_closure (IntPtr raw);

        public int Closure {
            get {
                int raw_ret = g_arg_info_get_closure (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_destroy (IntPtr raw);

        public int Destroy {
            get {
                int raw_ret = g_arg_info_get_destroy (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_direction (IntPtr raw);

        public GI.Direction Direction {
            get {
                int raw_ret = g_arg_info_get_direction (Handle);
                GI.Direction ret = (GI.Direction)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_ownership_transfer (IntPtr raw);

        public GI.Transfer OwnershipTransfer {
            get {
                int raw_ret = g_arg_info_get_ownership_transfer (Handle);
                GI.Transfer ret = (GI.Transfer)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_scope (IntPtr raw);

        public GI.ScopeType Scope {
            get {
                int raw_ret = g_arg_info_get_scope (Handle);
                GI.ScopeType ret = (GI.ScopeType)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_arg_info_get_type (IntPtr raw);

        public GI.TypeInfo TypeInfo {
            get {
                IntPtr raw_ret = g_arg_info_get_type (Handle);
                GI.TypeInfo ret = MarshalPtr<TypeInfo> (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_caller_allocates (IntPtr raw);

        public bool IsCallerAllocates {
            get {
                bool raw_ret = g_arg_info_is_caller_allocates (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_optional (IntPtr raw);

        public bool IsOptional {
            get {
                bool raw_ret = g_arg_info_is_optional (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_return_value (IntPtr raw);

        public bool IsReturnValue {
            get {
                bool raw_ret = g_arg_info_is_return_value (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_skip (IntPtr raw);

        public bool IsSkip {
            get {
                bool raw_ret = g_arg_info_is_skip (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_arg_info_load_type (IntPtr raw, IntPtr type);

        public void LoadType (GI.TypeInfo type)
        {
            g_arg_info_load_type (Handle, type == null ? IntPtr.Zero : type.Handle);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_may_be_null (IntPtr raw);

        public bool MayBeNull {
            get {
                bool raw_ret = g_arg_info_may_be_null (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        public ArgInfo (IntPtr raw) : base (raw)
        {
        }

#endregion
    }
}
