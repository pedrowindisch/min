using System.Reflection;
using Xunit.Sdk;

namespace Min.Tests.Utils;

public class SamplesDataAttribute(string sampleType) : DataAttribute
{
    private readonly string _path = Path.Combine("Samples", sampleType);

    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        if (File.Exists(_path))
        {
            yield return File.ReadAllLines(_path);
            yield break;
        }

        if (Directory.Exists(_path)) 
        {
            foreach (var file in Directory.EnumerateFiles(_path))
                yield return File.ReadAllLines(file);

            yield break;
        }

        throw new ArgumentException("The file or directory does not exists.");
    }
}