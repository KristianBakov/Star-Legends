using UnityEngine;
using UnityEngine.UIElements;

class AgentSelectButton : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        var root = document.rootVisualElement;
        if (root == null) return;

        root.Clear();
    }
}

