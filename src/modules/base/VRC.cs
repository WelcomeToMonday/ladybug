using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug
{
	/// <summary>
	/// Ladybug Virtual Resolution Container
	/// </summary>
	public class VRC
	{
		private Scene _scene;
		private SpriteBatch _spriteBatch;
		private RenderTarget2D _renderTarget;

		private Action<GameTime, SpriteBatch> _onDraw = (GameTime gameTime, SpriteBatch spriteBatch) => { };

		private Action<GameTime, SpriteBatch> _onDrawBackground = (GameTime gameTime, SpriteBatch spriteBatch) => { };

		/// <summary>
		/// Creates a new Virtual Resolution Container
		/// </summary>
		/// <param name="parentScene">Parent <see cref="Ladybug.Scene"/></param>
		/// <param name="virtualWidth">Virtual Resolution Width</param>
		/// <param name="virtualHeight">Virtual Resolution Height</param>
		public VRC(Scene parentScene, int virtualWidth, int virtualHeight)
		{
			_scene = parentScene;

			VirtualHeight = virtualHeight;
			VirtualWidth = virtualWidth;

			_spriteBatch = new SpriteBatch(_scene.Game.GraphicsDevice);

			_renderTarget = new RenderTarget2D(
				_scene.Game.GraphicsDevice,
				VirtualWidth, VirtualHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				_scene.Game.GraphicsDevice.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.DiscardContents
			);

			_scene.Game.Window.ClientSizeChanged += OnClientSizeChanged;

			RebuildCanvas();
		}
		/// <summary>
		/// GraphicsDevice of the parent <see cref="Ladybug.Game"/> instance
		/// </summary>
		/// <value></value>
		public GraphicsDevice GraphicsDevice { get => _scene.Game.GraphicsDevice; }

		/// <summary>
		/// Window of the parent <see cref="Ladybug.Game"/> instance
		/// </summary>
		/// <value></value>
		public GameWindow Window { get => _scene.Game.Window; }

		/// <summary>
		/// Matrix representing this VRC's size, scale, and rotation
		/// </summary>
		/// <value></value>
		public Matrix TransformMatrix { get; private set; }

		/// <summary>
		/// Virtual aspect ratio the VRC will maintain as window size changes
		/// </summary>
		/// <returns></returns>
		public float PreferredAspectRatio { get => (float)VirtualWidth / (float)VirtualHeight; }

		/// <summary>
		/// Actual aspect ratio of the application window
		/// </summary>
		/// <returns></returns>
		public float ActualAspectRatio { get => (float)Window.ClientBounds.Width / (float)Window.ClientBounds.Height; }

		/// <summary>
		/// Virtual height value of the VRC's canvas
		/// </summary>
		/// <value></value>
		public int VirtualHeight { get; private set; }

		/// <summary>
		/// Virtual width value of the VRC's canvas
		/// </summary>
		/// <value></value>
		public int VirtualWidth { get; private set; }

		/// <summary>
		/// Calculated scale of the VRC's canvas
		/// </summary>
		/// <returns></returns>
		public Vector2 Scale { get => new Vector2((float)Canvas.Width / VirtualWidth, (float)Canvas.Height / VirtualHeight); }

		/// <summary>
		/// The Canvas contains the content the VRC is rendering, which automatically resizes to maintain
		/// aspect ratio as the game window size changes
		/// </summary>
		/// <value></value>
		public Rectangle Canvas { get; private set; }

		/// <summary>
		/// Background color of the VRC. Affects the color of Letterbox bars
		/// </summary>
		/// <value></value>
		public Color BackgroundColor { get; private set; } = Color.Black;

		private void OnClientSizeChanged(object sender, EventArgs e) => RebuildCanvas();

		private void RebuildCanvas()
		{
			Rectangle dst;
			if (ActualAspectRatio <= PreferredAspectRatio)
			{
				int presentHeight = (int)((Window.ClientBounds.Width / PreferredAspectRatio) + 0.5f);
				int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
				dst = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
			}
			else
			{
				int presentWidth = (int)((Window.ClientBounds.Height * PreferredAspectRatio) + 0.5f);
				int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
				dst = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
			}
			Canvas = dst;
		}

		/// <summary>
		/// Converts a Vector2 coordinate from screen space to a
		/// location relative to the VRC's canvas
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector2 ScreenToCanvasSpace(Vector2 position)
		{
			var realX = position.X - Canvas.X;
			var realY = position.Y - Canvas.Y;
			return new Vector2
			(
				realX / Scale.X,
				realY / Scale.Y
			);
		}

		/// <summary>
		/// Sets the action performed when the VRC is drawn to the screen
		/// </summary>
		/// <param name="action"></param>
		/// <remarks>Use this to draw to the VRC's canvas</remarks>
		public void OnDraw(Action<GameTime, SpriteBatch> action) => _onDraw = action;

		/// <summary>
		/// Sets the action performed when the VRC is drawing the background behind its canvas
		/// </summary>
		/// <param name="action"></param>
		/// <remarks>Use this to set a texture/image for letterboxes</remarks>
		public void OnDrawBackground(Action<GameTime, SpriteBatch> action) => _onDrawBackground = action;

		/// <summary>
		/// Draws all content handled by the VRC to its canvas, then draws the canvas
		/// centered within the game window
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			GraphicsDevice.SetRenderTarget(_renderTarget);

			GraphicsDevice.Clear(BackgroundColor);

			_onDraw(gameTime, spriteBatch);

			GraphicsDevice.SetRenderTarget(null);

			GraphicsDevice.Clear(ClearOptions.Target, BackgroundColor, 1.0f, 0);

			_onDrawBackground(gameTime, spriteBatch);

			_spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, blendState: BlendState.Opaque, samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_renderTarget, Canvas, Color.White);
			_spriteBatch.End();
		}
	}
}