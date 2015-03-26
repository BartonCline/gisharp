// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GI
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

#region Autogenerated code
    public partial class CallableInfo : GI.BaseInfo
    {

        InfoCollection<ArgInfo> args;

        public InfoCollection<ArgInfo> Args {
            get {
                if (args == null) {
                    args = new InfoCollection<ArgInfo> (() => NArgs, GetArg);
                }
                return args;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_callable_info_can_throw_gerror (IntPtr raw);

        public bool CanThrowGError {
            get {
                bool raw_ret = g_callable_info_can_throw_gerror (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_callable_info_get_arg (IntPtr raw, int index);

        protected GI.ArgInfo GetArg (int index)
        {
            IntPtr raw_ret = g_callable_info_get_arg (Handle, index);
            GI.ArgInfo ret = MarshalPtr<ArgInfo> (raw_ret);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_callable_info_get_caller_owns (IntPtr raw);

        public GI.Transfer CallerOwns {
            get {
                int raw_ret = g_callable_info_get_caller_owns (Handle);
                GI.Transfer ret = (GI.Transfer)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_callable_info_get_instance_ownership_transfer (IntPtr raw);

        public GI.Transfer InstanceOwnershipTransfer {
            get {
                int raw_ret = g_callable_info_get_instance_ownership_transfer (Handle);
                GI.Transfer ret = (GI.Transfer)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_callable_info_get_n_args (IntPtr raw);

        protected int NArgs {
            get {
                int raw_ret = g_callable_info_get_n_args (Handle);
                int ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_callable_info_get_return_attribute (IntPtr raw, IntPtr name);

        public string GetReturnAttribute (string name)
        {
            IntPtr native_name = GLib.Marshaller.StringToPtrGStrdup (name);
            IntPtr raw_ret = g_callable_info_get_return_attribute (Handle, native_name);
            string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
            GLib.Marshaller.Free (native_name);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_callable_info_get_return_type (IntPtr raw);

        public GI.TypeInfo ReturnTypeInfo {
            get {
                IntPtr raw_ret = g_callable_info_get_return_type (Handle);
                GI.TypeInfo ret = MarshalPtr<TypeInfo> (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern unsafe bool g_callable_info_invoke (IntPtr raw, IntPtr function, GI.Argument[] in_args, int n_in_args, GI.Argument[] out_args, int n_out_args, IntPtr return_value, bool is_method, bool throws, out IntPtr error);

        public unsafe bool Invoke (IntPtr function, GI.Argument[] in_args, GI.Argument[] out_args, out GI.Argument return_value, bool is_method, bool throws)
        {
            IntPtr native_return_value = Marshal.AllocHGlobal (Marshal.SizeOf (typeof(GI.Argument)));
            IntPtr error = IntPtr.Zero;
            bool raw_ret = g_callable_info_invoke (Handle, function, in_args, (in_args == null ? 0 : in_args.Length), out_args, (out_args == null ? 0 : out_args.Length), native_return_value, is_method, throws, out error);
            bool ret = raw_ret;
            return_value = GI.Argument.New (native_return_value);
            Marshal.FreeHGlobal (native_return_value);
            if (error != IntPtr.Zero)
                throw new GLib.GException (error);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_callable_info_is_method (IntPtr raw);

        public bool IsMethod {
            get {
                bool raw_ret = g_callable_info_is_method (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_callable_info_iterate_return_attributes (IntPtr raw, IntPtr iterator, IntPtr name, IntPtr value);

        public bool IterateReturnAttributes (GI.AttributeIter iterator, string name, string value)
        {
            IntPtr native_iterator = GLib.Marshaller.StructureToPtrAlloc (iterator);
            bool raw_ret = g_callable_info_iterate_return_attributes (Handle, native_iterator, GLib.Marshaller.StringToPtrGStrdup (name), GLib.Marshaller.StringToPtrGStrdup (value));
            bool ret = raw_ret;
            iterator = GI.AttributeIter.New (native_iterator);
            Marshal.FreeHGlobal (native_iterator);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_callable_info_load_arg (IntPtr raw, int n, IntPtr arg);

        public void LoadArg (int n, GI.ArgInfo arg)
        {
            g_callable_info_load_arg (Handle, n, arg == null ? IntPtr.Zero : arg.Handle);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_callable_info_load_return_type (IntPtr raw, IntPtr type);

        public void LoadReturnType (GI.TypeInfo type)
        {
            g_callable_info_load_return_type (Handle, type == null ? IntPtr.Zero : type.Handle);
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_callable_info_may_return_null (IntPtr raw);

        public bool MayReturnNull {
            get {
                bool raw_ret = g_callable_info_may_return_null (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_callable_info_skip_return (IntPtr raw);

        public bool SkipReturn {
            get {
                bool raw_ret = g_callable_info_skip_return (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        public CallableInfo (IntPtr raw) : base (raw)
        {
        }

#endregion
    }
}
