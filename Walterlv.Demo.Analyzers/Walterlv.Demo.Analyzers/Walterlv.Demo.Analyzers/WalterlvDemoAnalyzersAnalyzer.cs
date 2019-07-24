using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Walterlv.Demo.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class WalterlvDemoAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public WalterlvDemoAnalyzersAnalyzer()
        {
            var a = DiagnosticId;
        }

        public const string DiagnosticId = "WAL001";

        private static readonly LocalizableString _title = "自动属性";
        private static readonly LocalizableString _messageFormat = "这是一个自动属性";
        private static readonly LocalizableString _description = "可以转换为可通知属性。";
        private const string _category = "Usage";

        private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor(
            DiagnosticId, _title, _messageFormat, _category, DiagnosticSeverity.Error,
            isEnabledByDefault: true, description: _description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxNodeAction(AnalyzeAutoProperty, SyntaxKind.PropertyDeclaration);

        private void AnalyzeAutoProperty(SyntaxNodeAnalysisContext context)
        {
            var propertyNode = (PropertyDeclarationSyntax)context.Node;
            var propertyName = propertyNode.Identifier.ValueText;
            var getAccessor = propertyNode.AccessorList.Accessors.FirstOrDefault(x =>
                x.Body == null && x.Keyword.ValueText is "get");
            var setAccessor = propertyNode.AccessorList.Accessors.FirstOrDefault(x =>
                x.Body == null && x.Keyword.ValueText is "set");

            var diagnostic = Diagnostic.Create(_rule, propertyNode.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
