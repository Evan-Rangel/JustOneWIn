using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class PlayerMovementControllerTemp : NetworkBehaviour
{
    public float speed = 0.1f;
    public GameObject playerModel;
   [SerializeField] CharacterData characterData;

    private void Start()
    {
        playerModel.SetActive(false);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name=="TestLevel")
        {
            if (!playerModel.activeSelf)
            {
                SetPosition();
                PlayerCharacterSetup();
                playerModel.SetActive(true);
            }
            //if (authority)
            {
                Movement();
            }
        }
    }
    public void SetPosition()
    {
        transform.position = Vector2.zero;//new Vector2(Random.Range(-5,5),0);
    }
    public void Movement()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        Vector2 movDirection = new Vector2(xDir, yDir);
        transform.position +=(Vector3) movDirection * speed;
    }
    public void PlayerCharacterSetup()
    {
     //   characterData = GetComponent<PlayerObjectController>().character;
    }
}
