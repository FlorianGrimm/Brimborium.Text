using System.Text;

namespace Brimborium.Text;
public class StringSpliceTests {
    [Fact()]
    public void StringSpliceTest() {
        { 
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new StringSplice(new SubString("abc"));
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new StringSplice(new SubString("abc"), 1, 1);
            Assert.Equal("b", sut.ToString());
        }
        {
            var sut = new StringSplice(new SubString("abc"), 1..3);
            Assert.Equal("bc", sut.ToString());
        }
    }
    
    [Fact()]
    public void AsSubStringTest() {
        {
            var sut = new StringSplice(new SubString("abcdef", 1..3));
            var act = sut.AsSubString();
            Assert.Equal("bc", act.ToString());
        }
    }

    [Fact()]
    public void GetTextTest() {
        {
            var sut = new StringSplice(new SubString("abcdef", 1..3));
            Assert.Equal("bc", sut.GetText());
        }
    }

    [Fact()]
    public void SetGetReplacementTextTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.Equal("b", part.ToString());
            
            part.SetReplacementText("B");
            Assert.Equal("B", part.GetReplacementText());
            Assert.Equal("b", part.ToString());

            Assert.Equal("aBc", sut.BuildReplacement());
            Assert.Equal("aBc", sut.ToString());
        }
    }

    [Fact()]
    public void SetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var sb = new StringBuilder();
            part.SetReplacementBuilder(sb);
            Assert.Same(sb, part.GetReplacementBuilder());
            sb.Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            part.SetReplacementText("BBB");
            Assert.Throws<InvalidOperationException>(() => part.GetReplacementBuilder());
        }
    }

    [Fact()]
    public void GetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            Assert.Throws<InvalidOperationException>(() => part.SetReplacementText("BBB"));
        }
    }

    [Fact()]
    public void GetArrayPartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var part2 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            var part3 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            Assert.NotNull(sut.GetArrayPart());
            Assert.Same(part0, sut.GetArrayPart()![0]);
            Assert.Same(part1, sut.GetArrayPart()![1]);
            Assert.Same(part2, sut.GetArrayPart()![2]);
            Assert.Same(part3, sut.GetArrayPart()![3]);
        }
    }

    [Fact()]
    public void IsRangeValidTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal(true, sut.IsRangeValid(1, 2));
            Assert.Equal(false, sut.IsRangeValid(1, 42));
        }
    }

    [Fact()]
    public void CreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
        }
        {
            // overlap
            var sut = new StringSplice("abc");
            Assert.NotNull(sut.CreatePart(0..2));
            Assert.Null(sut.CreatePart(1..2));
        }

        {
            // append for zero length
            var sut = new StringSplice("abc");
            Assert.NotNull(sut.CreatePart(0..2));
            Assert.NotNull(sut.CreatePart(2..2));
            Assert.NotNull(sut.CreatePart(2..2));
            Assert.Equal(3, sut.GetArrayPart()!.Length);
        }
    }


    [Fact()]
    public void GetOrCreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.GetOrCreatePart(0,1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1,2) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(0,1) ?? throw new InvalidOperationException();
            Assert.NotSame(part0, part1);
            Assert.Same(part0, part2);
            Assert.NotSame(part1, part2);
        }
    }

    [Fact()]
    public void GetLstPartInRangeTest() {
        {
            var sut = new StringSplice("abcdef");
            var part0 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1, 1) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(2, 1) ?? throw new InvalidOperationException();
            var part4 = sut.GetOrCreatePart(3, 1) ?? throw new InvalidOperationException();
            var act = sut.GetLstPartInRange(1, 2).ToList();
            Assert.Equal(2, act.Count);
            Assert.Same(part1, act[0]);
            Assert.Same(part2, act[1]);
        }
    }

    [Fact()]
    public void BuildReplacementTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.Equal("ac", sut.ToString());
            
            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());

            part.GetReplacementBuilder().Append("bBB");
            Assert.Equal("aBBbBBc", sut.BuildReplacement());
        }

        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            
            Assert.Equal("ac", sut.BuildReplacement());
        }
    }

    [Fact()]
    public void BuildReplacementStringBuilderTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            var sb = new StringBuilder();
            sut.BuildReplacementStringBuilder(sb);
            Assert.Equal("aBBc", sb.ToString());
            Assert.Equal("aBBc", sut.ToString());
        }
    }

        [Fact]
    public void T001CreatePartUp() {
        var sut = new StringSplice("Hello World");

        var part22 = sut.CreatePart(2, 2);
        Assert.Equal("ll", part22?.GetText());

        var part81 = sut.CreatePart(8, 1);
        Assert.Equal("r", part81?.GetText());

        Assert.Equal("2..4;8..9", string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }
    [Fact]
    public void T002CreatePartDown() {
        var sut = new StringSplice("Hello World");

        var part81 = sut.CreatePart(8, 1);
        Assert.Equal("r", part81?.GetText());

        var part22 = sut.CreatePart(2, 2);
        Assert.Equal("ll", part22?.GetText());

        Assert.Equal("2..4;8..9", string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }

    [Fact]
    public void T003CreatePartUp() {
        var sut = new StringSplice("Hello World");
        var act = new List<StringSplice>();
        foreach (int start in new int[] { 0, 8, 6, 4 }) {
            var p = sut.CreatePart(start, 2);
            if (p is null) throw new Exception("p is null");
            act.Add(p);
        }
        Assert.Equal("0..2;4..6;6..8;8..10", string.Join(";", sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
        Assert.Equal("Heo Worl", string.Join("", sut.GetArrayPart()?.Select(item => item.GetText())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }

    [Fact]
    public void T004CreatePartOverlapUp() {
        var sut = new StringSplice("Hello World");

        Assert.NotNull(sut.CreatePart(4, 4));
        Assert.Null(sut.CreatePart(4, 4));
        Assert.Null(sut.CreatePart(2, 4));
    }

    [Fact]
    public void T005CreatePartLength() {
        var sut = new StringSplice("Hello World");
        Assert.Null(sut.CreatePart(4, 10));
    }

    [Fact]
    public void T006GetOrCreatePart() {
        var sut = new StringSplice("Hello World");
        var a = sut.GetOrCreatePart(4, 4);
        Assert.NotNull(a);
        Assert.Equal(4, a.Range.Start);
        Assert.Equal(8, a.Range.End);
        Assert.Equal(4, a.Length);

        var b = sut.GetOrCreatePart(4, 4);
        Assert.NotNull(b);

        Assert.Same(a, b);

        var c = sut.GetOrCreatePart(4, 40);
        Assert.Null(c);

    }



    [Fact]
    public void T011BuildReplacement() {
        var sut = new StringSplice("Hello World");
        var part = sut.CreatePart(1, 1);
        if (part is null) { throw new Exception("part is null"); }
        part.GetReplacementBuilder().Append("a");
        Assert.Equal("Hallo World", sut.BuildReplacement());
        var part2 = sut.CreatePart(sut.Length, 0);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.GetReplacementBuilder().Append("!");
        Assert.Equal("Hallo World!", sut.BuildReplacement());
    }

    [Fact]
    public void T012BuildReplacement() {
        var sut = new StringSplice("123BC");
        //var sutRange = new Range(1, new Index(1, true),
        var part1 = sut.CreatePart(1..^1);
        if (part1 is null) { throw new Exception("part is null"); }
        Assert.Equal("23B", part1.GetText());

        var part2 = part1.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        Assert.Equal("2AB", part1.BuildReplacement());
        Assert.Equal("12ABC", sut.BuildReplacement());
    }

    [Fact]
    public void T013BuildReplacement() {
        var sut = new StringSplice("123BC");

        var part = new StringSplice(sut.AsSubString(), 1..^1);
        if (part is null) { throw new Exception("part is null"); }
        Assert.Equal("23B", part.GetText());

        var part2 = part.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        Assert.Equal("2AB", part.BuildReplacement());

        Assert.Equal("123BC", sut.BuildReplacement());
    }

    [Fact]
    public void T014BuildReplacement() {
        var sut = new StringSplice("15");

        sut.CreatePart(1, 0)?.SetReplacementText("2");
        sut.CreatePart(1, 0)?.SetReplacementText("3");
        sut.CreatePart(1, 0)?.SetReplacementText("4");

        Assert.Equal("12345", sut.BuildReplacement());
    }
}
