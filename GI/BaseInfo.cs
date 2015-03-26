// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

namespace GI
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

#region Autogenerated code
    public partial class BaseInfo : GLib.Opaque, IEquatable<GI.BaseInfo>
    {

        internal static BaseInfo MarshalPtr (IntPtr raw)
        {
            if (raw == IntPtr.Zero)
                return null;
            Type type = typeof(BaseInfo);
            switch ((InfoType)g_base_info_get_type (raw)) {
            case InfoType.Arg:
                type = typeof(ArgInfo);
                break;
            case InfoType.Boxed:
                // TODO: could be struct or union
                type = typeof(StructInfo);
                break;
            case InfoType.Callback:
                type = typeof(CallbackInfo);
                break;
            case InfoType.Constant:
                type = typeof(ConstantInfo);
                break;
            case InfoType.Enum:
            case InfoType.Flags:
                type = typeof(EnumInfo);
                break;
            case InfoType.Field:
                type = typeof(FieldInfo);
                break;
            case InfoType.Function:
                type = typeof(FunctionInfo);
                break;
            case InfoType.Interface:
                type = typeof(InterfaceInfo);
                break;
            case InfoType.Object:
                type = typeof(ObjectInfo);
                break;
            case InfoType.Property:
                type = typeof(PropertyInfo);
                break;
            case InfoType.Signal:
                type = typeof(SignalInfo);
                break;
            case InfoType.Struct:
                type = typeof(StructInfo);
                break;
            case InfoType.Type:
                type = typeof(TypeInfo);
                break;
            case InfoType.Union:
                type = typeof(UnionInfo);
                break;
            case InfoType.Unresolved:
                type = typeof(UnresolvedInfo);
                break;
            case InfoType.Value:
                type = typeof(ValueInfo);
                break;
            case InfoType.VFunc:
                type = typeof(VFuncInfo);
                break;
            }
            return (GI.BaseInfo)GLib.Opaque.GetOpaque (raw, type, false);
        }

        public string Name {
            get {
                // calling g_base_info_get_name on a TypeInfo will cause a crash.
                var typeInfo = this as TypeInfo;
                if (typeInfo != null) {
                    if (typeInfo.Tag == TypeTag.Interface) {
                        return typeInfo.Interface.Name;
                    }
                    if (typeInfo.Tag == TypeTag.Array) {
                        return typeInfo.ArrayType.ToString ();
                    }
                    if (typeInfo.Tag == TypeTag.Error) {
                        return "Error";
                    }
                    return null;
                }
                return NameInternal;
            }
        }

        public override bool Equals (object o)
        {
            var baseInfo = o as BaseInfo;
            if (baseInfo != null) {
                return Equals (baseInfo);
            }
            return base.Equals (o);
        }

        public override int GetHashCode ()
        {
            return Handle.ToInt32 ();
        }

        public static bool operator == (BaseInfo info1, BaseInfo info2)
        {
            if ((object)info1 == null) {
                return (object)info2 == null;
            }
            if ((object)info2 == null) {
                return false;
            }
            return info1.Equals (info2);
        }

        public static bool operator != (BaseInfo info1, BaseInfo info2)
        {
            return !(info1 == info2);
        }

        public IEnumerable<KeyValuePair<string, string>> Attributes {
            get {
                AttributeIter iter = AttributeIter.Zero;
                string key, value;
                while (IterateAttributes (ref iter, out key, out value)) {
                    yield return new KeyValuePair<string, string> (key, value);
                }
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_base_info_equal (IntPtr raw, IntPtr info2);

        public bool Equals (GI.BaseInfo info2)
        {
            bool raw_ret = g_base_info_equal (Handle, info2.Handle);
            bool ret = raw_ret;
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_get_attribute (IntPtr raw, IntPtr name);

        public string GetAttribute (string name)
        {
            IntPtr native_name = GLib.Marshaller.StringToPtrGStrdup (name);
            IntPtr raw_ret = g_base_info_get_attribute (Handle, native_name);
            string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
            GLib.Marshaller.Free (native_name);
            return ret;
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_get_container (IntPtr raw);

        public GI.BaseInfo Container {
            get {
                IntPtr raw_ret = g_base_info_get_container (Handle);
                GI.BaseInfo ret = GI.BaseInfo.MarshalPtr (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_get_name (IntPtr raw);

        protected string NameInternal {
            get {
                IntPtr raw_ret = g_base_info_get_name (Handle);
                string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_get_namespace (IntPtr raw);

        public string Namespace {
            get {
                IntPtr raw_ret = g_base_info_get_namespace (Handle);
                string ret = GLib.Marshaller.Utf8PtrToString (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_base_info_get_type (IntPtr raw);

        public GI.InfoType InfoType {
            get {
                int raw_ret = g_base_info_get_type (Handle);
                GI.InfoType ret = (GI.InfoType)raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_get_typelib (IntPtr raw);

        public GI.Typelib Typelib {
            get {
                IntPtr raw_ret = g_base_info_get_typelib (Handle);
                GI.Typelib ret = raw_ret == IntPtr.Zero ? null : (GI.Typelib)GLib.Opaque.GetOpaque (raw_ret, typeof(GI.Typelib), false);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_base_info_is_deprecated (IntPtr raw);

        public bool IsDeprecated {
            get {
                bool raw_ret = g_base_info_is_deprecated (Handle);
                bool ret = raw_ret;
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_base_info_iterate_attributes (IntPtr raw, IntPtr iterator, out IntPtr name, out IntPtr value);

        protected bool IterateAttributes (ref GI.AttributeIter iterator, out string name, out string value)
        {
            IntPtr native_iterator = GLib.Marshaller.StructureToPtrAlloc (iterator);
            IntPtr native_name;
            IntPtr native_value;
            bool raw_ret = g_base_info_iterate_attributes (Handle, native_iterator, out native_name, out native_value);
            bool ret = raw_ret;
            iterator = GI.AttributeIter.New (native_iterator);
            Marshal.FreeHGlobal (native_iterator);
            name = GLib.Marshaller.PtrToStringGFree (native_name);
            value = GLib.Marshaller.PtrToStringGFree (native_value);
            return ret;
        }

        public BaseInfo (IntPtr raw) : base (raw)
        {
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_base_info_ref (IntPtr raw);

        protected override void Ref (IntPtr raw)
        {
            if (!Owned) {
                g_base_info_ref (raw);
                Owned = true;
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_base_info_unref (IntPtr raw);

        protected override void Unref (IntPtr raw)
        {
            if (Owned) {
                g_base_info_unref (raw);
                Owned = false;
            }
        }

        class FinalizerInfo
        {
            IntPtr handle;

            public FinalizerInfo (IntPtr handle)
            {
                this.handle = handle;
            }

            public bool Handler ()
            {
                g_base_info_unref (handle);
                return false;
            }
        }

        ~BaseInfo ()
        {
            if (!Owned)
                return;
            FinalizerInfo info = new FinalizerInfo (Handle);
            GLib.Timeout.Add (50, new GLib.TimeoutHandler (info.Handler));
        }

#endregion
    }
}
