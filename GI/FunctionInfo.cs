// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using GISharp.GI.Dynamic;
using GISharp.Runtime;

namespace GISharp.GI
{
    public sealed class FunctionInfo : CallableInfo, IDynamicMetaObjectProvider
    {
        static System.Reflection.MethodInfo invokeMethodInfo;

        static FunctionInfo ()
        {
            invokeMethodInfo = typeof (FunctionInfo).GetMethod (nameof (FunctionInfo.DynamicInvoke));
            if (invokeMethodInfo == null) {
                throw new MissingMethodException (
                    typeof (FunctionInfo).FullName,
                    nameof (FunctionInfo.Invoke));
            }
        }

        public bool IsConstructor {
            get {
                return Flags.HasFlag (FunctionInfoFlags.IsConstructor);
            }
        }

        public bool IsGetter {
            get {
                return Flags.HasFlag (FunctionInfoFlags.IsGetter);
            }
        }

        public bool IsSetter {
            get {
                return Flags.HasFlag (FunctionInfoFlags.IsSetter);
            }
        }

        public bool Throws {
            get {
                return Flags.HasFlag (FunctionInfoFlags.Throws);
            }
        }

        public bool WrapsVfunc {
            get {
                return Flags.HasFlag (FunctionInfoFlags.WrapsVfunc);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern FunctionInfoFlags g_function_info_get_flags (IntPtr raw);

        FunctionInfoFlags Flags {
            get {
                return g_function_info_get_flags (Handle);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_function_info_get_property (IntPtr raw);

        public PropertyInfo Property {
            get {
                // Sometimes g_function_info_get_property will incorrectly return
                // a non-null value when there is no property, so we check first.
                if (!IsGetter && !IsSetter) {
                    return null;
                }
                IntPtr raw_ret = g_function_info_get_property (Handle);
                return MarshalPtr<PropertyInfo> (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_function_info_get_symbol (IntPtr raw);

        public string Symbol {
            get {
                IntPtr raw_ret = g_function_info_get_symbol (Handle);
                return MarshalG.Utf8PtrToString (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_function_info_get_vfunc (IntPtr raw);

        public VFuncInfo VFunc {
            get {
                if (!WrapsVfunc) {
                    return null;
                }
                IntPtr raw_ret = g_function_info_get_vfunc (Handle);
                return MarshalPtr<VFuncInfo> (raw_ret);
            }
        }

        [DllImport ("libgirepository-1.0.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_function_info_invoke (
            IntPtr raw,
            Argument[] inArgs,
            int nInArgs,
            Argument[] outArgs,
            int nOutArgs,
            out Argument returnValue,
            out IntPtr error);

        public Argument Invoke (Argument[] inArgs, Argument[] outArgs)
        {
            int inArgsLength = inArgs?.Length ?? 0;
            int outArgsLength = outArgs?.Length ?? 0;
            Argument ret;
            IntPtr err;

            if (!g_function_info_invoke (Handle, inArgs, inArgsLength, outArgs, outArgsLength, out ret, out err)) {
                throw GErrorException.CreateInstance (err);
            }

            return ret;
        }

        public DynamicMetaObject GetMetaObject (Expression parameter)
        {
            return new FunctionInfoDynamicMetaObject (parameter, this);
        }

        static Argument MarshalInArg (TypeInfo typeInfo, DynamicMetaObject obj, ref Action free)
        {
            var arg = new Argument ();

            if (typeInfo.IsPointer) {
                switch (typeInfo.Tag) {
                case TypeTag.Interface:
                    var iface = typeInfo.Interface;
                    switch (iface.InfoType) {
                    case InfoType.Boxed:
                    case InfoType.Interface:
                    case InfoType.Object:
                    case InfoType.Struct:
                    case InfoType.Union:
                        dynamic dObj = obj.Value;
                        arg.Pointer = dObj.Handle;
                        break;
                    default:
                        throw new NotImplementedException ();
                    }
                    break;
                case TypeTag.UTF8:
                    var utf8Ptr = MarshalG.StringToUtf8Ptr ((string)obj.Value);
                    arg.Pointer = utf8Ptr;
                    free += () => MarshalG.Free (utf8Ptr);
                    break;
                case TypeTag.Array:
                    var elementType = typeInfo.GetParamType (0);
                    switch (typeInfo.ArrayType) {
                    case ArrayType.C:
                        if (elementType.Tag == TypeTag.UTF8) {
                            var strvPtr = MarshalG.StringArrayToGStrvPtr ((string[])obj.Value);
                            arg.Pointer = strvPtr;
                            free += () => MarshalG.FreeGStrv (strvPtr);
                        } else {
                            throw new NotImplementedException ();
                        }
                        break;
                    default:
                        throw new NotImplementedException ();
                    }
                    break;
                default:
                    throw new NotImplementedException ();
                }
            } else {
                switch (typeInfo.Tag) {
                case TypeTag.Boolean:
                    arg.Boolean = (bool)obj.Value;
                    break;
                case TypeTag.Int8:
                    arg.Int8 = (sbyte)obj.Value;
                    break;
                case TypeTag.UInt8:
                    arg.UInt8 = (byte)obj.Value;
                    break;
                case TypeTag.Int16:
                    arg.Int16 = (short)obj.Value;
                    break;
                case TypeTag.UInt16:
                    arg.UInt16 = (ushort)obj.Value;
                    break;
                case TypeTag.Int32:
                    arg.Int32 = (int)obj.Value;
                    break;
                case TypeTag.UInt32:
                    arg.UInt32 = (uint)obj.Value;
                    break;
                case TypeTag.Int64:
                    arg.Int64 = (long)obj.Value;
                    break;
                case TypeTag.UInt64:
                    arg.UInt64 = (ulong)obj.Value;
                    break;
                case TypeTag.Interface:
                    var iface = typeInfo.Interface;
                    switch (iface.InfoType) {
                    case InfoType.Enum:
                    case InfoType.Flags:
                        var valueInfo = (ValueInfo)obj.Value;
                        arg.Int64 = valueInfo.Value;
                        break;
                    default:
                        throw new NotImplementedException ();
                    }
                    break;
                default:
                    throw new NotImplementedException ();
                }
            }

            return arg;
        }

        static object MarshalOutArg (Argument arg, TypeInfo info, ref Action free)
        {
            if (info.IsPointer) {
                switch (info.Tag) {
                case TypeTag.Interface:
                    switch (info.Interface.InfoType) {
                    case InfoType.Object:
                        return new DynamicGObject (arg.Pointer);
                    default:
                        throw new NotImplementedException ();
                    }
                case TypeTag.Void:
                    return arg.Pointer;
                default:
                    throw new NotImplementedException ();
                }
            } else {
                switch (info.Tag) {
                case TypeTag.Boolean:
                    return arg.Boolean;
                case TypeTag.Int8:
                    return arg.Int8;
                case TypeTag.UInt8:
                    return arg.UInt8;
                case TypeTag.Int16:
                    return arg.Int16;
                case TypeTag.UInt16:
                    return arg.UInt16;
                case TypeTag.Int32:
                    return arg.Int32;
                case TypeTag.UInt32:
                    return arg.UInt32;
                case TypeTag.Int64:
                    return arg.Int64;
                case TypeTag.UInt64:
                    return arg.UInt64;
                case TypeTag.Void:
                    return null;
                default:
                    throw new NotImplementedException ();
                }
            }
        }

        public object DynamicInvoke (CallInfo callInfo, dynamic instance, params DynamicMetaObject[] args)
        {
            var methodOffset = IsMethod ? 1 : 0;
            if (instance == null && IsMethod) {
                throw new ArgumentNullException (nameof (instance), "Methods require instance");
            }
            if (instance != null && !IsMethod) {
                throw new ArgumentException ("Instance provided for static method", nameof (instance));
            }

            foreach (var name in callInfo.ArgumentNames) {
                if (!Args.ContainsKey (name)) {
                    throw new ArgumentException ("Invalid named parameter", name);
                }
            }
            var xp = Args.ToList ();
            foreach (var arg in Args) {
                if (arg.Closure != null) {
                    xp.Remove (arg.Closure);
                }
                if (arg.Destroy != null) {
                    xp.Remove (arg.Destroy);
                }
                if (arg.ArrayLength != null) {
                    xp.Remove (arg.ArrayLength);
                }
            }
            if (xp.Count != args.Length) {
                var names = string.Join (", ", xp.Select (x => x.Name));
                var message = $"Bad arg count - expecting {xp.Count}: {names}";
                throw new ArgumentException (message);
            }
            foreach (var name in callInfo.ArgumentNames) {
                var arg = xp.Find (x => x.Name == name);
                if (arg != null) {
                    xp.Remove (arg);
                    xp.Add (arg);
                }
            }

            var inArgs = new Argument[InArgs.Count + methodOffset];
            var outArgs = new Argument[OutArgs.Count];
            var freeInArgs = default (Action);
            var freeOutArgs = default (Action);

            if (IsMethod) {
                inArgs[0].Pointer = instance.Handle;
            }
            foreach (var arg in xp) {
                if (arg.InIndex >= 0) {
                    inArgs[arg.InIndex + methodOffset] = MarshalInArg (arg.TypeInfo, args[xp.IndexOf (arg)], ref freeInArgs);
                }
            }

            try {
                var retArg = Invoke (inArgs, outArgs);

                // TODO: marshal out args

                if (SkipReturn) {
                    return default (object);
                }

                var ret = MarshalOutArg (retArg, ReturnTypeInfo, ref freeOutArgs);

                return ret;
            } finally {
                if (freeInArgs != null) {
                    freeInArgs ();
                }
                if (freeOutArgs != null) {
                    freeOutArgs ();
                }
            }
        }

        public Expression GetInvokeExpression (CallInfo callInfo, Type returnType, dynamic instance, DynamicMetaObject[] args)
        {
            var expression = Expression.Call (Expression.Constant (this),
                                              invokeMethodInfo,
                                              Expression.Constant (callInfo),
                                              Expression.Constant (instance),
                                              Expression.Constant (args));

            return expression;
        }

        public FunctionInfo (IntPtr raw) : base (raw)
        {
        }
    }
}
