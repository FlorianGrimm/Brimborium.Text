using BenchmarkDotNet.Running;

/*
dotnet run --configuration Release
dotnet run --configuration Release -- --memory

dotnet run --configuration Release -- --memory --filter StringSliceBench
dotnet run --configuration Release -- --memory --filter *StringSliceTrimStartBench*
*/

namespace Brimborium.Text {
    public class Program {
        public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
