﻿using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace GISharp.GIRepository.Dynamic
{
    class EnumInfoDynamicMetaObject : DynamicMetaObject
    {
        readonly BindingRestrictions typeRestriction;

        EnumInfo Info { get { return (EnumInfo)Value; } }

        public EnumInfoDynamicMetaObject (Expression parameter, EnumInfo info)
            : base (parameter, BindingRestrictions.Empty, info)
        {
            typeRestriction = BindingRestrictions.GetTypeRestriction (Expression, typeof (EnumInfo));
        }

        public override System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames ()
        {
            return Info.Values.Keys.Concat (Info.Methods.Keys);
        }

        public override DynamicMetaObject BindGetMember (GetMemberBinder binder)
        {
            var value = Info.Values.SingleOrDefault (x => x.Name == binder.Name);
            if (value != null) {
                var expression = Expression.Constant (value);
                return new DynamicMetaObject (expression, typeRestriction);
            }
            return base.BindGetMember (binder);
        }
    }
}
