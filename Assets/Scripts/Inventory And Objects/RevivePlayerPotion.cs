/* Function: Add functionality to the revive player potion
   Author: Daniel Degollado Rodríguez
   Modification date: 27/10/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RevivePlayerPotion : MonoBehaviourPunCallbacks{

    private SpriteRenderer spriteRenderer = null;
    private float range = 10.0f;
    DropManager drm;
    string currentSceneName;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        drm = DropManager.Instance;
        currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(Explode());
    }

    // Remote procedure call to destroy the potion after explosion. Takes the name of the potion as a parameter.
    [PunRPC]
    public void DestroyPotion(string potionGameObjectName){
        GameObject potion = GameObject.Find(potionGameObjectName);
        if (potion.GetPhotonView().IsMine)
        {
            PhotonNetwork.Destroy(potion);
        }
    }

    // Find the photon views within a specific range
    void FindPhotonViewsInRange(){
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        Vector3 myPosition = transform.position;

        foreach (PhotonView view in photonViews){
            if (view != photonView && Vector3.Distance(view.transform.position, myPosition) <= range){
                photonView.RPC("RevivePlayer", RpcTarget.All, view.ViewID);
            }
        }
    }

    // Remote procedure call to revive player and destroy the thomb object (DeadCharacter tag). Takes photon view's id as a parameter.
    [PunRPC]
    public void RevivePlayer(int targetViewID){

        PhotonView targetView = PhotonView.Find(targetViewID);

        if (targetView != null){
            if(targetView.tag == "DeadCharacter" &&  targetView != null){
                Vector2 targetPosition = targetView.transform.position;
                string targetViewTag = targetView.tag;
                drm.RemoveDropPosition(targetPosition, currentSceneName, targetViewTag);
                PhotonNetwork.Destroy(targetView.gameObject);      
            }
            else{
                UIController uiController = targetView.GetComponent<UIController>();

                if (uiController != null){
                    bool isPlayerDead = uiController.isDead;

                    if (isPlayerDead){
                        uiController.RevivePlayer();
                    }
                }
                else{
                    Debug.Log("UIController not found");
                }
            }
          
        }
        else{
            Debug.Log("Target view not found");
        }
    }

    /* Coroutine that changes potion color to illustrate that it's about to explode, calls  the function to find the photon views within a range
    and destroys the potion for all players using a remote procedure call */
    IEnumerator Explode(){
        // Color changing effect
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }

        // photonView.RPC("GetColliders", RpcTarget.All);
        FindPhotonViewsInRange();
        
        // RPC for owner to destroy de gameobject
        photonView.RPC("DestroyPotion", RpcTarget.All, gameObject.name);
    }
}
