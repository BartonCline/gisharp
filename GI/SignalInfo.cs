// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Runtime.InteropServices;
using GISharp.GObject;

namespace GISharp.GI
{
    public sealed class SignalInfo : CallableInfo
    {
        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_signal_info_get_class_closure (IntPtr raw);

        public VFuncInfo ClassClosure {
            get {
                IntPtr raw_ret = g_signal_info_get_class_closure (Handle);
                return MarshalPtr<VFuncInfo> (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern SignalFlags g_signal_info_get_flags (IntPtr raw);

        public SignalFlags Flags {
            get {
                return g_signal_info_get_flags (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_signal_info_true_stops_emit (IntPtr raw);

        public bool TrueStopsEmit {
            get {
                return g_signal_info_true_stops_emit (Handle);
            }
        }

        public SignalInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
