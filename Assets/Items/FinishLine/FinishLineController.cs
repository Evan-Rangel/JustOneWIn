using UnityEngine;
using Mirror;
namespace Avocado
{
    public class FinishLineController : NetworkBehaviour//, ICollidable
    {
        string tempName;
        Texture2D tempIcon;

        //[Server]
       // public void OnCollision()
       // {
            //if (isServer)
              //  RpcNotifyClients();
        //}

        [ClientRpc]
        private void RpcNotifyClients()
        {
            Debug.Log("Notify");

            EndGamePanel.instance.SetWinnerPlayer(tempName,tempIcon);
        }
        [Server]
        public void OnPlayerCollision()
        {
            Debug.Log("Command");

            RpcNotifyClients();

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Trigger");
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Player");

                tempName = collision.transform.GetComponent<PlayerObjectController>().playerName;
                tempIcon = collision.transform.GetComponent<PlayerObjectController>().iconText;
                if(authority)
                OnPlayerCollision();
            }    
        }

    }
}
