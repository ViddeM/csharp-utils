namespace CSUtils.PriorityQueue
{
    public interface IPriorityQueue<T>
    {
        void Enqueue(T data, float priority);
        T Dequeue();
        void UpdatePriority(T data, float priority);
        bool Contains(T data);
        int Count();
    }
}