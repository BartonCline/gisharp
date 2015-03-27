// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GISharp.GI
{
    public class ArgInfo : BaseInfo
    {
        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_closure (IntPtr raw);

        public int Closure {
            get {
                return g_arg_info_get_closure (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_arg_info_get_destroy (IntPtr raw);

        public int Destroy {
            get {
                return g_arg_info_get_destroy (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern Direction g_arg_info_get_direction (IntPtr raw);

        public Direction Direction {
            get {
                return g_arg_info_get_direction (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern Transfer g_arg_info_get_ownership_transfer (IntPtr raw);

        public Transfer OwnershipTransfer {
            get {
                return g_arg_info_get_ownership_transfer (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern ScopeType g_arg_info_get_scope (IntPtr raw);

        public ScopeType Scope {
            get {
                return g_arg_info_get_scope (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_arg_info_get_type (IntPtr raw);

        public TypeInfo TypeInfo {
            get {
                IntPtr raw_ret = g_arg_info_get_type (Handle);
                return MarshalPtr<TypeInfo> (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_caller_allocates (IntPtr raw);

        public bool IsCallerAllocates {
            get {
                return g_arg_info_is_caller_allocates (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_optional (IntPtr raw);

        public bool IsOptional {
            get {
                return g_arg_info_is_optional (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_return_value (IntPtr raw);

        public bool IsReturnValue {
            get {
                return g_arg_info_is_return_value (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_is_skip (IntPtr raw);

        public bool IsSkip {
            get {
                return g_arg_info_is_skip (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_arg_info_load_type (IntPtr raw, IntPtr type);

        public void LoadType (TypeInfo type)
        {
            g_arg_info_load_type (Handle, type == null ? IntPtr.Zero : type.Handle);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_arg_info_may_be_null (IntPtr raw);

        public bool MayBeNull {
            get {
                return g_arg_info_may_be_null (Handle);
            }
        }

        public ArgInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
