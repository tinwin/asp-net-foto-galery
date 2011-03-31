// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace EFPocoAdapter.CodeGeneration
{
    public class SourceWriter
    {
        private IDictionary<string, string> _variables = new Dictionary<string, string>();
        private TextWriter _textWriter;
        private int _indentLevel;

        public SourceWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public IDictionary<string, string> Variables
        {
            get { return _variables; }
            private set { _variables = value; }
        }

        public IDisposable NewVariableScope()
        {
            IDictionary<string, string> oldVariables = _variables;
            _variables = new Dictionary<string, string>(_variables);
            return new VariableRestorer(this, oldVariables);
        }

        public IDisposable Indent()
        {
            WriteLine("{");
            _indentLevel++;
            return new Outdenter(this);
        }

        public void Unindent()
        {
            _indentLevel--;
            WriteLine("}");
        }

        public void WriteLine()
        {
            _textWriter.WriteLine();
        }

        public void WriteLine(string line)
        {
            for (int i = 0; i < _indentLevel; ++i)
            {
                _textWriter.Write("    ");
            }
            _textWriter.WriteLine(ReplaceVariables(line));
        }

        private string ReplaceVariables(string text)
        {
            foreach (var k in _variables)
            {
                text = text.Replace("{" + k.Key + "}", k.Value);
            }
            return text;
        }

        class VariableRestorer : IDisposable
        {
            private SourceWriter _writer;
            private IDictionary<string, string> _variables;

            internal VariableRestorer(SourceWriter writer, IDictionary<string, string> variables)
            {
                _variables = variables;
                _writer = writer;
            }
            public void Dispose()
            {
                _writer.Variables = _variables;
                GC.SuppressFinalize(this);
            }
        }

        class Outdenter : IDisposable
        {
            private SourceWriter _writer;

            internal Outdenter(SourceWriter writer)
            {
                _writer = writer;
            }

            public void Dispose()
            {
                _writer.Unindent();
                GC.SuppressFinalize(this);
            }
        }
    }
}
