using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptOnXaml
{
    public class ScriptConverter : MarkupExtension, IValueConverter
    {
        public ScriptConverter(string code)
        {
            ScriptCode = code;
        }
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
                    _compleTask = ScriptCompiler.CompileAsync<Globals>(value);
                }
            }
        }
        private string _scriptCode = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var globals = new Globals
            {
                Value = value,
                Prm = parameter,
            };
            var runnter = _compleTask.Result;
            var result = runnter(globals).Result;
            if (result ==null && targetType.IsClass)
            {
                return null;
            }
            else if (targetType.IsAssignableFrom(result.GetType()))
            {
                return result;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public class Globals
        {
            public dynamic Value { get; set; }
            public dynamic Prm { get; set; }
        }
    }
}
