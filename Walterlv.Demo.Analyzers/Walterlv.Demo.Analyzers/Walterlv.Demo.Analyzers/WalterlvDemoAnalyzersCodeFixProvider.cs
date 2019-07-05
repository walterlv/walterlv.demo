using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Walterlv.Demo.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(WalterlvDemoAnalyzersCodeFixProvider)), Shared]
    public class WalterlvDemoAnalyzersCodeFixProvider : CodeFixProvider
    {
        private const string _title = "转换为可通知属性";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(WalterlvDemoAnalyzersAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var declaration = (PropertyDeclarationSyntax)root.FindNode(diagnostic.Location.SourceSpan);

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: _title,
                    createChangedSolution: ct => ConvertToNotificationProperty(context.Document, declaration, ct),
                    equivalenceKey: _title),
                diagnostic);
        }

        private async Task<Solution> ConvertToNotificationProperty(Document document,
            PropertyDeclarationSyntax propertyDeclarationSyntax, CancellationToken cancellationToken)
        {
            // 获取文档根语法节点。
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            // 生成可通知属性的语法节点集合。
            var type = propertyDeclarationSyntax.Type;
            var propertyName = propertyDeclarationSyntax.Identifier.ValueText;
            var fieldName = $"_{char.ToLower(propertyName[0])}{propertyName.Substring(1)}";
            var newNodes = CreateNotificationProperty(type, propertyName, fieldName);

            // 将可通知属性的语法节点插入到原文档中形成一份中间文档。
            var intermediateRoot = root
                .InsertNodesAfter(
                    root.FindNode(propertyDeclarationSyntax.Span),
                    newNodes);

            // 将中间文档中的自动属性移除形成一份最终文档。
            var newRoot = intermediateRoot
                .RemoveNode(intermediateRoot.FindNode(propertyDeclarationSyntax.Span), SyntaxRemoveOptions.KeepNoTrivia);

            // 将原来解决方案中的此份文档换成新文档以形成新的解决方案。
            return document.Project.Solution.WithDocumentSyntaxRoot(document.Id, newRoot);
        }

        private SyntaxNode[] CreateNotificationProperty(TypeSyntax type, string propertyName, string fieldName)
            => new SyntaxNode[]
            {
                SyntaxFactory.FieldDeclaration(
                    new SyntaxList<AttributeListSyntax>(),
                    new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)),
                    SyntaxFactory.VariableDeclaration(
                        type,
                        SyntaxFactory.SeparatedList(new []
                        {
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(fieldName)
                            )
                        })
                    ),
                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                ),
                SyntaxFactory.PropertyDeclaration(
                    type,
                    SyntaxFactory.Identifier(propertyName)
                )
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration
                    )
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken),
                            SyntaxFactory.IdentifierName(fieldName)
                        )
                    )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.SetAccessorDeclaration
                    )
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.IdentifierName("SetValue"),
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.Token(SyntaxKind.OpenParenToken),
                                    SyntaxFactory.SeparatedList(new []
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(fieldName)
                                        )
                                        .WithRefKindKeyword(
                                            SyntaxFactory.Token(SyntaxKind.RefKeyword)
                                        ),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("value")
                                        ),
                                    }),
                                    SyntaxFactory.Token(SyntaxKind.CloseParenToken)
                                )
                            )
                        )
                    )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                ),
            };
    }
}
