// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GISharp
{

    using System;
    using System.Runtime.InteropServices;

#region Autogenerated code
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
	internal delegate IntPtr ObjectInfoRefFunctionNative (IntPtr objekt);

    internal class ObjectInfoRefFunctionInvoker
    {

        ObjectInfoRefFunctionNative native_cb;
        IntPtr __data;
        GLib.DestroyNotify __notify;

        ~ObjectInfoRefFunctionInvoker ()
        {
            if (__notify == null)
                return;
            __notify (__data);
        }

        internal ObjectInfoRefFunctionInvoker (ObjectInfoRefFunctionNative native_cb) : this (native_cb, IntPtr.Zero, null)
        {
        }

        internal ObjectInfoRefFunctionInvoker (ObjectInfoRefFunctionNative native_cb, IntPtr data) : this (native_cb, data, null)
        {
        }

        internal ObjectInfoRefFunctionInvoker (ObjectInfoRefFunctionNative native_cb, IntPtr data, GLib.DestroyNotify notify)
        {
            this.native_cb = native_cb;
            __data = data;
            __notify = notify;
        }

        internal GI.ObjectInfoRefFunction Handler {
            get {
                return new GI.ObjectInfoRefFunction (InvokeNative);
            }
        }

        IntPtr InvokeNative (IntPtr objekt)
        {
            IntPtr __result = native_cb (objekt);
            return __result;
        }
    }

    internal class ObjectInfoRefFunctionWrapper
    {

        public IntPtr NativeCallback (IntPtr objekt)
        {
            try {
                IntPtr __ret = managed (objekt);
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

        internal ObjectInfoRefFunctionNative NativeDelegate;
        GI.ObjectInfoRefFunction managed;

        public ObjectInfoRefFunctionWrapper (GI.ObjectInfoRefFunction managed)
        {
            this.managed = managed;
            if (managed != null)
                NativeDelegate = new ObjectInfoRefFunctionNative (NativeCallback);
        }

        public static GI.ObjectInfoRefFunction GetManagedDelegate (ObjectInfoRefFunctionNative native)
        {
            if (native == null)
                return null;
            ObjectInfoRefFunctionWrapper wrapper = (ObjectInfoRefFunctionWrapper)native.Target;
            if (wrapper == null)
                return null;
            return wrapper.managed;
        }
    }
#endregion
}
