

# Setting Up the Game
In this section, we'll be going over Ladybug's `Game` and `Scene` classes, and how you will use them to get your game up and running.

#### Prerequisites
To follow this article, you will need to have created a new project that references both MonoGame and Ladybug.

See the [previous article](/ladybug/articles/getting-started/installation.html) for guidance on setting up a new Ladybug project.

## Ladybug's Game Class
Ladybug is built upon Monogame/XNA, and at its most basic level runs on the same underlying `Game` class.

Where Ladybug's `Game` class differs is with the concept of Scenes -- independent game loops that can manage their own resources and lifecycles, and can run concurrently with one another.

### Creating a New Game Instance
To get started with Ladybug, the first thing you will need to create is a `Game` instance. The `Game` instance has many responsibilities, but its biggest job is handling Scenes, which hold the bulk of your game's logic and behavior.
To create a `Game` instance, instantiate one in your program's `Main` method as follows:

```csharp
using System;
using Ladybug;

public static class Program
{
	[STAThread]
	static void Main()
	{
		using (var game = new Game())
		{
			game.Run();
		}
	}
}
```

The above code will run as-is, but will produce only an empty window. To build a game that is playable, we'll need to begin creating Scenes.

## What is a Scene?
A `Scene` is an independent game loop that handles its own load/run/unload lifecycle, manages its own resources, and manages frame-by-frame updates to logic and drawing. In essence, it is a logical re-implmentation of MonoGame/XNA's `Game` class that provides extra functionality.

Having multiple active Scenes allows you to have simpler, more focused Update and Draw loops and more well-defined resource ownership, allowing each Scene to only be concerned with one self-contained part of the game.

Furthermore, Scenes can be paused, resumed, and switched between on the fly. They can share common resources, and transfer state through the root `Game` object which maintains them.

### Components of a Scene
A `Scene` contains a wide array of methods that allow you to define its behavior throughout its lifecycle. The four primary methods that should be defined in every Scene are as follows:

* `OnLoadContent()`, where assets are loaded in for use in the `Scene`
* `OnInitalize()`, where variables and properties are set to their initial values
* `OnUpdate()`, in which game objects are updated every frame
* `OnDraw()`, in which all drawing logic is handled

Other methods exist that allow you to further define behavior at different points in the lifecycle, such as when the Scene's Active/Paused/Suspended state changes, and when it is unloaded and removed from the `Game` instance that is managing it.
For a full reference of these methods, check out the [Ladybug.Scene documentation](/ladybug/api/Ladybug.Scene.html).

### Loading and Unloading Scenes
You can load a `Scene` into a `Game` instance by calling its `LoadScene()` method and passing it a new `Scene` object. Once loaded, a Scene is immediately initialized and set to the `ACTIVE` state, where it will have its `OnUpdate()` and `OnDraw()` methods called every frame.

To unload a `Scene`, you can call `Unload()` on the Scene instance. Once Unloaded, a `Scene` is effectively destroyed, and would have to be Loaded again to be reactivated.

### Managing a Scene's State
Scenes have a range of states they can be in at any one time (determined by the `Scene.State` enum)
* `ACTIVE`: Both `OnUpdate()` and `OnDraw()` are called every frame.
* `PAUSED`: `OnDraw()` is called every frame, but `OnUpdate()` is not.
* `SUSPENDED`: Neither `OnUpdate()` nor `OnDraw()` is called.

A Scene's state can be changed through the `Pause()`, `Unpause()`, `Suspend()`, and `Unsuspend()` methods.

Further, a Scene's behavior upon changing state can be defined through the `Paused`, `Unpaused`, `Suspended`, `Unsuspended`, `Stopped`, and `Resumed` events, as well as through the `OnPause()`, `OnUnpause()`, `OnSuspend()`, `OnUnsuspend()`, `OnStop()`, and `OnResume()` methods.

### Making a Scene

To create a `Scene`, simply instantiate a new `Scene` object using `Scene.Compose()` and use its `On*()` methods to define its behavior. This can be done anywhere in your project, but for sample purposes, we'll show you what this would look like if we were to create it right where we set up our `Game` instance.

