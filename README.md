# VanType
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
After installation the VanType.dll will be added to your project and the folder VanType with the file index.tt (T4 template).
Below you can find an example of a T4 template configuration.

```CSharp
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
    .AddClass<Product>()
    .Generate()
#>
```

VanType will only generate code for non abstract/static classes with public getter properties.
Below you can find an example of the generated TypeScript definition file.

```TypeScript
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

> Please note that you have to build your project before the TypeScript definition file is generated.
> The reason for this is that the T4 template looks in your build output folder for the VanType.dll.

## Configuration

### TypeScript
The class that does all the magic is called TypeScript. This is the starting point.

### Config
When you call the Config method a new configuration is initialized. With this configuration you 
can setup the rest of your configuration.

```CSharp
TypeScript.Config();
```

### Add Class
With the AddClass method your can add a class to the configuration for which a TypeScript interface should be generated.

> Note that VanType only generates interface for your classes and not interface.

```CSharp
TypeScript
    .Config()
    .AddClass<Product>();
```

### Generate
With the generate method the configured TypeScript is generated and returned as string. 
This method should always be call as last method in the chain.

```CSharp
TypeScript
    .Config()
    .AddClass<Product>()
    .Generate();
```

## Wish list
In the near future we would like to support inheritance. 
Currently this isn't taken into account when generating TypeScript.