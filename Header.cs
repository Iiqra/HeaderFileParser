using System.Collections.Generic;

namespace DataStructures {
    public class Comment
    {
        public string Content { get; set; }
    }

    public class Const
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Comment Comment { get; set; }
    }

    public class Define {
        public string Name { get; set; }
        public string Value { get; set; }
        public Comment Comment { get; set; }
    }

    public class TypeDefine {
        public string Type { get; set; }
        public string Alias { get; set; }
        public Comment Comment { get; set; }
    }

    public class Structure {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public Comment Comment { get; set; }
    }

    public class Field {
        public string Name { get; set; }
        public string Type { get; set; }
        public Comment Comment { get; set; }
    }

    public class Callback
    {
        public string Name { get; set; }
        public string Param { get; set; }
        public Comment Comment { get; set; }
    }

    public class Header {
        public List<Const> Consts { get; set; }
        public List<Define> Defines { get; set; }
        public List<TypeDefine> TypeDefines { get; set; }
        public List<Structure> Structures { get; set; }
        public List<Callback> Callbacks { get; set; }

        public Header() {
            Consts = new List<Const>();
            Defines = new List<Define>();
            TypeDefines = new List<TypeDefine>();
            Structures = new List<Structure>();
            Callbacks = new List<Callback>();
        }
    }
}
