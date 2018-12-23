using System.Collections.Generic;

namespace DataStructures {
    public class Expression
    {
        public Comment Comment { get; set; }
    }

    public class Comment
    {
        public string Content { get; set; }
    }

    public class Const : Expression
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Define : Expression
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class TypeDefine : Expression
    {
        public string Type { get; set; }
        public string Alias { get; set; }
    }

    public class Structure : Expression
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
    }

    public class Field 
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Callback : Expression
    {
        public string Name { get; set; }
        public List<Parameter> Param { get; set; }
    }

    public class Parameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Header {
        public Queue<Expression> Expressions { get; set; }

        public Header() {
            Expressions = new Queue<Expression>();
        }
    }
}
