namespace Brimborium.Text;

public class SubStringTests {
    [Fact]
    public void T1_SubString_Ctor() {
        {
            var sut = new SubString("abc");
            Assert.Equal("abc", sut.Text);
            Assert.Equal("abc", sut.ToString());
        }
    }


    [Fact()]
    public void SubStringTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void SubStringTest1() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void GetSubStringTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void GetSubStringTest1() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void ToStringTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void AsSpanTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void IsNullOrEmptyTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void IsNullOrWhiteSpaceTest() {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact()]
    public void SplitTest() {
        Assert.True(false, "This test needs an implementation");
    }
}
