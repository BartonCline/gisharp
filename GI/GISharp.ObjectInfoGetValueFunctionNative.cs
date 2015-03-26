// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GISharp
{

    using System;
    using System.Runtime.InteropServices;

#region Autogenerated code
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    internal delegate IntPtr ObjectInfoGetValueFunctionNative (IntPtr value);

    internal class ObjectInfoGetValueFunctionInvoker
    {

        ObjectInfoGetValueFunctionNative native_cb;
        IntPtr __data;
        GLib.DestroyNotify __notify;

        ~ObjectInfoGetValueFunctionInvoker ()
        {
            if (__notify == null)
                return;
            __notify (__data);
        }

        internal ObjectInfoGetValueFunctionInvoker (ObjectInfoGetValueFunctionNative native_cb) : this (native_cb, IntPtr.Zero, null)
        {
        }

        internal ObjectInfoGetValueFunctionInvoker (ObjectInfoGetValueFunctionNative native_cb, IntPtr data) : this (native_cb, data, null)
        {
        }

        internal ObjectInfoGetValueFunctionInvoker (ObjectInfoGetValueFunctionNative native_cb, IntPtr data, GLib.DestroyNotify notify)
        {
            this.native_cb = native_cb;
            __data = data;
            __notify = notify;
        }

        internal GI.ObjectInfoGetValueFunction Handler {
            get {
                return new GI.ObjectInfoGetValueFunction (InvokeNative);
            }
        }

        IntPtr InvokeNative (GLib.Value value)
        {
            IntPtr native_value = GLib.Marshaller.StructureToPtrAlloc (value);
            IntPtr __result = native_cb (native_value);
            value = (GLib.Value)Marshal.PtrToStructure (native_value, typeof(GLib.Value));
            Marshal.FreeHGlobal (native_value);
            return __result;
        }
    }

    internal class ObjectInfoGetValueFunctionWrapper
    {

        public IntPtr NativeCallback (IntPtr value)
        {
            try {
                IntPtr __ret = managed ((GLib.Value)Marshal.PtrToStructure (value, typeof(GLib.Value)));
                if (release_on_call)
                    gch.Free ();
                return __ret;
            } catch (Exception e) {
                GLib.ExceptionManager.RaiseUnhandledException (e, true);
                // NOTREACHED: Above call does not return.
                throw e;
            }
        }

        bool release_on_call = false;
        GCHandle gch;

        public void PersistUntilCalled ()
        {
            release_on_call = true;
            gch = GCHandle.Alloc (this);
        }

        internal ObjectInfoGetValueFunctionNative NativeDelegate;
        GI.ObjectInfoGetValueFunction managed;

        public ObjectInfoGetValueFunctionWrapper (GI.ObjectInfoGetValueFunction managed)
        {
            this.managed = managed;
            if (managed != null)
                NativeDelegate = new ObjectInfoGetValueFunctionNative (NativeCallback);
        }

        public static GI.ObjectInfoGetValueFunction GetManagedDelegate (ObjectInfoGetValueFunctionNative native)
        {
            if (native == null)
                return null;
            ObjectInfoGetValueFunctionWrapper wrapper = (ObjectInfoGetValueFunctionWrapper)native.Target;
            if (wrapper == null)
                return null;
            return wrapper.managed;
        }
    }
#endregion
}
