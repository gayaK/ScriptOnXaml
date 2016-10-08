using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptOnXaml
{
    public class ScriptCommand : MarkupExtension, ICommand
    {
        public ScriptCommand()
        {

        }

        public event EventHandler CanExecuteChanged;

        private readonly ScriptCommandArguments _args = new ScriptCommandArguments();

        private Task<ScriptRunner<object>> _compleTask;

        public string ScriptCode
        {
            get
            {
                return _scriptCode;
            }
            set
            {
                if (_scriptCode != value)
                {
                    _scriptCode = value;
                    _compleTask = ScriptCompiler.CompileAsync<ScriptCommandArguments>(value);
                }
            }
        }
        private string _scriptCode = "";

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            _args.Prm = parameter;
            var runner = await _compleTask;
            await runner(_args);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public class ScriptCommandArguments
        {
            public dynamic Prm { get; set; }
        }
    }
}
