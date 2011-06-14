
using System.Collections.Generic;
using System.Linq;

namespace Stump.Tools.UtilityBot.FileWriter
{
    internal class CsFileWriter : FileWriter
    {
        private bool m_inNamespace;

        public CsFileWriter(string outputPath, IEnumerable<string> namespaces)
            : base(outputPath)
        {
            Namespaces = namespaces.ToList();
        }

        public List<string> Namespaces
        {
            get;
            set;
        }

        public void StartNamespace(string name)
        {
            WriteLineWithIndent("namespace " + name);
            WriteLineWithIndent("{");
            m_inNamespace = true;

            IncreaseIntendation();
        }

        public void EndNamespace()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");

            m_inNamespace = false;
        }

        public void StartClass(ClassInfo classInfo)
        {
            if(string.IsNullOrEmpty(classInfo.CustomAttribute))
                WriteLineWithIndent(classInfo.CustomAttribute);

            WriteAccessModifier(classInfo.AccessModifier);
            WriteClassModifier(classInfo.ClassModifier);

            m_writer.WriteLine("class " + classInfo.Name +
                               ( !string.IsNullOrEmpty(classInfo.Heritage) ? " : " + classInfo.Heritage : "" ));
            WriteLineWithIndent("{");

            IncreaseIntendation();
        }

