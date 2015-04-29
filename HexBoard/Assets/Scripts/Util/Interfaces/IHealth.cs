
namespace Assets.Scripts.Util.Interfaces
{
    public interface IHealth
    {
        void TakeDemage(float demage);
        bool IsAlive();
        float GetTotalHealth();
        float GetCurrentHealth();
        void AddHealth(float healPoints);
        void UpdateHealth();
        void RestoreHealth();
    }
}

