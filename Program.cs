using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DataStructures;
using System.Text;

namespace CToPython
{
    class Program
    {

        public static Dictionary<string, string> datatypes = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            try
            {
                datatypes.Add("CHAR", "c_char");
                datatypes.Add("BYTE", "c_ubyte");
                datatypes.Add("Array", "c_char*");
                processHeader("cpp.h", "python.py");
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private static void processHeader(string headerFile, string pythonFile) {
            if (File.Exists(headerFile)) {
                var content = File.ReadAllLines(headerFile);
                if (content.Length > 0) {
                    Header header = new Header();
                    Comment comment = null;
                    for (int i = 0; i < content.Length; i++) {
                        var line = content[i].Trim();
                        if (line.StartsWith("//"))
                        {
                            if(comment == null)
                            {
                                comment = new Comment();
                            }

                            comment.Content += line.Replace("//", "").Trim();
                        }
                        if (line.StartsWith("const"))
                        {
                            var cons = line.Split(" ");
                            header.Consts.Add(new Const
                            {
                                Name = cons[2],
                                Value = cons[4].Substring(0, cons[4].IndexOf(";")), 
                                Comment = comment
                            });

                            comment = null;
                        }
                        else if (line.StartsWith("#define"))
                        {
                            // #define name value
                            var define = line.Split(" ");
                            header.Defines.Add(new Define
                            {
                                Name = define[1],
                                Value = define[2],
                                Comment = comment
                            });

                            comment = null;
                        }
                        else if (line.StartsWith("typedef struct"))
                        {
                            // #typedef struct {
                            // type name; 
                            //  } name, *ptr;
                            var structure = line.Split(" ");
                            var fields = new List<Field>();
                            line = content[++i];
                            do
                            {
                                var tokens = line.Trim().Split(" ");
                                try
                                {
                                    var _field = new Field();
                                    if (tokens[0].StartsWith("Array"))
                                    {
                                        var _ctype = tokens[0];
                                        int _len = int.Parse(_ctype.Substring(5)) + 1;
                                        _field.Type = datatypes["Array"] + (_len);
                                    }
                                    else
                                    {
                                        _field.Type = datatypes[tokens[0]];
                                    }
                                    _field.Name = tokens[1].Replace(";", "");
                                    fields.Add(_field);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }
                                line = content[++i];
                            } while (!(line.Contains("}") && line.Contains(";")));
                            line = line.Replace("}", "").Replace(";", "");
                            var names = line.Split(",");
                            header.Structures.Add(new Structure
                            {
                                Name = names[0].Trim(),
                                Fields = fields,
                                Comment = comment
                            });

                            comment = null;
                        }
                        else if (line.StartsWith("typedef"))
                        {
                            // #typedef type alias
                            var typedef = line.Split(" ");
                            header.TypeDefines.Add(new TypeDefine
                            {
                                Type = typedef[1],
                                Alias = typedef[2].Replace(";", ""),
                                Comment = comment
                            });

                            comment = null;
                        }
                        else if (line.StartsWith("VOID WINAPI")) {
                            // VOID WINAPI name(paramname name);
                            // name = CFUNCTRPE(None, POINTE(paramname))
                            line = line.Replace("VOID WINAPI", "").Trim();
                            var callback = line.Split("(");
                            var _cb = new Callback();
                            // _cb.Name = callback[2].Substring(0, callback[2].IndexOf('('));
                            _cb.Name = callback[0];

                            if (callback[1] == ");")
                            {
                                _cb.Param = "None";
                            }
                            else
                            {
                                _cb.Param = callback[1].Split(" ")[0].Replace("Ptr", "").Trim();
                            }
                            
                            _cb.Comment = comment;
                            comment = null;

                            header.Callbacks.Add(_cb);
                        }
                }
                
                writePython(header, pythonFile);
                Console.WriteLine("Processed header and written in Python file.");
            } else {
                Console.WriteLine("Header file is empty.");
            }
            } else {
                Console.WriteLine("File {0} does not exists.", headerFile);
            }
        }

        private static void writePython(Header header, string pythonFile) {
            if(header == null || string.IsNullOrEmpty(pythonFile)) {
                return;
            }

            StringBuilder builder = new StringBuilder();

            if (header.Consts.Count != 0)
            {
                appendSectionSeparator(builder, "Consts");
                foreach (var cons in header.Consts)
                {
                    if(cons.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {cons.Comment.Content}");
                    }
                    builder.AppendLine($"{cons.Name} = {cons.Value}");
                }
            }
            
            if (header.Defines.Count != 0)
            {
                appendSectionSeparator(builder, "Defines");
                foreach (var define in header.Defines)
                {
                    if (define.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {define.Comment.Content}");
                    }
                    builder.AppendLine($"{define.Name} = {define.Value}");
                }
            }
      
            if (header.TypeDefines.Count !=0)
            {
                appendSectionSeparator(builder, "Typedefs");
                foreach (var typedef in header.TypeDefines)
                {
                    if (typedef.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {typedef.Comment.Content}");
                    }
                    builder.AppendLine($"{typedef.Alias} = {typedef.Type}");
                }
            }
            
            if (header.Structures.Count != 0)
            {
                appendSectionSeparator(builder, "Structures");
                foreach (var structure in header.Structures)
                {
                    if (structure.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {structure.Comment.Content}");
                    }
                    builder.AppendLine($"class {structure.Name}(Structure):");
                    builder.Append("    _fields_ = [");
                    for (int i = 0; i < structure.Fields.Count; i++)
                    {
                        var field = structure.Fields[i];
                        if (i == structure.Fields.Count - 1)
                        {
                            builder.Append($"(\"{field.Name}\", {field.Type})");
                        }
                        else
                        {
                            builder.Append($"(\"{field.Name}\", {field.Type}),\n\t\t\t ");
                        }
                    }
                    builder.AppendLine("]");
                }
            }
            if (header.Callbacks.Count != 0)
            {
                appendSectionSeparator(builder, "Callbacks");
                foreach (var callback in header.Callbacks)
                {
                    if (callback.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {callback.Comment.Content}");
                    }
                    builder.AppendLine($"{callback.Name} = CFUNCTYPE(None, POINTER({callback.Param}))");
                }
            }

            File.WriteAllText(pythonFile, builder.ToString());
        }

        #region Helper Method

        private static void dicDatatypes()
        {
            datatypes = new Dictionary<string, string>();
            datatypes.Add("CHAR", "c_char");
            datatypes.Add("BYTE", "c_ubyte");
            datatypes.Add("Array", "c_char*");
        }

        private static void appendSectionSeparator(StringBuilder builder , string section)
        {
            builder.AppendLine($"\n############################################## {section} ##############################################\n");
        } 
        #endregion Helper Method 



    }
}
