using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStomper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float bounce;

    #region Stomper
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "WeakPoint")
        {
            Destroy(collider.gameObject);
            playerRb.velocity = new Vector2 (playerRb.velocity.x , bounce);
        }
    }
    #endregion
}
