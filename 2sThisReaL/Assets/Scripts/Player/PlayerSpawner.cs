using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0, 3, 0);

    IEnumerator Start()
    {
        // 프레임 대기 후 실행 (CharacterManager가 완전히 살아난 뒤)
        yield return null;

        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.SpawnPlayer(spawnPosition);
            Debug.Log("[PlayerSpawner] SpawnPlayer 호출 완료");
        }
        else
        {
            Debug.LogError("[PlayerSpawner] CharacterManager.Instance가 null입니다");
        }
    }
}
