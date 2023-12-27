using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectScreen : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;
    //[SerializeField]
    void Start()
    {
        Generate();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;
        Generate();
#endif
    }

    private void Generate()
    {
        var root = document.rootVisualElement;
        if (root == null) return;

        root.Clear();

        //add the stylesheet
        root.styleSheets.Add(styleSheet);


        var backgroundBox = UIUtils.CreateUIElement("background-box");

        var titleLabel = UIUtils.CreateUIElement<Label>("title-label");
        titleLabel.text = "Select Agent";

        var agentSelectButton = UIUtils.CreateUIElement<AgentSelectButton>("title-label");

        var agentSelectBox = UIUtils.CreateUIElement("agent-select-box");
        var agentSelectListView = UIUtils.CreateUIElement<ListView>("agent-select-list-view");
        agentSelectListView.selectionType = SelectionType.Single;

        

        

        root.Add(backgroundBox);
        backgroundBox.Add(titleLabel);
       
    }


}
