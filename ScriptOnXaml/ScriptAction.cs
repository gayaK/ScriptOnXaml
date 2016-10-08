﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptOnXaml
{
    public class ScriptAction : TriggerAction<DependencyObject>
    {
        public ScriptAction()
        {
        }

        private readonly ScriptActionArguments _args = new ScriptActionArguments();

        private Task<ScriptRunner<object>> _compileTask;

        #region ScriptCode
        public string ScriptCode
        {
            get { return (string)GetValue(ScriptCodeProperty); }
            set { SetValue(ScriptCodeProperty, value); }
        }

        public static readonly DependencyProperty ScriptCodeProperty =
            DependencyProperty.Register(
                "ScriptCode",
                typeof(string),
                typeof(ScriptAction),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    (d, e) =>
                    {
                        // スクリプトをコンパイルしてデリゲートを作成する。
                        ((ScriptAction)d)._compileTask = ScriptCompiler.CompileAsync<ScriptActionArguments>((string)e.NewValue);
                    }));
        #endregion

        #region Arg1
        public object Arg1
        {
            get { return GetValue(Arg1Property); }
            set { SetValue(Arg1Property, value); }
        }

        public static readonly DependencyProperty Arg1Property =
            DependencyProperty.Register(
                "Arg1",
                typeof(object),
                typeof(ScriptAction),
                new FrameworkPropertyMetadata(null, (d, e) => ((ScriptAction)d)._args.Arg1 = e.NewValue));
        #endregion

        #region Arg2
        public object Arg2
        {
            get { return GetValue(Arg2Property); }
            set { SetValue(Arg2Property, value); }
        }

        public static readonly DependencyProperty Arg2Property =
            DependencyProperty.Register(
                "Arg2",
                typeof(object),
                typeof(ScriptAction),
                new FrameworkPropertyMetadata(null, (d, e) => ((ScriptAction)d)._args.Arg2 = e.NewValue));
        #endregion

        #region Arg3
        public object Arg3
        {
            get { return GetValue(Arg3Property); }
            set { SetValue(Arg3Property, value); }
        }

        public static readonly DependencyProperty Arg3Property =
            DependencyProperty.Register(
                "Arg3",
                typeof(object),
                typeof(ScriptAction),
                new FrameworkPropertyMetadata(null, (d, e) => ((ScriptAction)d)._args.Arg3 = e.NewValue));
        #endregion

        #region Arg4
        public object Arg4
        {
            get { return GetValue(Arg4Property); }
            set { SetValue(Arg4Property, value); }
        }

        public static readonly DependencyProperty Arg4Property =
            DependencyProperty.Register(
                "Arg4",
                typeof(object),
                typeof(ScriptAction),
                new FrameworkPropertyMetadata(null, (d, e) => ((ScriptAction)d)._args.Arg4 = e.NewValue));
        #endregion

        protected override void OnAttached()
        {
            _args.Arg0 = AssociatedObject;

            base.OnAttached();
        }

        protected override async void Invoke(object parameter)
        {
            if (_compileTask == null)
            {
                return;
            }
            var runner = await _compileTask;
            await runner(_args);
        }
    }

    public class ScriptActionArguments
    {
        public dynamic Arg0 { get; set; }
        public dynamic Arg1 { get; set; }
        public dynamic Arg2 { get; set; }
        public dynamic Arg3 { get; set; }
        public dynamic Arg4 { get; set; }
    }

}