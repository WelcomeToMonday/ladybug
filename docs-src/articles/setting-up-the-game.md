

# Setting Up the Game
In this section, we'll be going over Ladybug's `Game` and `Scene` classes, and how you will use them to get your game up and running.

## Ladybug's Game Class
Ladybug is built upon Monogame/XNA, and at its most basic level runs on the same underlying `Game` class.

Where Ladybug's `Game` class differs is with the concept of `Scenes` -- independent game loops that can run concurrently with one another.

## What is a Scene?
A Scene is an independent game loop that handles frame-by-frame updates to logic and drawing. In essence, it is a logical re-implmentation of MonoGame/XNA's `Game` class that provides extra functionality and support for running multiple game loops concurrently.

Having multiple active scenes allows you to have simpler, more focused Update and Draw loops, each concerned with one self-contained part of the game.

Furthermore, scenes can be paused, resumed, and switched between on the fly. They can share common resources, and transfer state through the root `Game` object which maintains them.

Check out the API Documentation for [Scenes](/ladybug/api/Ladybug.Scene.html) and [Ladybug's Game class](/ladybug/api/Ladybug.Game) for more information.

### Components of a Scene
A Scene has four main overridable Methods:

* `LoadContent()`, where assets are loaded in for use in the Scene
* `Initalize()`, where variables and properties are set to their initial values
* `Update()`, in which game objects are updated every frame *
* `Draw()`, in which all drawing logic is handled *

\* *It is technically possible to update game objects in `Draw()` and use drawing logic in `Update()`, but it is considered bad practice to do so.*

### Loading, Unloading, and Switching Scenes
You can load a Scene into a Game by calling `LoadScene()` and passing it a new Scene object. Once loaded, a Scene is immediately initialized and set to the `ACTIVE` state, where it will have its `Update()` and `Draw()` methods called every frame.

Similarly, you can unload a Scene by calling `UnloadScene()` on the Game and passing it the Scene you wish to Unload. Once Unloaded, a Scene is effectively destroyed, and would have to be Loaded again to be reactivated.

### Managing a Scene's State
Scenes have a range of states they can be in at any one time (determined by the `Scene.State` enum)
* `ACTIVE`: Both `Update()` and `Draw()` are called every frame.
* `PAUSED`: `Draw()` is called every frame, but `Update()` is not.
* `SUSPENDED`: Neither `Update()` nor `Draw()` is called.

A scene's state can be changed through the `Game` via methods like `PauseScene()`. It can also be changed by directly altering the `Scene.State` variable from within the Scene, but this is generally not the recommended way to accomplish it.

Scenes have overridable methods that are called when their state is changed through the `Game`, which assist with transitioning between scene states. For example, to make background music stop when your main scene is paused, you can stop the music in the `Pause()` method, and resume it in the `Unpause()` method.

### Making a Scene

To create a Scene, simply create a new `.cs` file containing a class derived from `Ladybug.Scene`. At bare minimum, you will want to override the `LoadContent()`, `Initialize()`, `Update()`, and `Draw()` methods. You will also have to define the Scene's constructor, though in most cases you will leave the constructor's body empty:

```csharp
using Ladybug;

public class MainScene : Scene
{
	public MainScene(Game game) : base(game)
	{
		// Leave empty, unless you need custom constructor logic (rare)
	}

	public override void LoadContent()
	{
		// Load your content here
	}

	public override void Initialize()
	{
		// Initialize your variables here
	}

	public override void Update(GameTime gameTime)
	{
		// Add game loop update logic here
	}

	public override void Draw(GameTime gameTime)
	{
		// Add drawing logic here
	}
}
```

If you're using the Shared Project/Platform Target Project project structure, you will be creating your scenes in the Shared Project (usually in a Scene folder, but anywhere in the Shared Project could work), unless the scene contains platform-specific code. See [Installation and Setup](/ladybug/articles/installation.html) for a project structure example.

### Using the Game class to Manage Scenes
In order for your Scenes to run, you will need to load them into a `Game` instance -- and in order to load them into a `Game` instance, you will need to instantiate one.
A new `Game` instance is usually created in the `Program.cs` file (found in your Platform Target Project folder(s)), in the `Main` method. Instantiating a `Game`, loading a Scene into it, then calling `Run()` is usually the entry point into a Ladybug Scene-managed game.

Here is a sample `Program.cs` file using Ladybug Scene Management:
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
			game.LoadScene(new MainScene(game));
			game.Run()
		}
	}
}
```

You should now see your scene running -- assuming you've added assets and drawing logic to your scene!

### Wrapping Up
Now that you have a basic grasp of Ladybug Scene Management, you're ready to create some games!

Here's where you can go from here:

* Check out [our next article](#wrapping-up) (coming soon) for a quick tutorial on making a simple snake game from start to finish, using more of Ladybug's features.
* Look through the excellent MonoGame/XNA Tutorials on [R.B. Whitaker's Wiki](http://rbwhitaker.wikidot.com/monogame-tutorials)
* Take a look at the [MonoGame Documentation](http://www.monogame.net/documentation/?page=main) for more tutorials.