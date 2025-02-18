# openApiDiscriminatorIssue

This sln demonstrates an issue with the Open API generator when using the JsonDerivedType attribute.
It was created using the `webapi` template and making minor modifications to the code.

## C# classes
```csharp
[JsonDerivedType(typeof(Bar), "bar")]
[JsonDerivedType(typeof(Baz), "baz")]
public abstract record Foo;
public record Bar(int Value) : Foo;
public record Baz(string Name) : Foo;
```

## Generated Open API Schema
```yaml
  "components": {
    "schemas": {
      "Foo": {
        "required": [
          "$type"
        ],
        "type": "object",
        "anyOf": [
          {
            "$ref": "#/components/schemas/FooBar"
          },
          {
            "$ref": "#/components/schemas/FooBaz"
          }
        ],
        "discriminator": {
          "propertyName": "$type",
          "mapping": {
            "bar": "#/components/schemas/FooBar",
            "baz": "#/components/schemas/FooBaz"
          }
        }
      },
      "FooBar": {
        "required": [
          "value"
        ],
        "properties": {
          "$type": {
            "enum": [
              "bar"
            ],
            "type": "string"
          },
          "value": {
            "type": "integer",
            "format": "int32"
          }
        }
      },
      "FooBaz": {
        "required": [
          "name"
        ],
        "properties": {
          "$type": {
            "enum": [
              "baz"
            ],
            "type": "string"
          },
          "name": {
            "type": "string"
          }
        }
      }
    }
  }
```

## Issue
In the schema, FooBar and FooBaz have required properties, but critically the discriminator property `$type` is not required,
even though it is required in the base class Foo. This is inconsistent and can cause issues when e.g.
trying to generate TypeScript classes from the Open API schema.