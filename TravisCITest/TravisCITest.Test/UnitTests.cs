using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using NUnit.Framework;

namespace TravisCITest.Test
{
    [TestFixture]
    public class UnitTest
    {
        CodeAnalysisVerifier Verifier { get; } = new CodeAnalysisVerifier(new TravisCITestAnalyzer(), new TravisCITestCodeFixProvider());

        [Test]
        public void Test1()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";
            var expectedDiagnostics = new DiagnosticResult
            {
                Id = "TravisCITest",
                Message = String.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            var actualDiagnostics = Verifier.GetSortedDiagnostics(test);
            Verifier.VerifyDiagnosticResults(actualDiagnostics, expectedDiagnostics);

            var expected = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var actual = Verifier.GetFixResult(test);
            actual.Is(expected);
        }

        [Test]
        public void FailTest()
        {
            "aaa".Is("bbb");
        }
    }
}