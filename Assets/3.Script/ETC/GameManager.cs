using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê (Awake)
    public static GameManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public bool isPause = false;
    public GameObject LastDoor;
    
    public void Ending()
    {
        LastDoor.transform.GetComponent<Animator>().enabled = true;
    }
}
