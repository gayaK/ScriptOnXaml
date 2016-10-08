﻿using System;
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

            _options = ScriptOptions.Default.WithReferences(domain.GetAssemblies());

            AppDomain.CurrentDomain.AssemblyLoad += (_, e) =>
            {
                _options = _options.AddReferences(e.LoadedAssembly);
            };
        }

        private static ScriptOptions _options = ScriptOptions.Default;

        public static Task<ScriptRunner<object>> CompileAsync<TGrobals>(string code)
        {
            return Task.Run(() => CSharpScript
                .Create(code, _options, typeof(TGrobals))
                .CreateDelegate());
        }
    }
}