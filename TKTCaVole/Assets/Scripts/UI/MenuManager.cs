using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [FormerlySerializedAs("rotateSpeed")] [SerializeField] private Vector3 cameraRotateSpeed;

    [Header("Transforms")]
    [SerializeField] private Transform menuCam;
    [SerializeField] private Transform menuTr;
    [SerializeField] private Transform levelsTr;
    [SerializeField] private Transform levelsParent;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button returnToMenuButton;

    public static bool skipMenu;
    public static int lastPlayedLevel;
    
    private void Start()
    {
        playButton.onClick.AddListener(ShowLevels);
        returnToMenuButton.onClick.AddListener(ShowMenu);
        exitButton.onClick.AddListener(Application.Quit);
        
        if(skipMenu) ShowLevels();
    }

    private void Update()
    {
        menuCam.Rotate(cameraRotateSpeed);
    }

    private void ShowLevels()
    {
        menuTr.DOLocalMoveX(-1920, 0.75f);
        levelsTr.DOLocalMoveX(0, 0.75f);
    }

    private void ShowMenu()
    {
        menuTr.DOLocalMoveX(0, 0.75f);
        levelsTr.DOLocalMoveX(1920, 0.75f);
    }
}