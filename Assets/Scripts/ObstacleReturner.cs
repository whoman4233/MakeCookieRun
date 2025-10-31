using UnityEngine;

public class ObstacleReturner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Obstacles>(out Obstacles obstacle))
        {
            ObstaclePool pool = FindObjectOfType<ObstaclePool>();
            if (pool != null)
            {
                pool.ReturnObstacle(obstacle.gameObject, obstacle.obstacleType);
            }
        }
    }
}