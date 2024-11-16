//using Assets.Scripts.UI.Command;
using System.Collections.Generic;

public class UICommandQueue
{
    // Доступная из системной библиотеки коллекция типа Очередь (первый вошел - первый вышел)
    private readonly Queue<IUICommand> queue = new();

    // Метод для добавления команд в очередь
    public bool TryEnqueueCommand(IUICommand сommand)
    {
        queue.Enqueue(сommand);
        return true;
    }

    // Метод для получения команд из очереди
    public bool TryDequeueCommand(out IUICommand сommand)
    {
        if (queue.Count > 0)
        {
            сommand = queue.Dequeue();
            return true;
        }

        сommand = default;
        return false;
    }
}
