using UnityEngine;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] private SelectableLevel selectableLevelPrefab;
    [SerializeField] private Transform selectableLevelParent;
    
    private void Start()
    {
        var levels = LevelTracker.AvailableLevelCount;

        for (int i = 0; i < levels; i++)
        {
            var level = Instantiate(selectableLevelPrefab, selectableLevelParent);
            level.InitButton(i);
        }
    }

    
}
