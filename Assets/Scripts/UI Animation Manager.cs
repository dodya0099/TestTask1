using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject _wintext1, _wintext2, _winButton1, _winButton2, _winImage1, _losetext1, _loseButton1, _loseImage1;
    [SerializeField] private Text _winButtonText;
    [SerializeField] private LevelManager _levelManager;

    public void WinPanelIn()
    {
        _winButtonText.text = (_levelManager.GetCurrentMoney()*4).ToString();
        _wintext1.transform.DOLocalMoveY(900, 1).SetEase(Ease.OutBack);
        _wintext2.transform.DOLocalMoveY(500, 1).SetEase(Ease.OutBack);
        _winButton1.transform.DOLocalMoveY(-896, 1f).SetEase(Ease.OutBack);
        _winImage1.transform.DOLocalMoveY(963, 1f).SetEase(Ease.OutBack);
    }

    public void LosePanelIn()
    {
        _losetext1.transform.DOLocalMoveY(900, 1).SetEase(Ease.OutBack);
        _loseButton1.transform.DOLocalMoveY(-896, 1f).SetEase(Ease.OutBack);
        _loseImage1.transform.DOLocalMoveY(963, 1f).SetEase(Ease.OutBack);
    }

    public void WinPanelNextIn()
    {
        _winButton1.transform.DOLocalMoveY(-1500, 1f).SetEase(Ease.OutBack);
        _winButton2.transform.DOLocalMoveY(-896, 1f).SetEase(Ease.OutBack);
    }
}