        public void EndClass()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
        }

        public void StartClassWithNamespace(ClassInfo classInfo)
        {
            if (!string.IsNullOrEmpty(classInfo.Namespace))
            {
                foreach (string str in Namespaces)
                    WriteLineWithIndent("using " + str + ";");

                StartNamespace(classInfo.Namespace);
            }

            WriteLineWithIndent();

            if (!string.IsNullOrEmpty(classInfo.CustomAttribute))
                WriteLineWithIndent(classInfo.CustomAttribute);

            WriteAccessModifier(classInfo.AccessModifier);
            WriteClassModifier(classInfo.ClassModifier);

            m_writer.WriteLine("class " + classInfo.Name +
                               (!string.IsNullOrEmpty(classInfo.Heritage) ? " : " + classInfo.Heritage : ""));
            WriteLineWithIndent("{");

            IncreaseIntendation();
        }

        public void EndClassWithNamespace()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");

            if (m_inNamespace)
            {
                EndNamespace();
            }

            m_writer.Close();
        }

        public void StartControlSequence(ControlSequenceType type, string condition)
        {
            switch (type)
            {
                case ControlSequenceType.IF:
                    WriteLineWithIndent("if ( " + condition + " )");
                    WriteLineWithIndent("{");
                    IncreaseIntendation();
                    break;

                case ControlSequenceType.ELSEIF:
                    WriteLineWithIndent("else if ( " + condition + " )");
                    WriteLineWithIndent("{");
                    IncreaseIntendation();
                    break;

                case ControlSequenceType.ELSE:
                    WriteLineWithIndent("else");
                    WriteLineWithIndent("{");
                    IncreaseIntendation();
                    break;

                case ControlSequenceType.WHILE:
                    WriteLineWithIndent("while ( " + condition + " )");
                    WriteLineWithIndent("{");
                    IncreaseIntendation();
                    break;

                case ControlSequenceType.BREAK:
                    if (condition == null)
                        WriteLineWithIndent("break;");
                    else
                        WriteLineWithIndent("break " + condition + ";");
                    break;

                case ControlSequenceType.RETURN:
                    if (condition == null)
                        WriteLineWithIndent("return;");
                    else
                        WriteLineWithIndent("return " + condition + ";");
                    break;
            }
        }

        public void EndControlSequence()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
        }

        public void StartMethod(MethodInfo info)
        {
            WriteAccessModifier(info.AccessModifier);

            foreach (MethodInfo.MethodModifiers m in info.Modifiers)
            {
                WriteMethodModifier(m);
            }

            if (!string.IsNullOrEmpty(info.ReturnType))
                WriteReturnType(info.ReturnType, info.ReturnsArray);

            m_writer.Write(info.Name + "(");

            for (int i = 0; i < info.ArgsType.Length; i++)
            {
                if (i != 0)
                {
                    m_writer.Write(", ");
                }
                m_writer.Write(info.ArgsType[i] + " " + info.Args[i] +
                               (info.ArgsDefaultValue[i] != null ? " = " + info.ArgsDefaultValue[i] : ""));
            }

            m_writer.WriteLine(")");
            WriteLineWithIndent("{");
            IncreaseIntendation();
        }

        public void StartMethod(MethodInfo info, string customLine)
        {
            WriteAccessModifier(info.AccessModifier);

            foreach (MethodInfo.MethodModifiers m in info.Modifiers)
            {
                WriteMethodModifier(m);
            }

            if (!string.IsNullOrEmpty(info.ReturnType))
                WriteReturnType(info.ReturnType, info.ReturnsArray);

            m_writer.Write(info.Name + "(");

            for (int i = 0; i < info.ArgsType.Length; i++)
            {
                if (i != 0)
                {
                    m_writer.Write(", ");
                }
                m_writer.Write(info.ArgsType[i] + " " + info.Args[i] +
                               (info.ArgsDefaultValue[i] != null ? " = " + info.ArgsDefaultValue[i] : ""));
            }

            m_writer.WriteLine(")");
            WriteLineWithIndent("\t" + customLine);
            WriteLineWithIndent("{");
            IncreaseIntendation();
        }

        public void EndMethod()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
            WriteLineWithIndent();
        }


        public void StartProperty(PropertyInfo info)
        {
            WriteAccessModifier(info.AccessModifier);

            if (!string.IsNullOrEmpty(info.PropertyType))
                WriteReturnType(info.PropertyType, false);

            m_writer.Write(info.Name);

            WriteLineWithIndent("{");
            IncreaseIntendation();
        }

        public void StartGetProperty()
        {
            WriteLineWithIndent("get");
            WriteLineWithIndent("{");
            IncreaseIntendation();
        }

        public void StartSetProperty()
        {
            WriteLineWithIndent("set");
            WriteLineWithIndent("{");
            IncreaseIntendation();
        }

        public void EndSetProperty()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
        }

        public void EndGetProperty()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
        }

        public void EndProperty()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
            WriteLineWithIndent();
        }

        public void StartEnum(AccessModifiers modifier, string enumName)
        {
            WriteAccessModifier(modifier);
            WriteLineWithIndent("enum " + enumName);
            WriteLineWithIndent("{");

            IncreaseIntendation();
        }

        public void WriteEnumElement(string elementName)
        {
            WriteLineWithIndent(string.Format("{0},", elementName));
        }

        public void WriteEnumElement(string elementName, string value)
        {
            WriteLineWithIndent(string.Format("{0} = {1},", elementName, value));
        }

        public void EndEnum()
        {
            DecreaseIntendation();
            WriteLineWithIndent("}");
        }

        public void WriteField(AccessModifiers modifier, string type, string name, string value, string stereotype)
        {
            WriteAccessModifier(modifier);

            if (!string.IsNullOrEmpty(stereotype))
            {
                m_writer.Write(stereotype + " ");
            }

            m_writer.Write(type + " ");
            m_writer.Write(name);

            if (!string.IsNullOrEmpty(value))
            {
                m_writer.Write(" = " + value);
            }

            m_writer.Write(";");
            m_writer.WriteLine();
        }

        public void WriteVariableAssignation(string target, string variable, string value, string typedeclaration)
        {
            WriteWithIndent();

            if (!string.IsNullOrEmpty(typedeclaration))
            {
                m_writer.Write(typedeclaration + " ");
            }

            if (!string.IsNullOrEmpty(target))
            {
                m_writer.Write(target + ".");
            }

            m_writer.Write(variable);

            if (!string.IsNullOrEmpty(value))
            {
                m_writer.Write(" = ");

                m_writer.Write(value);
            }

            m_writer.Write(";");
            m_writer.WriteLine();
        }

        public void WriteAttribute(string type, string name, string accessRight)
        {
            WriteLineWithIndent(accessRight + " " + type + " " + name + ";");
            WriteLineWithIndent();
        }

        public void WriteExecution(string target, string method, string returnvariable, string returnvariabletype,
                                   string returnvariabletarget, string[] args, string stereotype)
        {
            WriteWithIndent();

            if (!string.IsNullOrEmpty(returnvariabletype))
            {
                m_writer.Write(returnvariabletype + " ");
            }

            if (!string.IsNullOrEmpty(returnvariabletarget))
            {
                m_writer.Write(returnvariabletarget + ".");
            }

            if (!string.IsNullOrEmpty(returnvariable))
            {
                if ("new".Equals(stereotype))
                {
                    m_writer.Write(returnvariable + " = new ");
                }
                else
                {
                    m_writer.Write(returnvariable + " = ");
                }
            }

            if (stereotype == "throw new")
            {
                m_writer.Write(stereotype + " ");
            }
            else if (stereotype.StartsWith("(") && stereotype.EndsWith(")"))
            {
                m_writer.Write(stereotype);
            }

            if (!string.IsNullOrEmpty(target))
            {
                m_writer.Write(target + ".");
            }

            m_writer.Write(method + "(");
            if (args != null)
            {
                bool first = true;
                foreach (string a in args)
                {
                    if (!first)
                    {
                        m_writer.Write(", ");
                    }
                    first = false;
                    m_writer.Write(a);
                }
            }
            m_writer.Write(");");
            m_writer.WriteLine();
        }

        public void WriteComment(string comment)
        {
            WriteCustom("// " + comment);
        }

        public void WriteComment(string[] comment)
        {
            if (comment.Length == 1)
            {
                WriteComment(comment[0]);
                return;
            }

            WriteLineWithIndent("/* " + comment[0]);

            for (int i = 1; i < comment.Length - 1; i++)
            {
                WriteLineWithIndent(" * " + comment[i]);
            }

            WriteLineWithIndent(" * " + comment.Last() + " */");
        }

        public void WriteCustom(string custom)
        {
            WriteLineWithIndent(custom);
        }

        private void WriteAccessModifier(AccessModifiers accessModifier)
        {
            switch (accessModifier)
            {
                case AccessModifiers.PUBLIC:
                    WriteWithIndent("public ");
                    break;
                case AccessModifiers.PROTECTED:
                    WriteWithIndent("protected ");
                    break;
                case AccessModifiers.PRIVATE:
                    WriteWithIndent("private ");
                    break;
                case AccessModifiers.INTERNAL:
                    WriteWithIndent("internal ");
                    break;
            }
        }

        private void WriteClassModifier(ClassInfo.ClassModifiers modifier)
        {
            switch (modifier)
            {
                case ClassInfo.ClassModifiers.ABSTRACT:
                    m_writer.Write("abstract ");
                    break;
            }
        }

        private void WriteMethodModifier(MethodInfo.MethodModifiers modifier)
        {
            switch (modifier)
            {
                case MethodInfo.MethodModifiers.ABSTRACT:
                    m_writer.Write("abstract ");
                    break;
                case MethodInfo.MethodModifiers.CONSTANT:
                    m_writer.Write("const ");
                    break;
                case MethodInfo.MethodModifiers.OVERRIDE:
                    m_writer.Write("override ");
                    break;
                case MethodInfo.MethodModifiers.NEW:
                    m_writer.Write("new ");
                    break;
                case MethodInfo.MethodModifiers.STATIC:
                    m_writer.Write("static ");
                    break;
                case MethodInfo.MethodModifiers.VIRTUAL:
                    m_writer.Write("virtual ");
                    break;
            }
        }

        private void WriteReturnType(string returnType, bool isArray)
        {
            m_writer.Write(returnType + (isArray ? "[] " : " "));
        }
    }
}