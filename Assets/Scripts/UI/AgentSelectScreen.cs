using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectScreen : MonoBehaviour
{
    [SerializeField] public UIDocument document;
    [SerializeField] private StyleSheet styleSheet;

    private AgentSelectButtonController agentSelectButtonController;
    void Start()
    {
        Generate();
        if (agentSelectButtonController == null)
            agentSelectButtonController = gameObject.GetComponent<AgentSelectButtonController>();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;
        Generate();
        if (agentSelectButtonController == null)
            agentSelectButtonController = gameObject.GetComponent<AgentSelectButtonController>();
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

        agentSelectButtonController.InitializeCharacterList();



        root.Add(backgroundBox);
        backgroundBox.Add(titleLabel);
        backgroundBox.Add(agentSelectButtonController.agentSelectListView);
       
    }


}
