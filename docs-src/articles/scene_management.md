# Creating and Managing Scenes
In this section, we'll be going over what a Scene is in Ladybug and how to manage creating, loading, pausing, and switching between them.

**Important**: *This article assumes your project is making use of Ladybug's Scene Management tools. This requires your project to either be using Ladybug Core, or the Ladybug Scene Management Module.*

## What is a Scene?
In essence, a Scene is a re-implmentation of MonoGame/XNA's `Game` class. It includes many of the same methods, and serves the same purpose -- to provide a game loop where game objects are updated and drawn every frame.

Where a Ladybug Scene differs is that it is built with the presence of *multiple, concurrent* Scenes in mind. This is accomplished via a Scene Manager, which handles updating and drawing multiple scenes concurrently. Having multiple active scenes allows you to have simpler, more focused Update and Draw loops, each concerned with one self-contained part of the game.

With the presence of a Scene Manager, scenes can be paused, resumed, and switched between on the fly. They can share common resources, and transfer state through the `SceneManager` object which maintains them.

Check out the API Documentation for [Scenes](/ladybug/api/Ladybug.SceneManagement.Scene.html) and [SceneManagers](/ladybug/api/Ladybug.SceneManagement.SceneManager.html) for more information.

### Components of a Scene
A Scene has four main overridable Methods:

* `LoadContent()`, where assets are loaded in for use in the Scene
* `Initalize()`, where variables and properties are set to their initial values
* `Update()`, in which game objects are updated every frame *
* `Draw()`, in which all drawing logic is handled *

\* *It is technically possible to update game objects in `Draw()` and use drawing logic in `Update()`, but it is considered bad practice to do so.*

### Loading, Unloading, and Switching Scenes
You can load a Scene into a SceneManager by calling `LoadScene()` and passing it a new Scene object. Once loaded, a Scene is immediately initialized and set to the `ACTIVE` state, where it will have its `Update()` and `Draw()` methods called every frame.

Similarly, you can unload a Scene by calling `UnloadScene()` on the SceneManager and passing it the Scene you wish to Unload. Once Unloaded, a Scene is effectively destroyed, and would have to be Loaded again to be reactivated.

### Managing a Scene's State
Scenes have a range of states they can be in at any one time (determined by the `Scene.State` enum)
* `ACTIVE`: Both `Update()` and `Draw()` are called every frame.
* `PAUSED`: `Draw()` is called every frame, but `Update()` is not.
* `SUSPENDED`: Neither `Update()` nor `Draw()` is called.

A scene's state can be changed through the SceneManager via methods like `PauseScene()`. It can also be changed by directly altering the `Scene.State` variable from within the Scene, but this is generally not the recommended way to accomplish it.

Scenes have overridable methods that are called when their state is changed through the SceneManager, which assist with transitioning between scene states. For example, to make background music stop when your main scene is paused, you can stop the music in the `Pause()` method, and resume it in the `Unpause()` method.

## Using Scenes in your Project
The Scene is the most basic unit of a Ladybug Scene-managed game. Every game has at least one, so in this section we'll go over creating your first scene.

### Creating a Scene

To create a Scene, simply create a new `.cs` file containing a class derived from `Ladybug.Scene`. At bare minimum, you will want to override the `LoadContent()`, `Initialize()`, `Update()`, and `Draw()` methods. You will also have to define the Scene's constructor, though in most cases you will leave the constructor's body empty:

```csharp
using Microsoft.Xna.Framework;

using Ladybug.SceneManagement;

public class MainScene : Scene
{
	public MainScene(SceneManager sceneManager) : base(sceneManager)
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

### Using the SceneManager
In order for your Scenes to run, you will need to load them into a `SceneManager` -- and in order to load them into a SceneManager, you will need to instantiate one.
A SceneManager is usually created in the `Program.cs` file (found in your Platform Target Project folder(s)), in the `Main` method. Instantiating a SceneManager, loading a Scene into it, then calling `Run()` is usually the entry point into a Ladybug Scene-managed game.

Here is a sample `Program.cs` file using Ladybug Scene Management:
```csharp
using System;

using Ladybug.SceneManagement;

public static class Program
{
	[STAThread]
	static void Main()
	{
		using (var sceneManager = new SceneManager()
		{
			sceneManager.LoadScene(new MainScene(sceneManager));
			sceneManager.Run()
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