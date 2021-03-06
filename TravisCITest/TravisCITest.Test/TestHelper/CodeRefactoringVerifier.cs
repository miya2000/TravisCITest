﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace TestHelper
{
    public class CodeRefactoringVerifier
    {
        public CodeRefactoringVerifier(CodeRefactoringProvider codeRefactoring)
        {
            this.CodeRefactoring = codeRefactoring;
        }

        public string Language { get; set; } = LanguageNames.CSharp;

        public CodeRefactoringProvider CodeRefactoring { get; }

        public string GetRefactoringResult(string oldSource, int index, string equivalenceKey = null)
        {
            return CodeAnalysisHelper.GetRefactoringResult(this.Language, this.CodeRefactoring, oldSource, new TextSpan(index, 0), equivalenceKey);
        }

        public string GetRefactoringResult(string oldSource, int fromIndex, int toIndex, string equivalenceKey = null)
        {
            return CodeAnalysisHelper.GetRefactoringResult(this.Language, this.CodeRefactoring, oldSource, new TextSpan(fromIndex, toIndex - fromIndex), equivalenceKey);
        }

        public void VerifyRefactoring(string oldSource, int index, string newSource, string equivalenceKey = null, bool allowNewCompilerDiagnostics = false)
        {
            var actual = GetRefactoringResult(oldSource, index, equivalenceKey);
            Assert.AreEqual(newSource, actual);
        }

        public List<CodeAction> GetRefactoringActions(string source, int index)
        {
            return CodeAnalysisHelper.GetRefactoringActions(this.Language, this.CodeRefactoring, source, new TextSpan(index, 0));
        }

        public List<CodeAction> GetRefactoringActions(string source, int fromIndex, int toIndex)
        {
            return CodeAnalysisHelper.GetRefactoringActions(this.Language, this.CodeRefactoring, source, new TextSpan(fromIndex, toIndex - fromIndex));
        }
    }
}
