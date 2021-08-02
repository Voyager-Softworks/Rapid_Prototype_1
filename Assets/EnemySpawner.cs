using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public PropList m_PropList;
    public GameObject m_EnemyPrefab;

    public int m_SpawnChance;

    public float m_RoomSize;
    public int m_TileNum;

    float m_offset;


    public bool[,] m_ValidTiles;



    public void CheckTiles()
    {

        m_ValidTiles = new bool[m_TileNum, m_TileNum];
        float tilesize = m_RoomSize / m_TileNum;
        for (var i = 0; i < m_TileNum; i++)
        {
            for (var j = 0; j < m_TileNum; j++)
            {
                Vector3 tilepos = this.transform.position - new Vector3((m_RoomSize / 2) - tilesize / 2, 0, (m_RoomSize / 2) - tilesize / 2) + new Vector3(i * tilesize, 0, j * tilesize);
                bool isfloor = false;
                bool isoccupied = false;

                RaycastHit[] hits = Physics.BoxCastAll(tilepos, new Vector3(tilesize / 2, tilesize / 2, tilesize / 2), Vector3.up);
                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject.CompareTag("Floor"))
                    {
                        isfloor = true;
                    }
                    if (hit.collider.gameObject.CompareTag("Obstacle"))
                    {
                        isoccupied = true;
                    }
                }
                if (!isoccupied)
                {
                    m_ValidTiles[i, j] = true;
                }
                else
                {
                    m_ValidTiles[i, j] = false;
                }



            }
        }
    }
    public void GenerateProps()
    {

        CheckTiles();
        float tilesize = m_RoomSize / m_TileNum;
        for (var i = 0; i < m_TileNum; i++)
        {
            for (var j = 0; j < m_TileNum; j++)
            {
                if (m_ValidTiles[i, j])
                {
                    Vector3 tilepos = this.transform.position - new Vector3((m_RoomSize / 2) - tilesize / 2, 0, (m_RoomSize / 2) - tilesize / 2) + new Vector3(i * tilesize, 0, j * tilesize);

                    if (Random.Range(0, 100) < m_SpawnChance)
                    {
                        GameObject.Instantiate(m_PropList.Prefabs[Random.Range(0, m_PropList.Prefabs.Count)], tilepos - new Vector3(0, 1, 0), Quaternion.identity, transform);

                    }
                    else if (Random.Range(0, 100) < m_SpawnChance)
                    {
                        GameObject.Instantiate(m_EnemyPrefab, tilepos - new Vector3(0, 0, 0), Quaternion.identity, transform);
                    }
                }




            }
        }
    }

    public void GenerateEnemies()
    {
        CheckTiles();
        float tilesize = m_RoomSize / m_TileNum;
        for (var i = 0; i < m_TileNum; i++)
        {
            for (var j = 0; j < m_TileNum; j++)
            {
                if (m_ValidTiles[i, j])
                {
                    Vector3 tilepos = this.transform.position - new Vector3((m_RoomSize / 2) - tilesize / 2, 0, (m_RoomSize / 2) - tilesize / 2) + new Vector3(i * tilesize, 0, j * tilesize);

                    if (Random.Range(0, 3) == 0)
                    {


                    }
                }




            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmosSelected()
    {
        CheckTiles();
        float tilesize = m_RoomSize / m_TileNum;
        for (var i = 0; i < m_TileNum; i++)
        {
            for (var j = 0; j < m_TileNum; j++)
            {
                if (m_ValidTiles[i, j])
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Vector3 tilepos = this.transform.position - new Vector3((m_RoomSize / 2) - tilesize / 2, 0, (m_RoomSize / 2) - tilesize / 2) + new Vector3(i * tilesize, 0, j * tilesize);


                Gizmos.DrawWireCube(tilepos, new Vector3(tilesize, tilesize, tilesize));

            }
        }
    }

}