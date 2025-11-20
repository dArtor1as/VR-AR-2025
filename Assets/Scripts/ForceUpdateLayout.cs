using UnityEngine;
using UnityEngine.UI;

public class ForceUpdateLayout : MonoBehaviour
{
    public void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}