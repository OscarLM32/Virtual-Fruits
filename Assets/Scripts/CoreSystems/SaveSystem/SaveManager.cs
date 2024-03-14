using GameSystems.Singleton;

namespace CoreSystems.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        private DynamicDifficultySaver _dynamicDifficultySaver;
        private LevelsDataSaver _levelsDataSaver;

        protected override void OnAwake()
        {
            
        }
    }
}