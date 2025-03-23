# VanType - DEPRICATED
NOTE: This project has been depricated and archived. There are other solutions like NSwag that do a beter job than this package. Please migrate your code to other solutions.

VanType is a library for generating TypeScript definition files for your .NET models. 
By using this library your client side models will always match with your server side models.

## Requirements
- Visual Studio
- .NET Framework 4.5 or higher

**Optional**
- AutoT4 extension from Bennor McCarthy (this extension runs T4 templates during the build)

## Installation
Install the package via NuGet with the following command.

`Install-Package VanType`

## Basic Setup
After installation the VanType.dll and the file VanType\index.tt (T4 template) will be added to your project.
Below you can find an example of a VanType T4 template configuration.

```csharp
<#@ template debug="false" hostspecific="false" language="C#" #>
<#@ assembly name="$(TargetDir)VanType.dll" #>
<#@ assembly name="$(TargetDir)Example.Models.dll" #>
<#@ import namespace="VanType" #>
<#@ import namespace="Example.Models" #>
<#@ output extension=".d.ts" #>
<#= TypeScript
    .Config()
    .IncludeEnums(true)
    .PrefixClasses(false)
    .PrefixInterfaces(false)
    .OrderPropertiesByName(true)
    .AddType<Product>()
    .Generate()
#>
```

VanType will only generate code for non abstract/static classes with public getter properties.
Below you can find an example of the generated TypeScript definition file.

> Note that you have to build your project before the TypeScript definition file is generated.
> The reason for this is that the T4 template looks in your build output folder for the VanType.dll.

```typescript
export interface Product
{
	id: string;
	inStock: boolean;
	lastUpdated: Date;
	name: string | null;
	price: number;
	status: ProductStatus;
}

export enum ProductStatus
{
	InStock = 0,
	OutOfStock = 1,
}
```

## Configuration

### TypeScript
The class that does all the magic is called TypeScript. This is the starting point.

### Config
When you call the Config method a new configuration is initialized. With this configuration you 
can setup the rest of your configuration.

```csharp
TypeScript.Config();
```

### Add Class
With the AddType method your can add a class to the configuration for which a TypeScript interface should be generated.

> Note that VanType only generates interface for your classes and not classes.

```csharp
TypeScript
    .Config()
    .AddType<Product>();
```

### Generate
With the Generate method the configured TypeScript is generated and returned as string. 
This method should always be call as the last method in the configuration chain.

```csharp
TypeScript
    .Config()
    .AddType<Product>()
    .Generate();
```

### Adding enums
You can add enumerations by calling the AddType method. 
Another option is to call the IncludeEnums method. 
This will add enumerations automatically during generation.

```csharp
TypeScript
    .Config()
    .AddType<ProductStatus>()
    .Generate()
```
Or
```csharp
TypeScript
    .Config()
    .IncludeEnums(true)
    .Generate()
```

### Prefix interface in TypeScript
You can automatically prefix classes and interfaces with the capital "I" during generation via the options
PrefixClasses and PrefixInterfaces. By default this is turned off.

```csharp
TypeScript
    .Config()
    .PrefixClasses(true)
    .PrefixInterfaces(true)
    .AddType<Product>()
    .Generate()
```


### Order properties
By default the properties are ordered alphabetically, 
you can disable this via the property OrderPropertiesByName.

```csharp
TypeScript
    .Config()
    .OrderPropertiesByName(true)
    .AddType<Product>()
    .Generate()
```

### Import types
When you are generating multiple TypeScript definition files you may want to import
types from other modules. You can do this with the Import method. Below you can see an 
example and the output that it will generate.

```csharp
TypeScript
    .Config()
    .Import<Category>("../category")
    .AddType<Product>()
    .Generate()
```

```typescript
import { Category } from '../category';

export interface Product
{
	id: string;
	name: string | null;
	category: Category | null;
}
```

### Add Assembly
When you want to generate TypeScript definitions for all classes in an assembly 
you can do this by calling the AddAssembly method.
This can be the case when you have all your models in a single assembly.

> Note that the supplied class is used to determin the assembly.

```csharp
TypeScript
    .Config()
    .AddAssembly<Product>()
    .Generate()
```
```

> J :heart: K
