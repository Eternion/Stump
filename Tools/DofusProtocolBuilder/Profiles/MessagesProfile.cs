using System;
using System.CodeDom.Compiler;
using System.IO;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Templates;
using Microsoft.VisualStudio.TextTemplating;

namespace DofusProtocolBuilder.Profiles
{
    public class MessagesProfile : ParsingProfile
    {
        public override void ExecuteProfile(Parser parser)
        {
            string file = Path.Combine(Program.Configuration.Output, OutPutPath, GetRelativePath(parser.Filename), Path.GetFileNameWithoutExtension(parser.Filename));
            var xmlMessage = Program.Configuration.XmlMessagesProfile.SearchXmlPattern(Path.GetFileNameWithoutExtension(parser.Filename));

            if (xmlMessage == null)
                Program.Shutdown(string.Format("File {0} not found", file));

            var engine = new Engine();
            var host = new TemplateHost(TemplatePath);
            host.Session["Message"] = xmlMessage;
            host.Session["Profile"] = this;
            var output = engine.ProcessTemplate(File.ReadAllText(TemplatePath), host);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine("File:{0} Line:{1} : {2}", error.FileName, error.Line, error.ErrorText);
            }

            if (host.Errors.Count > 0)
                Program.Shutdown();

            File.WriteAllText(file + host.FileExtension, output);

            Console.WriteLine("Wrote {0}", file);
        }
    }
}