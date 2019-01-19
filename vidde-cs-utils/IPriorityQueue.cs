/**
 * Created by Vidar Magnusson on 19/1 - 2019
 * https://github.com/ViddeM/
 */
namespace vidde_cs_utils.PriorityQueue
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