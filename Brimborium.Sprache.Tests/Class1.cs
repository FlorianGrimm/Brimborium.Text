namespace Brimborium.Sprache.Tests {
    public class Class1 {
        [Test]
        public async Task Test001() {
            var a = 1;
            await Assert.That(a).IsEqualTo(1);
        }
    }
}
