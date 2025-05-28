using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0, 3, 0);

    IEnumerator Start()
    {
        // ������ ��� �� ���� (CharacterManager�� ������ ��Ƴ� ��)
        yield return null;

        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.SpawnPlayer(spawnPosition);
            Debug.Log("[PlayerSpawner] SpawnPlayer ȣ�� �Ϸ�");
        }
        else
        {
            Debug.LogError("[PlayerSpawner] CharacterManager.Instance�� null�Դϴ�");
        }
    }
}
