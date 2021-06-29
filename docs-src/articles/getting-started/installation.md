# Installation and Setup
In this section, we'll be showing you where to get the Ladybug libraries and how to include them in your project.

#### Prerequisites

To get started, you'll need to install MonoGame and its dependencies.

* [.NET Core SDK](https://dotnet.microsoft.com/download)
* [Mono Runtime](https://www.mono-project.com/download/stable/)
* [MonoGame](http://monogame.net/downloads)
* [MonoGame MGCB Editor](https://docs.monogame.net/articles/tools/mgcb_editor.html)

## Setting up the Project
Once you have MonoGame and its dependencies installed, you are ready to begin setting up your project.

### Project Structure

MonoGame projects can be set up in various ways, but we will proceed to show you the way we set up our game projects. The resulting project structure is typically as follows:
```
game_name/
| - core/
| - lib/
| - ref/
| - xplatform/
```

* The `core` folder contains the Shared Project which contins the bulk of the game logic. A Shared Project contains code and resource files, but no instruction on how to build these files into an executable. It is used to house all *platform-independent* code in a central location.
* The `lib` folder is used to house any extra libraries used by the project. For example, we keep our Ladybug Pipeline extension library in this folder.
* The `ref` folder is used to house asset source files for the game, including audio projects, psd files, and other "editor-friendly" files that can't be directly consumed by the game. We like this approach because we can keep the files together with the game in the same folder, so it makes source/version control much easier.
* The `xplatform` folder houses a MonoGame project with the cross-platform desktop build target (Desktop using OpenGL). This project has a reference to the Shared Project in `core`, plus instructions on how to build an executable for the DesktopGL target.
* If other platforms aside from DesktopGL are being targeted, there will be an additional folder for each, e.g. `android`, `uwp`, `ios`, et al. These projects would all reference the `core` shared project, plus include any code specific to their targeted platforms.

### Creating the Structure

In order to create this project structure yourself, you'll need to install the MonoGame project templates. This can be done with the command `dotnet new -i MonoGame.Templates.CSharp`. Once you've run this command, you can see all the project templates available to you with `dotnet new`:
```bash
Templates                                         Short Name            Language          Tags                                 
-------------------------------------------------------------------------------------------------------------------------------
Console Application                               console               [C#], F#, VB      Common/Console                       
Class library                                     classlib              [C#], F#, VB      Common/Library                       
MonoGame Android Application                      mgandroid             [C#]              MonoGame                             
MonoGame iPhone/iPad Application                  mgios                 [C#]              MonoGame                             
MonoGame Mac Application                          mgmacos               [C#]              MonoGame                             
MonoGame NetStandard Library                      mgnetstandardlib      [C#]              MonoGame                             
MonoGame Portable Library                         mgpcllib              [C#]              MonoGame                             
MonoGame Shared Project                           mgsharedproj          [C#]              MonoGame                             
MonoGame tvOS Application                         mgtvos                [C#]              MonoGame                             
MonoGame Windows Universal Application            mguwp                 [C#]              MonoGame                             
MonoGame Windows Application                      mgwindows             [C#]              MonoGame                             
MonoGame Pipeline Extension                       mgpipelineext         [C#]              MonoGame                             
MonoGame Cross Platform Desktop Application       mgdesktopgl           [C#]              MonoGame/Game
```
Now that you have the MonoGame templates, you can create your project folder and then within that folder, create the core Shared Project and one or more "Platform Target" Projects:

```bash
mkdir game && cd game
dotnet new mgsharedproj -n core
dotnet new mgdesktopgl -n xplatform
```

Once you have the Shared Project and your platform target folder(s), you can create the `lib` folder and populate it with any external library files you'll need. Other folders can of course be created to suit your needs, but this guide will only assume that you have a Shared Project folder, at least one Platform Target Project folder, and a library folder.

### Bringing In Ladybug
Now that you have your `xplatform` Platform Target Project, we can add Ladybug to its references. There are two ways to do this.

#### Importing the NuGet Package
The easiest way to include Ladybug is to add it through a package. This can be done from the command line as follows:
```bash
dotnet add package WelcomeToMonday.Ladybug
```
This will add a `<PackageReference>` item to `xplatform.csproj` for Ladybug, which will include it when building your project automatically.

#### Using a local Ladybug library file
Just like with the `ladybug-pipeline.dll` file, ladybug can be included explicitly as a dll file if you've built Ladybug from source yourself.

### Hooking it all Together

At this point you should have a Shared Project (we'll use `core` to refer to this), one or more Platform Target Projects (we'll use `xplatform` as our example), and we'll assume for sample purposes that you'll be including `ladybug-pipeline.dll` in your library folder. While all of these are present in your project, they don't have any instructions on how to all work together to create a game.

#### Exporting the Shared Project Files

First, we'll go into the Shared Project, where we're going to tell it which of its files it'll be providing to the Platform Target Projects that will be using it. To do so, open the `core` folder (or whatever you may have titled it) and find the `.projitems` file. This file includes a lot of information on the dependencies of the MonoGame shared project, but what we're interested in is the section towards the middle that reads:

```XML
<ItemGroup>
	<Compile Include="$(MSBuildThisFileDirectory)Game1.cs" />
</ItemGroup>
```

**Note**: If your .projitems file doesn't have this exact line, don't worry. Find any line starting with `<Compile Include=` and that'll work just fine. If it doesn't exist at all, feel free to add a `<Compile Include=>` element surrounded by `<ItemGroup>` tags at the bottom of the document.

This section of the document lists the files that are being "shared" with Platform Target projects. If you're using Visual Studio, it will automatically generate one line like this for *every file in the project*. Luckily, that isn't necessary, as wildcards are accepted.

We like to export every C# file in the Shared Project, so we usually replace this line with the following:

```XML
<ItemGroup>
	<Compile Include="$(MSBuildThisFileDirectory)**\*.cs" />
</ItemGroup>
```

#### Referencing the Shared Project in the Platform Target Project

Now that the Shared Project is sharing its files, we need to show the Platform Target Project where to find the Shared Project. To do this, we need to go into the `xplatform` folder and find the `.csproj` file.

Towards the bottom of the file -- after the last closing `</ItemGroup>` tag but before the final `</Project>` tag, add `<Import Project="..\core\core.projitems" />`.

You will have to repeat this step for each Platform Target Project you have.

#### Referencing Ladybug in the Platform Target Project

While we're working on the `.csproj` file, we can also add the reference to the Ladybug Pipeline extension library. Find the `<ItemGroup>` section with the `<PackageReference>` elements referencing MonoGame (if you can't find this, feel free to create a new ItemGroup section by creating a new set of `<ItemGroup> </ItemGroup>` tags), and add a new line: `<Reference Include="..\lib\ladybug-pipeline.dll">`. If you're using multiple external libraries, add one of these lines for each .dll file you have in `lib`.

You will have to repeat this step for each Platform Target Project you have.

### Wrapping Up

With that, you should be all set to get started with Ladybug and MonoGame. In the [next section](/ladybug/articles/getting-started/setting-up-the-game.html), we'll go over setting up your first Ladybug Scene!