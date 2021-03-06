using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public FacilityData m_NorthConnecting, m_SouthConnecting, m_EastConnecting, m_WestConnecting;
    public float m_RoomSize = 0.0f;
    public bool m_DisplayGrid;

    

    public Door DoorNorth, DoorSouth, DoorEast, DoorWest;
    public AudioClip m_DoorOpen, m_DoorClose;
    public AudioSource m_DoorAudio;

    GameObject North, South, East, West;

    public bool m_ConnectsNorth, m_ConnectsSouth, m_ConnectsEast, m_ConnectsWest;

    public bool m_GenerateNeighbors = false;

    public bool m_RoomCompleted = false;



    public void SetDoorState(bool _state)
    {
        GenerateNeighbors();
        if (DoorNorth != null)
        {
            if (m_ConnectsNorth && North.gameObject.GetComponent<Room>().m_ConnectsSouth)
            {
                DoorNorth.m_IsOpen = _state;
            }
            else
            {
                DoorNorth.m_IsOpen = false;
            }
        }
        if (DoorSouth != null)
        {
            if (m_ConnectsSouth && South.gameObject.GetComponent<Room>().m_ConnectsNorth)
            {
                DoorSouth.m_IsOpen = _state;
            }
            else
            {
                DoorSouth.m_IsOpen = false;
            }
        }
        if (DoorEast != null)
        {
            if (m_ConnectsEast && East.gameObject.GetComponent<Room>().m_ConnectsWest)
            {
                DoorEast.m_IsOpen = _state;
            }
            else
            {
                DoorEast.m_IsOpen = false;
            }
        }
        if (DoorWest != null)
        {
            if (m_ConnectsWest && West.gameObject.GetComponent<Room>().m_ConnectsEast)
            {
                DoorWest.m_IsOpen = _state;
            }
            else
            {
                DoorWest.m_IsOpen = false;
            }
        }
        m_DoorAudio.clip = _state ? m_DoorOpen : m_DoorClose;
        m_DoorAudio.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GenerateNeighbors)
        {
            m_GenerateNeighbors = false;
            GenerateNeighbors();
        }

    }

    public void CheckNeighbors()
    {
        if (North == null)
        {
            Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(m_RoomSize, 0, 0), new Vector3(10, 10, 10), this.transform.rotation);
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<Room>())
                {
                    North = hit.transform.gameObject;
                }
            }
        }
        if (South == null)
        {
            Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(-m_RoomSize, 0, 0), new Vector3(10, 10, 10), this.transform.rotation);
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<Room>())
                {
                    South = hit.transform.gameObject;
                }
            }
        }
        if (East == null)
        {
            Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0, 0, -m_RoomSize), new Vector3(10, 10, 10), this.transform.rotation);
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<Room>())
                {
                    East = hit.transform.gameObject;
                }
            }
        }
        if (West == null)
        {
            Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0, 0, m_RoomSize), new Vector3(10, 10, 10), this.transform.rotation);
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<Room>())
                {
                    West = hit.transform.gameObject;
                }
            }
        }
    }

    public void GenerateNeighbors()
    {
        CheckNeighbors();
        if (North == null && m_ConnectsNorth)
        {
            North = GameObject.Instantiate(m_SouthConnecting.m_RoomPrefabs[Random.Range(0, m_SouthConnecting.m_RoomPrefabs.Count - 1)], transform.position + new Vector3(m_RoomSize, 0, 0), this.transform.rotation);
            North.GetComponent<Room>().South = this.gameObject;
        }
        if (South == null && m_ConnectsSouth)
        {
            South = GameObject.Instantiate(m_NorthConnecting.m_RoomPrefabs[Random.Range(0, m_NorthConnecting.m_RoomPrefabs.Count - 1)], transform.position + new Vector3(-m_RoomSize, 0, 0), this.transform.rotation);
            South.GetComponent<Room>().North = this.gameObject;
        }
        if (East == null && m_ConnectsEast)
        {
            East = GameObject.Instantiate(m_WestConnecting.m_RoomPrefabs[Random.Range(0, m_WestConnecting.m_RoomPrefabs.Count - 1)], transform.position + new Vector3(0, 0, -m_RoomSize), this.transform.rotation);
            East.GetComponent<Room>().West = this.gameObject;
        }
        if (West == null && m_ConnectsWest)
        {
            West = GameObject.Instantiate(m_EastConnecting.m_RoomPrefabs[Random.Range(0, m_EastConnecting.m_RoomPrefabs.Count - 1)], transform.position + new Vector3(0, 0, m_RoomSize), this.transform.rotation);
            West.GetComponent<Room>().East = this.gameObject;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_DisplayGrid)
        {


            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + new Vector3(0, -3, 0), new Vector3(m_RoomSize, 0, m_RoomSize));
            Gizmos.color = Color.red;
            if (North == null)
            {
                Gizmos.DrawCube(transform.position + new Vector3(m_RoomSize, -3, 0), new Vector3(m_RoomSize, 0, m_RoomSize));
            }
            if (South == null)
            {
                Gizmos.DrawCube(transform.position + new Vector3(-m_RoomSize, -3, 0), new Vector3(m_RoomSize, 0, m_RoomSize));
            }
            if (West == null)
            {
                Gizmos.DrawCube(transform.position + new Vector3(0, -3, m_RoomSize), new Vector3(m_RoomSize, 0, m_RoomSize));
            }
            if (East == null)
            {
                Gizmos.DrawCube(transform.position + new Vector3(0, -3, -m_RoomSize), new Vector3(m_RoomSize, 0, m_RoomSize));
            }
        }

    }


}
