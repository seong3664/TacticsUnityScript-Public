using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamMove : MonoBehaviour
{
    PlayerInput playerInput;
    InputActionMap actionMap;
    Vector3 movedir;
    List<GameObject> Player;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        actionMap = playerInput.actions.FindActionMap("3D turn based Tactics");
        Player = GameManager.gamemaneger.PlayerList;
        turnManager.EnemyEndTurn += EndTurnLookPlayer;
            transform.position = Player[0].transform.position;
    }
    private void Update()
    {
        transform.Translate(movedir*Time.deltaTime*10f);
    }
    public void OnMove(InputAction.CallbackContext Value)
    {
        Vector2 dir = Value.ReadValue<Vector2>();
        movedir = new Vector3(dir.x, 0, dir.y);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if(context.control.name == "q")
        {
            StartCoroutine(RotateOverTime(90));
        }
        else
        {
            StartCoroutine(RotateOverTime(-90));
        }
    }
    void EndTurnLookPlayer()
    {
        transform.position = Player[Random.Range(0, Player.Count)].transform.position;
    }
    IEnumerator RotateOverTime(float targetAngle)
    {
        float startAngle = transform.rotation.eulerAngles.y;
        float S_Time = 0f;

        while (S_Time < 1f)
        {
            S_Time += Time.deltaTime;
            float t = Mathf.Clamp01(S_Time / 1f);
            float currentAngle = Mathf.Lerp(startAngle,startAngle + targetAngle, t);
            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            yield return null; // 다음 프레임까지 대기
        }

 
        transform.rotation = Quaternion.Euler(0f,startAngle+ targetAngle, 0f);
    }
}
