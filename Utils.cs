using UnityEngine.SceneManagement;

namespace VisceralRagdolls
{
    public class Utils
    {
        private void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
        {
            if (!VisceralEntry.Instance)
            {
                return;
            }

            VisceralEntry.Instance.IsSoT = CheckForGameplayMap(newScene.name);
        }
        public static bool CheckForGameplayMap(string sceneName)
        {
            switch (sceneName)
            {
                case "City_Scripts": // Streets of Karkov
                    return true;
                default:
                    return false;
            }
        }
    }
}
