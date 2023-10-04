using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectableLevelButton : MonoBehaviour,ISelectHandler
{
    [SerializeField] private SelectableLevel selectableLevel;
    
    public void OnSelect(BaseEventData eventData)
    {
        selectableLevel.OnButtonSelected();
    }
}
