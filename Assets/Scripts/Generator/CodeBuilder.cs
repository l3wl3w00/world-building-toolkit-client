#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generator
{

    public class CodeBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private uint _tabCount = 0;

        public CodeBuilder AppendLineTabbed(string line = "")
        {
            AppendTabs();
            _stringBuilder.AppendLine(line);
            return this;
        }
        
        public CodeBuilder AppendTabbed(string str = "")
        {
            AppendTabs();
            _stringBuilder.Append(str);
            return this;
        }

        private CodeBuilder Append(string value)
        {
            _stringBuilder.Append(value);
            return this;
        }

        public CodeBuilder AppendLine(string line = "")
        {
            _stringBuilder.AppendLine(line);
            return this;
        }

        public CodeBuilder Namespace(string namespaceName, Action<CodeBuilder> build) => 
            this.AppendTabs()
                .AppendKeyWords(Keyword.Namespace)
                .AppendLine(namespaceName)
                .Block(build);

        public CodeBuilder Using(string? usingStatement)
        {
            if (usingStatement == null) return this;
            return this.Append("using ").Append(usingStatement).AppendLine(";");
        }

        public CodeBuilder PublicClass(string className, IEnumerable<string> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Class }, className, inherits, build);
        public CodeBuilder PublicClass(string className, IEnumerable<Type> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Class }, className, inherits, build);
        public CodeBuilder PublicPartialClass(string className, IEnumerable<Type> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Partial, Keyword.Class }, className, inherits, build);
        public CodeBuilder PublicPartialClass(string className, IEnumerable<string> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Partial, Keyword.Class }, className, inherits, build);

        public CodeBuilder PublicPartialStruct(string className, IEnumerable<Type> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Partial, Keyword.Struct }, className, inherits, build);
        
        public CodeBuilder PublicPartialStruct(string className, IEnumerable<string> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Partial, Keyword.Struct }, className, inherits, build);
        public CodeBuilder PublicSealedPartialClass(string className, IEnumerable<string> inherits, Action<CodeBuilder>? build = null) =>
            Class(new[] { Keyword.Public, Keyword.Sealed, Keyword.Partial, Keyword.Class }, className, inherits, build);

        public CodeBuilder Class(IEnumerable<Keyword> keywords, string className, IEnumerable<Type> inherits, Action<CodeBuilder>? build = null) => 
            Class(keywords, className, inherits.Select(t => t.GetNameAsString()), build);

        public CodeBuilder Class(IEnumerable<Keyword> keywords, string className, IEnumerable<string> inherits, Action<CodeBuilder>? build = null)
        {
            var inheritList = inherits.ToList();
            AppendTabs();
            AppendKeyWords(keywords.ToArray())
                .Append(className);
            
            if (inheritList.Count <= 0) return AppendLineTabbed().Block(build);
            
            Append(" : ");
            foreach(var inherit in inheritList.SkipLast(1))
            {
                Append(inherit).Append(", ");
            };
            Append(inheritList.Last());

            return AppendLineTabbed().Block(build);
        }

        public CodeBuilder Block(Action<CodeBuilder>? insideBlock)
        {
            AppendLineTabbed("{");
            AddTab();
            insideBlock?.Invoke(this);
            RemoveTab();
            return AppendLineTabbed("}");
        }

        public void AddTab()
        {
            _tabCount++;
        }

        public CodeBuilder AppendKeyWords(params Keyword[] keywords)
        {
            foreach (var k in keywords)
            {
                Append(k.Value).Append(" ");
            }

            return this;
        }

        public void RemoveTab()
        {
            _tabCount--;
        }


        private CodeBuilder AppendTabs()
        {
            for (int i = 0; i < _tabCount; i++)
            {
                _stringBuilder.Append('\t');
            }

            return this;
        }
        
        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
        
        
    }
    public struct Keyword
    {
        public readonly string Value;
        private Keyword(string value)
        {
            Value = value;
        }


        public static Keyword KeywordOfType(Type type)
        {
            var isRecord = type.GetMethods().Any(m => m.Name == "<Clone>$");
            if (isRecord) return Record;
            return type switch
            {
                { IsEnum : true } => Enum,
                { IsValueType : true } => Struct,
                { IsClass : true } => Class,
                _ => throw new Exception($"Unsupported keyword for type {type.Name}"),
            };
        }
        public static Keyword Namespace = new Keyword("namespace");
        public static Keyword Public = new Keyword("public");
        public static Keyword Class = new Keyword("class");
        public static Keyword Partial = new Keyword("partial");
        public static Keyword Record = new Keyword("record");
        public static Keyword Struct = new Keyword("struct");
        public static Keyword Enum = new Keyword("enum");
        public static Keyword Sealed = new Keyword("sealed");
    }
}