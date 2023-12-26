using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectScreen : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;
    void Start()
    {
        Generate();
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        Generate();
    }

    private void Generate()
    {
        var root = document.rootVisualElement;
        root.Clear();

        //add the stylesheet
        root.styleSheets.Add(styleSheet);


        var backgroundBox = UIUtils.CreateUIElement("background-box");
        var titleLabel = UIUtils.CreateUIElement<Label>("Select Agent");
        titleLabel.AddToClassList("title-label");

        

        root.Add(backgroundBox);
        backgroundBox.Add(titleLabel);
    }


}
