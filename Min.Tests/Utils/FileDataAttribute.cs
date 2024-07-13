using System.Reflection;
using Xunit.Sdk;

namespace Min.Tests.Utils;

public class FileDataAttribute(string path) : DataAttribute
{
    private readonly string _path = path;

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
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