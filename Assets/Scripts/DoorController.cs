using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private  Sound[] _takeSound;
    private LevelManager _levelManager;
    private Animator _animator;
    public virtual void Start()
    {
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {        
            PlaySound();
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        _animator.SetBool("open", true);
    }
    
    private void PlaySound()
    {
        _levelManager.soundManager.PlaySound(_takeSound);
    }
}
