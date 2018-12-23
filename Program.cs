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
                            header.Expressions.Enqueue(new Const
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
                            header.Expressions.Enqueue(new Define
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
                            header.Expressions.Enqueue(new Structure
                            {
                                Name = names[0].Trim(),
                                Fields = fields,
                                Comment = comment
                            });

                            comment = null;
                        }
                        else if (line.StartsWith("typedef VOID") 
                                || line.StartsWith("typedef INT") 
                                || line.StartsWith("typedef LPCSTR"))
                        {
                            // Ignore
                        }
                        else if (line.StartsWith("typedef"))
                        {
                            // #typedef type alias
                            var typedef = line.Split(" ");
                            header.Expressions.Enqueue(new TypeDefine
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

                            var param = new Parameter();
                            if (callback[1] == ");")
                            {
                                param.Name = "None";
                            }
                            else
                            {
                                param.Name = callback[1].Split(" ")[0].Replace("Ptr", "").Trim();
                            }

                            _cb.Param = new List<Parameter> { param };
                            _cb.Comment = comment;
                            comment = null;

                            header.Expressions.Enqueue(_cb);
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
            while (header.Expressions.Count > 0)
            {
                var expression = header.Expressions.Dequeue();
                if(expression is Const)
                {
                    //appendSectionSeparator(builder, "Consts");
                    var cons = expression as Const;

                    if (cons.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {cons.Comment.Content}");
                    }
                    builder.AppendLine($"{cons.Name} = {cons.Value}");
                } else if (expression is Define)
                {
                    //appendSectionSeparator(builder, "Defines");
                    var define = expression as Define;

                    if (define.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {define.Comment.Content}");
                    }
                    builder.AppendLine($"{define.Name} = {define.Value}");
                } else if (expression is TypeDefine)
                {
                    //appendSectionSeparator(builder, "Typedefs");
                    var typedef = expression as TypeDefine;

                    if (typedef.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {typedef.Comment.Content}");
                    }
                    builder.AppendLine($"{typedef.Alias} = {typedef.Type}");
                } else if (expression is Structure)
                {
                    //appendSectionSeparator(builder, "Structures");
                    var structure = expression as Structure;

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
                    
                } else if (expression is Callback)
                {
                    //appendSectionSeparator(builder, "Callbacks");
                    var callback = expression as Callback; 
                    if (expression.Comment != null)
                    {
                        builder.AppendLine();
                        builder.AppendLine($"# {expression.Comment.Content}");
                    }
                    builder.AppendLine($"{callback.Name} = CFUNCTYPE(None, POINTER({callback.Param[0].Name}))");
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
