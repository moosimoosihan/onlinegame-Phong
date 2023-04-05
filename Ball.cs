using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun
{
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private Vector2 direction = Vector2.right;
    private readonly float speed = 10f;
    private readonly float randomRefectionIntensity = 0.1f;
    
    private void FixedUpdate()
    {
        // 방장이 아닌경우 || 플레이어가 2보다 적을 경우
        if(!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2) return;

        var distance = speed * Time.deltaTime;
        var hit = Physics2D.Raycast(transform.position, direction, distance);

        if(hit.collider != null){

            var goalPost = hit.collider.GetComponent<Goalpost>();
            if(goalPost != null){
                if(goalPost.playerNumber == 1){
                    GameManager.Instance.AddScore(2,1);
                } else if(goalPost.playerNumber == 2){
                    GameManager.Instance.AddScore(1,1);
                }
            }
            direction = Vector2.Reflect(direction, hit.normal);
            direction += Random.insideUnitCircle * randomRefectionIntensity;
        }

        transform.position = (Vector2)transform.position + direction * distance;
    }
}
