﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GISharp.CodeGen.Model
{
    public class EnumMemberInfo : BaseInfo
    {
        EnumMemberDeclarationSyntax _EnumMemberDeclaration;
        public EnumMemberDeclarationSyntax EnumMemberDeclaration {
            get {
                if (_EnumMemberDeclaration == null) {
                    var valueText = Element.Attribute ("value").Value;
                    var literalValue = Literal (valueText[0] == '-'
                        ? int.Parse (valueText) : (int)uint.Parse (valueText));
                    var literalExpression = LiteralExpression (SyntaxKind.NumericLiteralExpression, literalValue);
                    var equalsValue = EqualsValueClause (literalExpression);
                    _EnumMemberDeclaration = EnumMemberDeclaration (ManagedName)
                        .WithEqualsValue (equalsValue)
                        .WithAttributeLists (AttributeLists)
                        .WithLeadingTrivia (DocumentationCommentTriviaList);
                }
                return _EnumMemberDeclaration;
            }
        }

        public EnumMemberInfo (XElement element, MemberInfo declaringMember)
            : base (element, declaringMember)
        {
            if (element.Name != gi + "member") {
                throw new ArgumentException ("Requires <member> element.", nameof(element));
            }
        }

        internal override IEnumerable<BaseInfo> GetChildInfos ()
        {
            yield break;
        }
    }
}
