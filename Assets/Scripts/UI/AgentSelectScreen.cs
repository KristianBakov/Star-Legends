using UnityEngine;
using UnityEngine.UIElements;

public class AgentSelectScreen : MonoBehaviour
{
    [SerializeField] public UIDocument document;
    [SerializeField] private StyleSheet styleSheet;
    private AgentSelectButtonController agentSelectButtonController;
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

        if(agentSelectButtonController == null) agentSelectButtonController = gameObject.AddComponent<AgentSelectButtonController>();
        agentSelectButtonController.InitializeCharacterList();



        root.Add(backgroundBox);
        backgroundBox.Add(titleLabel);
        backgroundBox.Add(agentSelectButtonController.agentSelectListView);
       
    }


}
