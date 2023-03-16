using System;

TaskLimit limit = new TaskLimit(3);
for (int i = 0; i < 100; i++)
{
	limit.Run(() => {
		Thread.Sleep(3000);
		Console.WriteLine("new task");
		});
}
Console.ReadKey();
class TaskLimit {

	public Task Run(Action action)
	{
		lock (_objLock)
		{
			if (_queue.Count >= _maxTask)
			{
				Task.WaitAny(_queue.ToArray());
				_queue.RemoveAll(a => ((int)a.Status) > 3);
			}
			var task = Task.Run(action);
			_queue.Add(task);

			return task;
		}
	}
	List<Task> _queue = new List<Task>();
	int _maxTask;
	object _objLock = new object();
	public TaskLimit(int maxCountParallerTask)
	{
		_maxTask = maxCountParallerTask;
	}
}
