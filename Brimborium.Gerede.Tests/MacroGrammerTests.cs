#pragma warning disable IDE0301 // Simplify collection initialization
#pragma warning disable IDE0350 // Use implicitly typed lambda
#pragma warning disable IDE0305 // Simplify collection initialization

using Brimborium.Text;

using System.Collections.Immutable;

namespace Brimborium.Gerede.Tests;

public class MacroGrammerTests {
    private const string InputText1
        = """
        /* #Macro:abc */
        def
        /* #EndMacro:abc */
        """;
    [Test]
    public async Task Start1Test() {
        BGParserInput input = new BGParserInput(
            new StringRange(InputText1),
            new());
        var success = MacroGrammerNodeMacroStart.Parser.Parse(input, out var match, out var error, out var next);
        await Assert.That(success).IsTrue();
        await Assert.That(next.Input.Start).IsEqualTo(16);
    }

    [Test]
    public async Task Content1Test() {
        BGParserInput input = new BGParserInput(
            new StringRange(InputText1).Substring(16),
            new());
        var success = MacroGrammerNodeConstant.Parser.Parse(input, out var match, out var error, out var next);
        await Assert.That(success).IsTrue();
        await Assert.That(next.Input.Start).IsEqualTo(23);
    }

    [Test]
    public async Task End1Test() {
        BGParserInput input = new BGParserInput(
            new StringRange(InputText1).Substring(23),
            new());
        var success = MacroGrammerNodeMacroEnd.Parser.Parse(input, out var match, out var error, out var next);
        await Assert.That(success).IsTrue();
        await Assert.That(next.Input.Start).IsEqualTo(42);
    }


    [Test]
    public async Task CompleteTest() {
        BGParserInput input = new BGParserInput(
            new StringRange(InputText1),
            new());
        var success = MacroGrammer.Parser.Parse(input, out var match, out var error, out var next);
        await Assert.That(success).IsTrue();
        await Assert.That(next.Input.IsEmpty).IsTrue();
        //await Assert.That(match.Value.ListNode[0].).IsTrue();
    }
}

#pragma warning disable CA2211 // Non-constant fields should not be visible

public partial class MacroGrammer {
    public static IBGParser<MacroGrammerResult> Parser
        = BGParser.Refer(() => MacroGrammerResult.Parser)
        .Then(BGParser.Token(BGTokenizer.AcceptEOF()))
        .Returns1();

    public static BGGrammer<MacroGrammerResult> Grammer = new BGGrammer<MacroGrammerResult>(Parser);

    public static IBGTokenizer<BGVoid> TokenWhitespaceOptional = BGTokenizer.AcceptChar(" /t\r\n").TRepeat(0, 1024);
    public static IBGTokenizer<BGVoid> TokenWhitespaceMust = BGTokenizer.AcceptChar(" /t\r\n").TRepeat(1, 1024);
    public static IBGTokenizer<string> TokenMultiCommentStart =
        (BGTokenizer.AcceptChar("/")
        .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
        .Then(TokenWhitespaceOptional)
        .Returns(
            static (value1, value2, value3, match)
                => match.Substring(0, value2.Match.End - value1.Match.Start).ToString()
            ));

    public static IBGTokenizer<BGVoid> TokenPrefixMacro = BGTokenizer.AcceptString("#Macro:");
    public static IBGTokenizer<BGVoid> TokenPrefixEndMacro = BGTokenizer.AcceptString("#EndMacro:");

    public static IBGTokenizer<BGVoid> TokenIdentifierStart = BGTokenizer.AcceptChar(
        BGChar.CRange('A', 'Z') + BGChar.CRange('a', 'z'));
    public static IBGTokenizer<BGVoid> TokenIdentifierRest = BGTokenizer.AcceptChar(
        BGChar.CList("_") + BGChar.CRange('A', 'Z') + BGChar.CRange('a', 'z') + BGChar.CRange('0', '9'));

    public static IBGTokenizer<BGVoid> TokenIdentifier
        = TokenIdentifierStart
            .Then(TokenIdentifierRest.TRepeat(0, 255))
            .Returns();

    public static IBGTokenizer<string> TokenIdentifierAsString
        = TokenIdentifierStart
            .Then(TokenIdentifierRest.TRepeat(0, 255))
            .Returns(static (_, _, match) => match.ToString());

