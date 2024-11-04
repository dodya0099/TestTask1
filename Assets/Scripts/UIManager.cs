using UnityEngine;
using Enums;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel, _gamePanel, _winPanel, _losePanel, _changeMoney, _redDollar, _greenDollar;
    [SerializeField] private Text _moneyCountText, _moneyBarText, _dollarCountText;
    [SerializeField] private Slider _moneyBar;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] Sound[] _winSound;
    private Vector3 changeMoneyPos;
    [SerializeField] private GameObject _dollarPrefab;
    [SerializeField] private Transform _spawnPosition, _targetPosition;
    private int dollarIcons = 20, currentDollarCount = 0;
    private float flyDelay = 0.5f;

    private void Start()
    {
        changeMoneyPos = _changeMoney.transform.position;
        InitializeUI();
    }
    public void SetGameStateUI(GameState gameState)
    {
        if (gameState == GameState.start)
        {
            _startPanel.SetActive(true);
            _gamePanel.SetActive(false);
            _winPanel.SetActive(false);
            _losePanel.SetActive(false);
        }
        else if (gameState == GameState.play)
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(true);
            _winPanel.SetActive(false);
            _losePanel.SetActive(false);
        }
        else if (gameState == GameState.win)
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _winPanel.SetActive(true);
            _losePanel.SetActive(false);
            _levelManager.UIAnimationManager.WinPanelIn();
            _levelManager.soundManager.PlaySound(_winSound);
        }
        else if (gameState == GameState.lose)
        {
            _startPanel.SetActive(false);
            _gamePanel.SetActive(false);
            _winPanel.SetActive(false);
            _losePanel.SetActive(true);
            _levelManager.UIAnimationManager.LosePanelIn();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void CollectDollars()
    {
        _levelManager.UIAnimationManager.WinPanelNextIn();
        StartCoroutine(AnimateDollars());
    }

    public void SetUIMoney(int count)
    {
        _moneyCountText.text = count.ToString();
    }

    public void ChangeCoins(int count, int currentMoney)
    {
        Text changeMoneyText = _changeMoney.GetComponent<Text>();
        char sign = ' ';
        if (count > 0)
        {
            _redDollar.SetActive(false);
            _greenDollar.SetActive(true);
            _greenDollar.GetComponent<Image>().DOFade(1, 0);
            _greenDollar.GetComponent<Image>().DOFade(0, 1);
            changeMoneyText.color = new Color(0.372549f, 0.8588236f, 0.01960784f);
            sign = '+';
        }
        else if (count < 0)
        {
            changeMoneyText.color = new Color(0.9921569f, 0, 0);
            _redDollar.SetActive(true);
            _greenDollar.SetActive(false);
            _redDollar.GetComponent<Image>().DOFade(1, 0);
            _redDollar.GetComponent<Image>().DOFade(0, 1);
            sign = ' ';
        }

        _changeMoney.gameObject.SetActive(true);
        changeMoneyText.DOKill();
        _changeMoney.transform.DOScale(0.8f, 0);
        changeMoneyText.DOFade(1, 0);
        _changeMoney.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutBounce);
        _changeMoney.GetComponent<Outline>().DOFade(1, 0);
        _changeMoney.transform.DOMove(changeMoneyPos, 0);
        changeMoneyText.text = sign + (count).ToString();
        _changeMoney.transform.DOMoveY(changeMoneyPos.y + 200f, 1f);
        _changeMoney.GetComponent<Outline>().DOFade(0, 1);
        changeMoneyText.DOFade(0, 1).OnComplete(() => { _changeMoney.gameObject.SetActive(false); });

        ChangeMoneyBar(currentMoney);
    }

    private void ChangeMoneyBar(int moneyCount)
    {
        _moneyBar.value = moneyCount;

        if (moneyCount < 30)
        {
            _moneyBar.fillRect.GetComponent<Image>().color = new Color(0.9921569f, 0, 0);
            _moneyBarText.text = "БЕДНЫЙ";
            _moneyBarText.color = new Color(0.9921569f, 0, 0);
        }
        else if (moneyCount < 60)
        {
            _moneyBar.fillRect.GetComponent<Image>().color = new Color(0.9882354f, 0.7215686f, 0);
            _moneyBarText.text = "СОСТОЯТЕЛЬНЫЙ";
            _moneyBarText.color = new Color(0.9882354f, 0.7215686f, 0);
        }
        else if (moneyCount > 60)
        {
            _moneyBar.fillRect.GetComponent<Image>().color = new Color(0.372549f, 0.8588236f, 0.01960784f);
            _moneyBarText.text = "БОГАТЫЙ";
            _moneyBarText.color = new Color(0.372549f, 0.8588236f, 0.01960784f);
        }
    }

    private IEnumerator AnimateDollars()
    {
        GameObject[] dollarIconsArray = new GameObject[dollarIcons];

        for (int i = 0; i < dollarIcons; i++)
        {
            GameObject dollarIcon = Instantiate(_dollarPrefab, _spawnPosition.position, Quaternion.identity, _spawnPosition.parent);
            dollarIconsArray[i] = dollarIcon;

            Vector3 upwardRightOffset = new Vector3(Random.Range(80f, 200f), Random.Range(100f, 500f), 0);
            dollarIcon.transform.DOMove(_spawnPosition.position + upwardRightOffset, 1f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(flyDelay);

        for (int i = 0; i < dollarIcons; i++)
        {
            GameObject dollarIcon = dollarIconsArray[i];

            yield return new WaitForSeconds(0.1f);

            dollarIcon.transform.DOMove(_targetPosition.position, 1f).SetEase(Ease.InQuad)
                .OnComplete(() => Destroy(dollarIcon));
        }

        AnimateDollarCountIncrease();
    }

    private void AnimateDollarCountIncrease()
    {
        int startValue = PlayerPrefs.GetInt("money"); 
        int targetValue = startValue + (_levelManager.GetCurrentMoney() * 4);

        DOTween.To(() => startValue, x =>
        {
            _dollarCountText.text = x.ToString();
        }, targetValue, 1f).SetEase(Ease.OutCubic)
          .OnComplete(() =>
          {
              PlayerPrefs.SetInt("money", targetValue);
              PlayerPrefs.Save();
          });
    }

    private void InitializeUI()
    {
        _dollarCountText.text = PlayerPrefs.GetInt("money").ToString();
    }
}
