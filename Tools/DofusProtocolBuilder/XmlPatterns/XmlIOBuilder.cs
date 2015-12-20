using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using DofusProtocolBuilder.Parsing;
using DofusProtocolBuilder.Parsing.Elements;

namespace DofusProtocolBuilder.XmlPatterns
{
    public abstract class XmlIOBuilder : XmlPatternBuilder
    {
        private const string RegexNewObject = @"new ([\w\d]+\.)*([\w\d]+)";

        protected XmlIOBuilder(Parser parser) : base(parser)
        {
        }

        public List<XmlField> GetXmlFields()
        {
            var xmlFields = new List<XmlField>();
            var localFields = new Dictionary<string, string>();

            MethodInfo deserializeAsMethod = Parser.Methods.Find(entry => entry.Name.Contains("deserializeAs"));
            string type = null;
            int limit = 0;

            for (int i = 0; i < deserializeAsMethod.Statements.Count; i++)
            {
                if (deserializeAsMethod.Statements[i] is AssignationStatement &&
                    ( (AssignationStatement)deserializeAsMethod.Statements[i] ).Value.Contains("Read"))
                {
                    var statement = ( (AssignationStatement)deserializeAsMethod.Statements[i] );
                    type = Regex.Match(statement.Value, @"Read([\w\d_]+)\(").Groups[1].Value.ToLower();
                    var name = statement.Name;

                    if (type == "bytes")
                        type = "byte[]";

                    Match arrayMatch = Regex.Match(name, @"^([\w\d]+)\[.+\]$");
                    if (arrayMatch.Success)
                    {
                        IEnumerable<string> limitLinq = from entry in Parser.Constructors[0].Statements
                                                        where
                                                            entry is AssignationStatement &&
                                                            ( (AssignationStatement)entry ).Name == arrayMatch.Groups[1].Value
                                                        let entryMatch =
                                                            Regex.Match(( (AssignationStatement)entry ).Value,
                                                                        @"new List<[\d\w\._]+>\(([\d]+)")
                                                        where entryMatch.Success
                                                        select entryMatch.Groups[1].Value;

                        if (limitLinq.Count() == 1)
                            limit = int.Parse(limitLinq.Single());

                        type += "[]";
                        name = name.Split('[')[0];
                    }
                    FieldInfo field = Parser.Fields.Find(entry => entry.Name == name);

                    if (field != null)
                    {
                        string condition = null;

                        if (i + 1 < deserializeAsMethod.Statements.Count &&
                            deserializeAsMethod.Statements[i + 1] is ControlStatement &&
                            ((ControlStatement) deserializeAsMethod.Statements[i + 1]).ControlType == ControlType.If)
                            condition = ((ControlStatement) deserializeAsMethod.Statements[i + 1]).Condition;

                        xmlFields.Add(new XmlField
                        {
                            Name = field.Name,
                            Type = type,
                            ArrayLength = limit > 0 ? limit.ToString() : null,
                            Condition = condition,
                        });

                        limit = 0;
                        type = null;
                    }
                    else
                        localFields.Add(name, type);
                }

                if (deserializeAsMethod.Statements[i] is InvokeExpression &&
                    ( (InvokeExpression)deserializeAsMethod.Statements[i] ).Name == "deserialize")
                {
                    var statement = ( (InvokeExpression)deserializeAsMethod.Statements[i] );
                    FieldInfo field = Parser.Fields.Find(entry => entry.Name == statement.Target);

                    if (field != null && xmlFields.Count(entry => entry.Name == field.Name) <= 0)
                    {
                        type = "Types." + field.Type.Name;

                        string condition = null;

                        if (i + 1 < deserializeAsMethod.Statements.Count &&
                            deserializeAsMethod.Statements[i + 1] is ControlStatement &&
                            ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).ControlType == ControlType.If)
                            condition = ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).Condition;

                        xmlFields.Add(new XmlField
                        {
                            Name = field.Name,
                            Type = type,
                            ArrayLength = limit > 0 ? limit.ToString() : null,
                            Condition = condition,
                        });

                        limit = 0;
                        type = null;
                    }
                    else if (i > 0 &&
                             deserializeAsMethod.Statements[i - 1] is AssignationStatement)
                    {
                        
                        var substatement = ( (AssignationStatement)deserializeAsMethod.Statements[i - 1] );
                        var name = substatement.Name;
                        Match match = Regex.Match(substatement.Value, RegexNewObject);

                        if (match.Success)
                        {
                            type = "Types." + match.Groups[2].Value;

                            Match arrayMatch = Regex.Match(name, @"^([\w\d]+)\[.+\]$");
                            if (arrayMatch.Success)
                            {
                                IEnumerable<string> limitLinq = from entry in Parser.Constructors[0].Statements
                                                                where
                                                                    entry is AssignationStatement &&
                                                                    ( (AssignationStatement)entry ).Name == arrayMatch.Groups[1].Value
                                                                let entryMatch =
                                                                    Regex.Match(( (AssignationStatement)entry ).Value,
                                                                                @"new List<[\d\w\._]+>\(([\d]+)")
                                                                where entryMatch.Success
                                                                select entryMatch.Groups[1].Value;

                                if (limitLinq.Count() == 1)
                                    limit = int.Parse(limitLinq.Single());

                                type += "[]";
                                name = name.Split('[')[0];

                            }
                        }

                        field = Parser.Fields.Find(entry => entry.Name == name);

                        if (field != null && xmlFields.Count(entry => entry.Name == field.Name) <= 0)
                        {
                            string condition = null;

                            if (i + 1 < deserializeAsMethod.Statements.Count &&
                                deserializeAsMethod.Statements[i + 1] is ControlStatement &&
                                ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).ControlType == ControlType.If)
                                condition = ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).Condition;

                            xmlFields.Add(new XmlField
                            {
                                Name = field.Name,
                                Type = type,
                                ArrayLength = limit > 0 ? limit.ToString() : null,
                                Condition = condition,
                            });

                            limit = 0;
                            type = null;
                        }
                    }
                }

                if (deserializeAsMethod.Statements[i] is AssignationStatement &&
                    ( (AssignationStatement)deserializeAsMethod.Statements[i] ).Value.Contains("getFlag"))
                {
                    var statement = ( (AssignationStatement)deserializeAsMethod.Statements[i] );
                    FieldInfo field = Parser.Fields.Find(entry => entry.Name == statement.Name);

                    var match = Regex.Match(statement.Value, @"getFlag\([_\w\d]+,\s?(\d+)\)");

                    if (match.Success)
                    {
                        type = "flag(" + match.Groups[1].Value + ")";

                        if (field != null)
                        {
                            xmlFields.Add(new XmlField
                            {
                                Name = field.Name,
                                Type = type,
                            });

                            type = null;
                        }
                    }
                }

                if (deserializeAsMethod.Statements[i] is AssignationStatement &&
                    ( (AssignationStatement)deserializeAsMethod.Statements[i] ).Value.Contains("getInstance"))
                {
                    var statement = ( (AssignationStatement)deserializeAsMethod.Statements[i] );
                    FieldInfo field = Parser.Fields.Find(entry => entry.Name == statement.Name);

                    type = "instance of Types." + Regex.Match(statement.Value, @"getInstance\((?:[\w\d\._]+\.)?([\w\d_]+),").Groups[1].Value;

                    if (field != null)
                    {
                        xmlFields.Add(new XmlField
                        {
                            Name = field.Name,
                            Type = type,
                        });

                        type = null;
                    }
                    else
                        localFields.Add(statement.Name, type);
                }

                if (deserializeAsMethod.Statements[i] is InvokeExpression &&
                    ( (InvokeExpression)deserializeAsMethod.Statements[i] ).Name == "Add" &&
                    type != null)
                {
                    var statement = ( (InvokeExpression)deserializeAsMethod.Statements[i] );

                    FieldInfo field = Parser.Fields.Find(entry => entry.Name == statement.Target);

                    string condition = null;

                    if (i + 1 < deserializeAsMethod.Statements.Count &&
                        deserializeAsMethod.Statements[i + 1] is ControlStatement &&
                        ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).ControlType == ControlType.If)
                        condition = ( (ControlStatement)deserializeAsMethod.Statements[i + 1] ).Condition;

                    var whileStatement = (ControlStatement)deserializeAsMethod.Statements.Take(i).
                        Last(x => x is ControlStatement && (x as ControlStatement).ControlType == ControlType.While);

                    // loc2 < loc3
                    var indexName = whileStatement.Condition.Split('<')[1].Trim();
                    var indexType = localFields[indexName];

                    xmlFields.Add(new XmlField
                    {
                        Name = field.Name,
                        Type = type + "[]",
                        ArrayLength = indexType,
                        Condition = condition,
                    });

                    limit = 0;
                    type = null;
                }
            }

            return xmlFields;
        }
    }
}