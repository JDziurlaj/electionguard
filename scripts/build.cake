using System.Xml.Linq;

var target =  HasArgument("target") ? Argument("target", "Build") : Argument("t", "Build");
var configuration = Argument("configuration", "Release");

var encryptionProj = "../bindings/netstandard/ElectionGuard/ElectionGuard.Encryption/ElectionGuard.Encryption.csproj";
var decryptionProj = "../bindings/netstandard/ElectionGuard/ElectionGuard.Decryption/ElectionGuard.Decryption.csproj";

Task("AssignVersion")
    .Does(() =>
{
    var csprojFile =  Argument("csproj", encryptionProj);
    var csprojFile2 = Argument("decryptcsproj", decryptionProj);
    var newVersion = Argument("newVersion", "");

    if (string.IsNullOrEmpty(newVersion))
    {
        throw new InvalidOperationException("Both csproj and newVersion arguments must be provided.");
    }

    var xdoc = XDocument.Load(csprojFile);
    var ns = xdoc.Root.GetDefaultNamespace();
    var versionElement = xdoc.Descendants(ns + "Version").FirstOrDefault();
    var packageVersionElement = xdoc.Descendants(ns + "PackageVersion").FirstOrDefault();

    if (versionElement == null)
    {
        throw new InvalidOperationException($"No 'Version' element found in the '{csprojFile}'.");
    }

    if (packageVersionElement == null)
    {
        throw new InvalidOperationException($"No 'PackageVersion' element found in the '{csprojFile}'.");
    }

    versionElement.Value = newVersion;
    packageVersionElement.Value = newVersion;
    xdoc.Save(csprojFile);

    var xdoc2 = XDocument.Load(csprojFile2);
    var ns2 = xdoc2.Root.GetDefaultNamespace();
    var versionElement2 = xdoc2.Descendants(ns2 + "Version").FirstOrDefault();
    var packageVersionElement2 = xdoc2.Descendants(ns2 + "PackageVersion").FirstOrDefault();

    if (versionElement2 == null)
    {
        throw new InvalidOperationException($"No 'Version' element found in the '{csprojFile2}'.");
    }

    if (packageVersionElement2 == null)
    {
        throw new InvalidOperationException($"No 'PackageVersion' element found in the '{csprojFile2}'.");
    }

    versionElement2.Value = newVersion;
    packageVersionElement2.Value = newVersion;
    xdoc2.Save(csprojFile2);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
