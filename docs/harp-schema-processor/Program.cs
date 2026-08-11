using System.Dynamic;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

var fileName = args[0];
var yaml = File.ReadAllText(fileName);
var deserializer = new Deserializer();
var parser = new MergingParser(new Parser(new StringReader(yaml)));
dynamic deviceModel = deserializer.Deserialize<ExpandoObject>(parser);

var builder = new StringBuilder();
builder.AppendLine($@"---
uid: Harp.{deviceModel.device}.Device
---

<table>
  <thead>
    <tr><th colspan=""2"">{deviceModel.device}</th></tr>
  </thead>
  <tbody>
    <tr><td>whoAmI</td><td>{deviceModel.whoAmI}</td></tr>
    <tr><td>firmwareVersion</td><td>{deviceModel.firmwareVersion}</td></tr>
    <tr><td>hardwareTargets</td><td>{deviceModel.hardwareTargets}</td></tr>
  </tbody>
</table>

### Registers

| name | address | type | length | access | description | range | interfaceType |
|-|-|-|-|-|-|-|-|");
foreach (var item in deviceModel.registers)
{
    if (item.Value.TryGetValue("visibility", out object visibility) &&
        (string)visibility == "private")
    {
        continue;
    }

    var name = item.Key;
    var register = ExpandoHelper.FromDictionary(item.Value,
        "length",
        "description",
        "minValue",
        "maxValue",
        "defaultValue",
        "payloadSpec",
        "maskType",
        "interfaceType");
    var interfaceType = register.maskType
        ?? register.interfaceType
        ?? (register.payloadSpec != null ? $"{name}Payload" : null);
    var interfaceTypeRef = (string)interfaceType == "EnableFlag"
        ? "Bonsai.Harp.EnableFlag"
        : $"Harp.{deviceModel.device}.{interfaceType}";

    var access = register.access;
    if (access is List<object> accessList)
    {
        access = string.Join(", ", accessList);
    }

    var range = "";
    if (register.minValue != null || register.maxValue != null)
    {
        range = $"[{register.minValue}:{register.maxValue}]";
    }
    if (register.defaultValue != null) range = $"{register.defaultValue} {range}";

    builder.AppendLine(
        $"| [{name}](xref:Harp.{deviceModel.device}.{name}) " + 
        $"| {register.address} " +
        $"| {register.type} " +
        $"| {register.length} " +
        $"| {access} " +
        $"| {register.description} " +
        $"| {range} " +
        (interfaceType != null ? $"| [{interfaceType}](xref:{interfaceTypeRef}) |" : "| |"));
}

var output = builder.ToString();
if (args.Length > 1)
{
    File.WriteAllText(Path.Combine(args[1], $"Harp_{deviceModel.device}_Device.md"), output);
    File.WriteAllText(Path.Combine(args[1], $"Harp_{deviceModel.device}.md"), $@"---
uid: Harp.{deviceModel.device}
---

[!include[RegisterTables](./Harp_{deviceModel.device}_Device.md)]");
}
else Console.WriteLine(output);
