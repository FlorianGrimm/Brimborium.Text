#pragma warning disable IDE0301 // Simplify collection initialization
#pragma warning disable IDE0350 // Use implicitly typed lambda
#pragma warning disable IDE0305 // Simplify collection initialization

using Brimborium.Text;

using System.Collections.Immutable;

namespace Brimborium.Gerede.Tests;

public class MacroGrammerTests {
}

#pragma warning disable CA2211 // Non-constant fields should not be visible

public partial class MacroGrammer {
    public static BGGrammer<MacroGrammerResult> Grammer = new BGGrammer<MacroGrammerResult>(
        BGParser.Refer(() => MacroGrammerResult.Parser)
        .Then(BGParser.Token(BGTokenizer.AcceptEOF()))
        .Returns1()
        );
    public static IBGTokenizer<BGVoid> TokenWhitespace = BGTokenizer.AcceptChar(" /t\r\n").TRepeat(0, 1024);
    public static IBGTokenizer<string> TokenMultiCommentStart =
        (BGTokenizer.AcceptChar("/")
        .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
        .Then(TokenWhitespace)
        .Returns(
            static (value1, value2, value3, match)
                => match.Substring(0, value2.Match.End - value1.Match.Start).ToString()
            ));

    public static IBGTokenizer<BGVoid> TokenPrefixMacro = BGTokenizer.AcceptString("#Macro:");

    public static IBGTokenizer<BGVoid> TokenIdentifierStart = BGTokenizer.AcceptChar("ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz");
    public static IBGTokenizer<BGVoid> TokenIdentifierRest = BGTokenizer.AcceptChar("_ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz0123456789");

    public static IBGTokenizer<BGVoid> TokenIdentifier
        = TokenIdentifierStart
            .Then(TokenIdentifierRest.TRepeat(0, 255))
            .Returns();

    public static IBGTokenizer<string> TokenIdentifierAsString
        = TokenIdentifierStart
            .Then(TokenIdentifierRest.TRepeat(0, 255))
            .Returns(static (_, _, match) => match.ToString());

    public static IBGTokenizer<string> TokenMultiCommentEnd =
        TokenWhitespace
        .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
        .Then(BGTokenizer.AcceptChar("/"))
        .Returns(
                static (value1, value2, value3, match)
                => match.Substring(value1.Match.Length, value3.Match.End - value2.Match.Start).ToString()
            )
        ;

    public MacroGrammer() {
    }

}

public partial record class MacroGrammerResult(
    ImmutableList<MacroGrammerNode> ListNode
    ) {

    public MacroGrammerResult() : this(ListNode: ImmutableList<MacroGrammerNode>.Empty) { }

    public static readonly IBGParser<MacroGrammerResult> Parser =
        BGParser.POr<MacroGrammerNode>(
                (BGParser.Refer(() => MacroGrammerNodeMacro.Parser)
                    .PCapture((MacroGrammerNodeMacro node, StringRange _) => (MacroGrammerNode)node)),
                (BGParser.Refer(() => MacroGrammerNodeConstant.Parser)
                    .PCapture((MacroGrammerNodeConstant node, StringRange _) => (MacroGrammerNode)node))
            ).PAggregate(new MacroGrammerResultAggregate());

    internal sealed class MacroGrammerResultAggregate : IBGParserResultAggregate<MacroGrammerNode, MacroGrammerResult> {
        public MacroGrammerResult Create() {
            return new MacroGrammerResult();
        }

        public MacroGrammerResult Aggregate(MacroGrammerResult result, MacroGrammerNode currentNode, StringRange match) {
            var listNode = result.ListNode;
            if (0 == listNode.Count) {
                listNode = listNode.Add(currentNode);
                return result with { ListNode = listNode };
            } else {
                var lastIndex = listNode.Count - 1;
                if ((currentNode is MacroGrammerNodeConstant nextNodeConstant)
                    && (listNode[lastIndex] is MacroGrammerNodeConstant lastConstant)
                    && (lastConstant.Code.TryCombine(nextNodeConstant.Code, out var combined))) {
                    listNode = listNode.SetItem(lastIndex, new MacroGrammerNodeConstant(combined));
                    return result with { ListNode = listNode };
                }
            }
            listNode = listNode.Add(currentNode);
            return result with { ListNode = listNode };
        }
    }
}

public partial record class MacroGrammerNode() {
    public static readonly IBGParser<ImmutableList<MacroGrammerNode>> ParserContent
        = BGParser.POr<MacroGrammerNode>(
            (BGParser.Refer(() => MacroGrammerNodeMacro.Parser)
                .PCapture((MacroGrammerNodeMacro node, StringRange _) => (MacroGrammerNode)node)),
            (BGParser.Refer(() => MacroGrammerNodeConstant.Parser)
                .PCapture((MacroGrammerNodeConstant node, StringRange _) => (MacroGrammerNode)node))
        ).PAggregate(new MacroGrammerListResultAggregate());

