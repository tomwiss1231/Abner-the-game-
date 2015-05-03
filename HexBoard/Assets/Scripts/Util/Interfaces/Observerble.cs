namespace Assets.Scripts.Util.Interfaces
{
    public interface IObserverble
    {
        void AddObserver(ISoldierObserver observer);
        void RemoveObserver(ISoldierObserver observer);
        void NotifyAll();
    }
}