// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Runtime.InteropServices;

using GISharp.Runtime;

namespace GISharp.GI
{
    /// <summary>
    /// Stores an argument of varying type.
    /// </summary>
    [StructLayout (LayoutKind.Explicit)]
    public struct Argument : IEquatable<Argument>
    {
        [FieldOffset (0)]
        public bool Boolean;
        [FieldOffset (0)]
        public sbyte Int8;
        [FieldOffset (0)]
        public byte UInt8;
        [FieldOffset (0)]
        public short Int16;
        [FieldOffset (0)]
        public ushort UInt16;
        [FieldOffset (0)]
        public int Int32;
        [FieldOffset (0)]
        public uint UInt32;
        [FieldOffset (0)]
        public long Int64;
        [FieldOffset (0)]
        public ulong UInt64;
        [FieldOffset (0)]
        public float Float;
        [FieldOffset (0)]
        public double Double;
        [FieldOffset (0)]
        public short Short;
        [FieldOffset (0)]
        public int Int;
        [FieldOffset (0)]
        public uint UInt;
        [FieldOffset (0)]
        private IntPtr v_long;

        public long Long {
            get {
                return (long)v_long;
            }
            set {
                v_long = new IntPtr (value);
            }
        }

        [FieldOffset (0)]
        private UIntPtr v_ulong;

        public ulong ULong {
            get {
                return (ulong)v_ulong;
            }
            set {
                v_ulong = new UIntPtr (value);
            }
        }

        [FieldOffset (0)]
        private IntPtr v_ssize;

        public long SSize {
            get {
                return (long)v_ssize;
            }
            set {
                v_ssize = new IntPtr (value);
            }
        }

        [FieldOffset (0)]
        private UIntPtr v_size;

        public ulong Size {
            get {
                return (ulong)v_size;
            }
            set {
                v_size = new UIntPtr (value);
            }
        }

        [FieldOffset (0)]
        private IntPtr _v_string;

        public string String {
            get {
                return MarshalG.Utf8PtrToString (_v_string);
            }
            set {
                var oldString = _v_string;
                _v_string = MarshalG.StringToUtf8Ptr (value);
                MarshalG.Free (oldString);
            }
        }

        [FieldOffset (0)]
        private IntPtr _v_pointer;

        public IntPtr Pointer {
            get { return _v_pointer; }
            set { _v_pointer = value; }
        }

        public static GISharp.GI.Argument Zero = new GISharp.GI.Argument ();

        public static GISharp.GI.Argument New (IntPtr raw)
        {
            if (raw == IntPtr.Zero)
                return GISharp.GI.Argument.Zero;
            return (GISharp.GI.Argument)Marshal.PtrToStructure (raw, typeof(GISharp.GI.Argument));
        }

        public bool Equals (Argument other)
        {
            return _v_pointer.Equals (other._v_pointer);
        }

        public override bool Equals (object other)
        {
            return other is Argument && Equals ((Argument)other);
        }

        public override int GetHashCode ()
        {
            return _v_pointer.GetHashCode ();
        }
    }
}