    internal sealed class MacroGrammerListResultAggregate
        : IBGParserResultAggregate<MacroGrammerNode, ImmutableList<MacroGrammerNode>> {
        public ImmutableList<MacroGrammerNode> Create() {
            return ImmutableList<MacroGrammerNode>.Empty;
        }

        public ImmutableList<MacroGrammerNode> Aggregate(ImmutableList<MacroGrammerNode> listNode, MacroGrammerNode currentNode, StringRange match) {
            if (0 == listNode.Count) {
                return listNode.Add(currentNode);
            } else {
                var lastIndex = listNode.Count - 1;
                if ((currentNode is MacroGrammerNodeConstant nextNodeConstant)
                    && (listNode[lastIndex] is MacroGrammerNodeConstant lastConstant)
                    && (lastConstant.Code.TryCombine(nextNodeConstant.Code, out var combined))) {
                    return listNode.SetItem(lastIndex, new MacroGrammerNodeConstant(combined));
                }
            }
            return listNode.Add(currentNode);
        }
    }
}

public partial record class MacroGrammerNodeMacro(
    MacroGrammerNodeMacroStart Start,
    ImmutableArray<MacroGrammerNode> ListContent,
    MacroGrammerNodeMacroEnd End
    ) : MacroGrammerNode() {
    public MacroGrammerNodeMacro() : this(
        Start: new(),
        ListContent: ImmutableArray<MacroGrammerNode>.Empty,
        End: new()) { }

    //public static readonly IBGParser<MacroGrammerNode> ParserAsNode
    //    = BGParser.Refer(() => Parser)
    //        .Capture((MacroGrammerNodeMacro node, StringRange _) => (MacroGrammerNode)node);
    public static readonly IBGParser<MacroGrammerNodeMacro> Parser =
        MacroGrammerNodeMacroStart.Parser
        .Then(MacroGrammerNode.ParserContent)
        .Then(MacroGrammerNodeMacroEnd.Parser)
        .Returns(
            (value1, value2, value3, match) =>
                new MacroGrammerNodeMacro(value1.Value, value2.Value.ToImmutableArray(), value3.Value)
            );
}

public partial record class MacroGrammerNodeMacroStart(
    StringRange Start,
    string Name,
    ImmutableArray<MacroGrammerNodeParameter> ListParameter,
    StringRange End
    ) : MacroGrammerNode() {
    public MacroGrammerNodeMacroStart() : this(
        Start: new(),
        Name: string.Empty,
        ListParameter: ImmutableArray<MacroGrammerNodeParameter>.Empty,
        End: new()) { }

    public static readonly IBGParser<MacroGrammerNodeMacroStart> Parser = default!;
    public static readonly object x =
        MacroGrammer.TokenMultiCommentStart
        .Then(MacroGrammer.TokenIdentifierAsString)
        .Then(MacroGrammer.TokenMultiCommentEnd)
        //Parser()
        ;
}
public partial record class MacroGrammerNodeMacroEnd(
    StringRange Start,
    string Name,
    StringRange End
    ) : MacroGrammerNode() {
    public MacroGrammerNodeMacroEnd() : this(
        Start: new(),
        Name: string.Empty,
        End: new()) { }

    public static readonly IBGParser<MacroGrammerNodeMacroEnd> Parser = default!;
}
public partial record class MacroGrammerNodeParameter(
    string Name,
    string Value
    ) : MacroGrammerNode() {
    public readonly static IBGParser<string> ParserName = MacroGrammer.TokenIdentifierAsString.Parser();
    public readonly static IBGParser<string> ParserValue = MacroGrammer.TokenIdentifierAsString.Parser();
    //.Then(BGTokenizer.AcceptChar(new char[] { '"' }))
    public readonly static IBGParser<MacroGrammerNodeParameter> Parser
        = ParserName
        .Then(BGTokenizer.AcceptChar(['=']).Parser())
        .Then(ParserValue)
        .Returns(
            static (name, _, value, _) =>
                new MacroGrammerNodeParameter(name.Value, value.Value))
        ;
    public readonly static IBGParser<ImmutableList<MacroGrammerNodeParameter>> ParserListParameter
        = MacroGrammerNodeParameter.Parser.PRepeat<MacroGrammerNodeParameter, ImmutableList<MacroGrammerNodeParameter>>(
            0,
            10,
            new BGParserResultRepeatDelegate<MacroGrammerNodeParameter, ImmutableList<MacroGrammerNodeParameter>>(
                (list, _) => ImmutableList<MacroGrammerNodeParameter>.Empty.AddRange(list.Select(static i => i.Value))
                ));

}

public partial record class MacroGrammerNodeConstant(
    StringRange Code
    ) : MacroGrammerNode() {

    public static readonly IBGParser<MacroGrammerNodeConstant> Parser
        = BGParser.TokenT<BGVoid, MacroGrammerNodeConstant>(
            BGTokenizer.Or(
                BGTokenizer.ExceptString("/*").TRepeat(0, 100_000),
                BGTokenizer.ExceptEOF()
                ),
            static (r, _) => new MacroGrammerNodeConstant(r.Match)
            )
        ;

    //public static readonly IBGParser<MacroGrammerNode> ParserAsNode
    //    = BGParser.Refer(() => Parser)
    //    .Capture((MacroGrammerNodeConstant node, StringRange _) => (MacroGrammerNode)node);
}
