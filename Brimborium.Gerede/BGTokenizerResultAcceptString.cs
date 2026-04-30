using Brimborium.Text;

namespace Brimborium.Gerede;

public sealed class BGTokenizerResultAcceptString : IBGTokenizerResultAccept<string> {
    public string Select(StringRange match) => match.ToString();
}
