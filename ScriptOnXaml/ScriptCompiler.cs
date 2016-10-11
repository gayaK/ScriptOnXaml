using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptOnXaml
{
    public static class ScriptCompiler
    {
        static ScriptCompiler()
        {
            var domain = AppDomain.CurrentDomain;

            _options = ScriptOptions.Default
                .AddReferences(domain.GetAssemblies())
                .AddReferences(domain.Load("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))
                .AddImports("System")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Linq")
                .AddImports("System.Text")
                .AddImports("System.Threading.Tasks")
                .AddImports("System.Windows")
                .AddImports("System.Windows.Controls")
                .AddImports("System.Windows.Controls.Primitives")
                .AddImports("System.Windows.Data")
                .AddImports("System.Windows.Documents")
                .AddImports("System.Windows.Input")
                .AddImports("System.Windows.Media")
                .AddImports("System.Windows.Media.Animation")
                .AddImports("System.Windows.Navigation")
                .AddImports("System.Windows.Shapes")
                .AddImports("System.Windows.Shell");

            AppDomain.CurrentDomain.AssemblyLoad += (_, e) =>
            {
                _options = _options.AddReferences(e.LoadedAssembly);
            };
        }

        private static ScriptOptions _options = ScriptOptions.Default;

        public static Task<ScriptRunner<object>> CompileAsync<TGlobals>(string code)
        {
            return Task.Run(() => CSharpScript
                .Create(code, _options, typeof(TGlobals))
                .CreateDelegate());
        }
    }
}
