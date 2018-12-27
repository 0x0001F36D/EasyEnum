# EasyEnum
Defines dynamically enum type with string or ValueTuple at runtime.

# How to
```csharp
dynamic @enum = new Enum(name: "PermissionKinds")
{
  "None",
  ("Normal", 1 << 1),
  ("Admin", 1 << 2),
  ("Root", 1 << 3)
};

var dynamic_admin = @enum.Admin; 
var admin = @enum.Admin as Enum.Field;

var v = @enum.None | @enum.Normal; // output: None,Normal
var k = (int)v; //output: 2
```

# License
Apache 2.0