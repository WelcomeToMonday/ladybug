using System;
using System.Collections.Generic;

namespace Ladybug
{
	/// <summary>
	/// Handles queuing and processing tasks
	/// on the main thread.
	/// </summary>
	public class ThreadManager
	{
		private static readonly List<Action> executeOnMainThread = new List<Action>();
		private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
		private static bool actionToExecuteOnMainThread = false;

		/// <summary>
		/// Process tasks in the ThreadManager's queue
		/// </summary>
		public void Update()
		{
			ExecuteActions();
		}

		/// <summary>Sets an action to be executed on the main thread.</summary>
		/// <param name="_action">The action to be executed on the main thread.</param>
		public static void QueueAction(Action _action)
		{
			if (_action == null)
			{
				return;
			}

			lock (executeOnMainThread)
			{
				executeOnMainThread.Add(_action);
				actionToExecuteOnMainThread = true;
			}
		}

		/// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
		private static void ExecuteActions()
		{
			if (actionToExecuteOnMainThread)
			{
				executeCopiedOnMainThread.Clear();
				lock (executeOnMainThread)
				{
					executeCopiedOnMainThread.AddRange(executeOnMainThread);
					executeOnMainThread.Clear();
					actionToExecuteOnMainThread = false;
				}

				for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
				{
					executeCopiedOnMainThread[i]();
				}
			}
		}
	}
}