    public static IBGTokenizer<string> TokenMultiCommentEnd =
        TokenWhitespaceOptional
        .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
        .Then(BGTokenizer.AcceptChar("/"))
        .Returns(
                static (value1, value2, value3, match)
                => value2.Match.Combine(value3.Match).ToString()
            );

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
            var lastIndex = listNode.Count - 1;
            if ((0 <= lastIndex)
                && ((currentNode is MacroGrammerNodeConstant nextNodeConstant)
                && (listNode[lastIndex] is MacroGrammerNodeConstant lastConstant)
                && (lastConstant.Code.TryCombine(nextNodeConstant.Code, out var combined)))) {
                return result with {
                    ListNode = listNode.SetItem(lastIndex, new MacroGrammerNodeConstant(combined))
                };
            }
            return result with {
                ListNode = listNode.Add(currentNode)
            };
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
            var lastIndex = listNode.Count - 1;
            if ((0 <= lastIndex)
                && (currentNode is MacroGrammerNodeConstant nextNodeConstant)
                && (listNode[lastIndex] is MacroGrammerNodeConstant lastConstant)
                && (lastConstant.Code.TryCombine(nextNodeConstant.Code, out var combined))) {
                return listNode.SetItem(lastIndex, new MacroGrammerNodeConstant(combined));
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
                new MacroGrammerNodeMacro(
                    Start: value1.Value,
                    ListContent: value2.Value.ToImmutableArray(),
                    End: value3.Value)
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

    public static readonly IBGParser<MacroGrammerNodeMacroStart> Parser =
        MacroGrammer.TokenMultiCommentStart
        .Then(MacroGrammer.TokenPrefixMacro)
        .Then(MacroGrammer.TokenIdentifierAsString)
        .Then(MacroGrammer.TokenMultiCommentEnd)
        .ReturnsTupple()
        .Parser(
            (token, _) => new MacroGrammerNodeMacroStart(
            Start: token.Value.Value1.Match,
            Name: token.Value.Value3.Value,
            ListParameter: ImmutableArray<MacroGrammerNodeParameter>.Empty,
            End: token.Value.Value4.Match))
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

    public static readonly IBGParser<MacroGrammerNodeMacroEnd> Parser
        = MacroGrammer.TokenMultiCommentStart
        .Then(MacroGrammer.TokenPrefixEndMacro)
        .Then(MacroGrammer.TokenIdentifierAsString)
        .Then(MacroGrammer.TokenMultiCommentEnd)
        .ReturnsTupple()
        .Parser(
            (token, _) => new MacroGrammerNodeMacroEnd(
            Start: token.Value.Value1.Match,
            Name: token.Value.Value3.Value,
            End: token.Value.Value4.Match))
        ;
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

    public readonly static IBGParser<ImmutableList<MacroGrammerNodeParameter>> ParserListParameter =
        MacroGrammerNodeParameter.Parser
        .PList(
            parserSplit: MacroGrammer.TokenWhitespaceMust.Parser(),
            minRepeat: 0,
            maxRepeat: 0,
            create: () => ImmutableList<MacroGrammerNodeParameter>.Empty,
            aggregate: (list, node, _) => list.Add(node)
        );
}

public partial record class MacroGrammerNodeConstant(
    StringRange Code
    ) : MacroGrammerNode() {
    public static readonly IBGParser<MacroGrammerNodeConstant> Parser
    = BGParser.TokenT<BGVoid, MacroGrammerNodeConstant>(
        BGTokenizer.ExceptString("/*").TRepeat(1, 100_000),
        static (r, _) => new MacroGrammerNodeConstant(r.Match)
        )
    ;

    //public static readonly IBGParser<MacroGrammerNodeConstant> Parser
    //    = BGParser.TokenT<BGVoid, MacroGrammerNodeConstant>(
    //        BGTokenizer.Or(
    //            BGTokenizer.ExceptString("/*").TRepeat(0, 100_000),
    //            BGTokenizer.ExceptEOF()
    //            ),
    //        static (r, _) => new MacroGrammerNodeConstant(r.Match)
    //        )
    //    ;

    //public static readonly IBGParser<MacroGrammerNode> ParserAsNode
    //    = BGParser.Refer(() => Parser)
    //    .Capture((MacroGrammerNodeConstant node, StringRange _) => (MacroGrammerNode)node);
}
