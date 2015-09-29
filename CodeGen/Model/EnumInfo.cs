﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GISharp.CodeGen.Model
{
    public class EnumInfo : MemberInfo
    {
        List<EnumMemberInfo> _EnumMemberInfos;
        public IReadOnlyList<EnumMemberInfo> EnumMemberInfos {
            get {
                if (_EnumMemberInfos == null) {
                    _EnumMemberInfos = Element.Elements (gi + "member")
                        .Select (x => new EnumMemberInfo (x, this))
                        .ToList ();
                }
                return _EnumMemberInfos.AsReadOnly ();
            }
        }

        SeparatedSyntaxList<EnumMemberDeclarationSyntax> _EnumMembers;
        public SeparatedSyntaxList<EnumMemberDeclarationSyntax> EnumMembers {
            get {
                if (_EnumMembers == default(SeparatedSyntaxList<EnumMemberDeclarationSyntax>)) {
                    _EnumMembers = SyntaxFactory.SeparatedList<EnumMemberDeclarationSyntax> ()
                        .AddRange (EnumMemberInfos.Select (x => x.EnumMemberDeclaration));
                }
                return _EnumMembers;
            }
        }

        public EnumInfo (XElement element, MemberInfo declaringMember)
            : base (element, declaringMember)
        {
            if (element.Name != gi + "enumeration" && element.Name != gi + "bitfield") {
                throw new ArgumentException ("Requires <enumeration> or <bitfield> element.", nameof(element));
            }
        }

        protected override IEnumerable<AttributeListSyntax> GetAttributeLists ()
        {
            foreach (var baseAttr in base.GetAttributeLists ()) {
                yield return baseAttr;
            }

            if (Element.Name == gi + "bitfield") {
                var flagsAttrName = SyntaxFactory.ParseName (typeof(FlagsAttribute).FullName);
                var flagsAttr = SyntaxFactory.Attribute (flagsAttrName);
                yield return SyntaxFactory.AttributeList ().AddAttributes (flagsAttr);
            }

            var errorDomain = Element.Attribute (glib + "error-domain")?.Value;
            if (errorDomain != null) {
                var errorDomainAttrName = SyntaxFactory.ParseName (typeof(GISharp.Core.ErrorDomainAttribute).FullName);
                var errorDomainAttrArgListText = string.Format ("(\"{0}\")", errorDomain);
                var errorDomainAttrArgList = SyntaxFactory.ParseAttributeArgumentList (errorDomainAttrArgListText);
                var errorDomainAttr = SyntaxFactory.Attribute (errorDomainAttrName)
                    .WithArgumentList (errorDomainAttrArgList);
                yield return SyntaxFactory.AttributeList ().AddAttributes (errorDomainAttr);
            }
        }

        protected override IEnumerable<MemberDeclarationSyntax> GetDeclarations ()
        {
            var enumDeclaration = SyntaxFactory.EnumDeclaration (Identifier)
                .WithModifiers (Modifiers)
                .WithAttributeLists (AttributeLists)
                .WithMembers (EnumMembers)
                .WithLeadingTrivia (DocumentationCommentTriviaList);
            yield return enumDeclaration;
            // TODO: add class declaration for extension methods if there are functions/methods
        }
    }
}
