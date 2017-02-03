using System;
using System.Runtime.InteropServices;
using GISharp.Runtime;

namespace GISharp.GObject
{
    /// <summary>
    /// A <see cref="ParamSpec"/> derived structure that contains the meta data for object properties.
    /// </summary>
    [GType ("GParamObject", IsWrappedNativeType = true)]
    public sealed class ParamSpecObject : ParamSpec
    {
        public sealed class SafeParamSpecObjectHandle : SafeParamSpecHandle
        {
            struct ParamSpecObject
            {
                #pragma warning disable CS0649
                public ParamSpecStruct ParentInstance;
                #pragma warning restore CS0649
            }

            public SafeParamSpecObjectHandle (IntPtr handle, Transfer ownership)
                : base (handle, ownership)
            {
            }
        }

        public new SafeParamSpecObjectHandle Handle {
            get {
                return (SafeParamSpecObjectHandle)base.Handle;
            }
        }

        static GType getGType ()
        {
            return paramSpecTypes[19];
        }

        public ParamSpecObject (SafeParamSpecObjectHandle handle) : base (handle)
        {
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        extern static IntPtr g_param_spec_object (
            IntPtr name,
            IntPtr nick,
            IntPtr blurb,
            GType objectType,
            ParamFlags flags);

        static SafeParamSpecObjectHandle New (string name, string nick, string blurb, GType objectType, ParamFlags flags)
        {
            if (name == null) {
                throw new ArgumentNullException (nameof (name));
            }
            if (nick == null) {
                throw new ArgumentNullException (nameof (nick));
            }
            if (blurb == null) {
                throw new ArgumentNullException (nameof (blurb));
            }
            if (!objectType.IsA (GType.Object)) {
                throw new ArgumentException ("Expecting object type.", nameof (objectType));
            }
            var namePtr = GMarshal.StringToUtf8Ptr (name);
            var nickPtr = GMarshal.StringToUtf8Ptr (nick);
            var blurbPtr = GMarshal.StringToUtf8Ptr (blurb);
            var ret_ = g_param_spec_object (namePtr, nickPtr, blurbPtr, objectType, flags);
            var ret = new SafeParamSpecObjectHandle (ret_, Transfer.None);

            // Any strings that have the cooresponding static flag set must not
            // be freed because they are passed to g_intern_static_string().
            if (!flags.HasFlag (ParamFlags.StaticName)) {
                GMarshal.Free (namePtr);
            }
            if (!flags.HasFlag (ParamFlags.StaticNick)) {
                GMarshal.Free (nickPtr);
            }
            if (!flags.HasFlag (ParamFlags.StaticBlurb)) {
                GMarshal.Free (blurbPtr);
            }

            return ret;
        }

        public ParamSpecObject (string name, string nick, string blurb, GType objectType, ParamFlags flags)
            : this (New (name, nick, blurb, objectType, flags))
        {
        }
    }
}
