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
}