```csharp
// Instantiate a new Game instance, called game.
using (var game = new Game())
{
	// Instantiate a new Scene instance, called scene.
	var scene = Scene.Compose(game)
		.OnLoadContent(() =>
		{
			// Content resources are loaded here
			// -or-
			// define a LoadContent() method elsewhere
			// and replace this with OnLoadContent(LoadContent)
		})
		.OnInitialize(() =>
		{
			// Initialization logic here.
			// -or-
			// define an Initialize() method elsewhere
			// and replace this with OnInitialize(Initialize)
		})
		.OnUpdate((GameTime gameTime) =>
		{
			// Update logic here
			// -or-
			// define an Update(GameTime) method elsewhere
			// and replace this with OnUpdate(Update)
		})
		.OnDraw((GameTime gameTime) =>
		{
			// Draw logic here
			// -or-
			// define a Draw(GameTime) method elsewhere
			// and replace this with OnDraw(Draw)
		});

	// Load the new Scene into the Game instance.
	// At this point, scene.OnLoadContent() and scene.OnInitialize() are called.
	game.LoadScene(scene);

	// Begin game loop execution.
	// At this point, scene.OnUpdate() and scene.OnDraw() are called
	// every frame, as are the OnUpdate() and OnDraw() methods of any other
	// Scenes loaded into this Game instance.
	game.Run();
}

```

_If you're using the Shared Project/Platform Target Project project structure, you will be creating your scenes in the Shared Project, unless the scene contains platform-specific code. See [Installation and Setup](/ladybug/articles/installation.html) for a project structure example._

## Putting it All Together
Now that we've reviewed instantiating a new `Game` instance, creating a `Scene` instance, we'll walk through a quick example of using both together.

We'll create a simple game that prints `Hello World` to the console and then closes itself.

### A Review: The Main Method
The Main Method is the entry point to our game application, so we will need to instantiate the `Game` class there and load in the initial `Scene`, which we will define in the next step.
```csharp
static void Main()
{
	using (var game = new Game())
	{
		// todo: load scene once we've created it
		// game.LoadScene(mainScene)
		
		game.Run();
	}
}
```
As mentioned before, this code will run but won't produce anything interesting.

### Creating the Main Scene
Although we can create a scene right within the `using` block in `Main()`, we'll explore creating the `Scene` in a new method in the `Program` class for this example. We'll call this method `CreateMainScene()`.
```csharp
public Scene CreateMainScene(Game game)
{
	var mainScene = Scene.Compose(game)
		.OnUpdate((GameTime gameTime) =>
		{
			Console.WriteLine("Hello World");
			game.Exit();
		});
	return mainScene;
}
```

### Loading the Main Scene
Now that we've created the method that will be defining our Scene's behavior, we can load it into the `Game` instance.
```csharp
static void Main()
{
	// Creates the new Game instance
	using (var game = new Game())
	{
		// Creates the new Scene instance
		var mainScene = CreateMainScene(game);
		
		// Loads the Scene instance into the Game instance
		// OnLoadContent() and OnInitialize() are called here
		game.LoadScene(mainScene)

		// Begins the main game loop
		// OnUpdate() and OnDraw() are called here every frame
		game.Run();
	}
}
```
Running the code should now produce a single `Hello World` in the output console. With that, you've successfully created a (very) basic game application in Ladybug!

## Wrapping Up
Now that you have a basic grasp of Ladybug's `Game` and `Scene` classes, you're ready to create some games!

Here's where you can go from here:

* Check out the [Learning Ladybug with Snake](/ladybug/articles/tutorials/1/intro.html) tutorial and walk through re-creating the classic game of Snake with Ladybug
* Look through the excellent MonoGame/XNA Tutorials on [R.B. Whitaker's Wiki](http://rbwhitaker.wikidot.com/monogame-tutorials)
* Take a look at the [MonoGame Documentation](http://www.monogame.net/documentation/?page=main) for more tutorials