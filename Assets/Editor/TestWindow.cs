using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private float _time;
    private float _shakeTime;
    private float _slideX;
    private bool _isShaking;
    private bool _isSliding;
    private bool _slidingOut;
    private VisualElement _pandaRoux;
    private VisualElement _root;
    private Button _reviensButton;

    [MenuItem("Window/UI Toolkit/TestWindow %&w")]
    public static void ShowExample()
    {
        TestWindow wnd = GetWindow<TestWindow>();
        wnd.titleContent = new GUIContent("Test Window controller");
    }

    public void CreateGUI()
    {
        _root = rootVisualElement;

        VisualElement tool = m_VisualTreeAsset.Instantiate();
        _root.Add(tool);

        Button firstButton = tool.Query<Button>("Scare");
        firstButton.clicked += OnClcikScareHim;
        
        _reviensButton = new Button(OnReviensButton) { text = "Reviens! 🐾" };
        _reviensButton.style.display = DisplayStyle.None;

        foreach (string cls in firstButton.GetClasses())
            _reviensButton.AddToClassList(cls);

        firstButton.parent.Add(_reviensButton);

        _pandaRoux = tool.Query<VisualElement>("Logo");
        _pandaRoux.style.transformOrigin = new TransformOrigin(Length.Percent(50), Length.Percent(50));

        _pandaRoux.schedule.Execute(() =>
        {
            _time += 0.016f;
            float angle = Mathf.Sin(_time * 2f) * 90f;
            _pandaRoux.style.rotate = new Rotate(angle);

            if (!_isSliding) return;

            float target = _slidingOut ? 600f : 0f;
            _slideX = Mathf.MoveTowards(_slideX, target, 20f);
            _pandaRoux.style.translate = new Translate(_slideX, 0);

            if (Mathf.Approximately(_slideX, target))
            {
                _isSliding = false;
                if (_slidingOut)
                    _reviensButton.style.display = DisplayStyle.Flex;
            }
        }).Every(16);

        _root.schedule.Execute(() =>
        {
            if (!_isShaking) return;
            _shakeTime += 0.016f;
            if (_shakeTime < 0.5f)
            {
                _root.style.left = Mathf.Sin(_shakeTime * 80f) * 6f;
                _root.style.top = Mathf.Sin(_shakeTime * 60f) * 3f;
            }
            else
            {
                _root.style.left = 0;
                _root.style.top = 0;
                _isShaking = false;
            }
        }).Every(16);
    }

    private void OnClcikScareHim()
    {
        _isShaking = true;
        _shakeTime = 0f;
        _slideX = 0f;
        _slidingOut = true;
        _isSliding = true;
    }

    private void OnReviensButton()
    {
        _reviensButton.style.display = DisplayStyle.None;
        _slideX = 600f;
        _slidingOut = false;
        _isSliding = true;
    }
}