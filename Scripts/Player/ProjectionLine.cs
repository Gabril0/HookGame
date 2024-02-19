using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionLine : MonoBehaviour
{
    SpriteRenderer projection;
    private UnityEngine.Color projectionYellow = new UnityEngine.Color(255f / 255f, 255f / 255f, 13f / 255f, 1f);
    private UnityEngine.Color projectionGray = new UnityEngine.Color(200f / 255f, 200f / 255f, 200f / 255f, 0.5f);
    private PlayerController playerController;
    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        projection = GetComponent<SpriteRenderer>();
        projection.enabled = true;
        transform.parent = null;
    }
    private void LateUpdate()
    {
        if (playerController.isAlive) { Project(); }
    }

    private void Project()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = new Vector2(playerController.transform.position.x, playerController.transform.position.y);
        Vector2 direction = (mousePosition - playerPosition).normalized;

        float distanceFromPlayer = 4f;
        projection.transform.position = playerController.transform.position + new Vector3(direction.x * distanceFromPlayer, direction.y * distanceFromPlayer, 0f);
        projection.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        projection.color = Physics2D.Raycast(playerController.transform.position, direction, playerController.maxRopeSize, ~playerController.playerLayer) ? projectionYellow : projectionGray;
        projection.enabled = playerController.isAlive;
    }
}
