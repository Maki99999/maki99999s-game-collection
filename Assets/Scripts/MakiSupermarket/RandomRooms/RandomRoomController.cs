using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakiSupermarket
{
    public class RandomRoomController : MonoBehaviour
    {
        public GameObject[] connections;
        public GameObject[] ends;
        public GameObject[] rooms;

        [Space(10)]
        public int maxRooms = 10;

        Queue<PosRotSca> nextRooms = new Queue<PosRotSca>();
        int roomCount = 0;

        List<Bounds> roomBounds = new List<Bounds>();

        List<Transform> unusedConnections = new List<Transform>();
        List<GameObject> unusedConnectionsEnds = new List<GameObject>();

        void Start()
        {
            StartCoroutine(CreateRooms(new PosRotSca(transform)));
        }

        void Update()
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.up * 10, Color.red);
        }

        IEnumerator CreateRooms(PosRotSca startRoom)
        {
            yield return null;
            nextRooms.Enqueue(startRoom);

            while (nextRooms.Count > 0 && roomCount < maxRooms)
            {
                //yield return null;

                PosRotSca newPos = nextRooms.Dequeue();

                //Add connection
                GameObject conn = Instantiate(connections[Random.Range(0, connections.Length)], newPos.position, newPos.rotation, transform);
                conn.transform.Rotate(0, 180, 0);
                RandomRoomConnection connScript = conn.GetComponent<RandomRoomConnection>();
                newPos = new PosRotSca(connScript.connection);


                List<GameObject> possibleNewRooms = new List<GameObject>();
                foreach (GameObject gameObject in rooms)
                {
                    possibleNewRooms.Add(gameObject);
                }

                GameObject newRoom = null;

                while (newRoom == null && possibleNewRooms.Count > 0)
                {
                    int newRoomInt = Random.Range(0, possibleNewRooms.Count);
                    GameObject newRoomListItem = possibleNewRooms[newRoomInt];
                    newRoom = Instantiate(newRoomListItem, transform);
                    newRoom.name = "Room " + (roomCount + 1);
                    RandomRoom randomNewRoom = newRoom.GetComponent<RandomRoom>();

                    List<Transform> possibleNextConnections = new List<Transform>();
                    foreach (Transform connection in randomNewRoom.connections)
                    {
                        possibleNextConnections.Add(connection);
                    }

                    Transform nextConnection = null;

                    while (nextConnection == null && possibleNextConnections.Count > 0)
                    {
                        int nextConnectionInt = Random.Range(0, possibleNextConnections.Count);
                        nextConnection = possibleNextConnections[nextConnectionInt];

                        newRoom.transform.rotation = Quaternion.Euler(newPos.eulerAngles + (newRoom.transform.eulerAngles - nextConnection.eulerAngles));
                        newRoom.transform.position = newPos.position + (newRoom.transform.position - nextConnection.position);

                        bool isIntersecting = false;

                        bool breakOut = false;
                        foreach (Bounds bounds in roomBounds)
                        {
                            foreach (Bounds boundsNewRoom in randomNewRoom.GetBounds())
                            {
                                if (bounds.Intersects(boundsNewRoom))
                                {
                                    isIntersecting = true;
                                    breakOut = true;
                                    break;
                                }
                            }
                            if (breakOut)
                                break;
                        }

                        if (isIntersecting)
                        {
                            possibleNextConnections.Remove(nextConnection);
                            nextConnection = null;
                        }
                        else
                        {
                            roomCount++;
                            roomBounds.AddRange(randomNewRoom.GetBounds());

                            float randomChance = 1f;
                            foreach (Transform connection in randomNewRoom.connections)
                            {
                                if (connection != nextConnection)
                                {
                                    if (Random.value <= randomChance)
                                    {
                                        nextRooms.Enqueue(new PosRotSca(connection));
                                        randomChance /= 2f;
                                    }
                                    else
                                    {
                                        unusedConnections.Add(connection);
                                        unusedConnectionsEnds.Add(
                                        Instantiate(ends[Random.Range(0, ends.Length)], connection.position,
                                                Quaternion.Euler(connection.eulerAngles + (Vector3.up * 180)), transform));
                                    }
                                }
                            }
                        }
                    }

                    if (nextConnection == null)
                    {
                        possibleNewRooms.Remove(newRoomListItem);
                        Destroy(newRoom);
                        newRoom = null;
                    }
                }

                if (newRoom == null)
                {
                    Instantiate(ends[Random.Range(0, ends.Length)], conn.transform.position, conn.transform.rotation, transform);
                    Destroy(conn);
                }

                if (nextRooms.Count <= 0 && roomCount < maxRooms)
                {
                    Debug.Log("Keine Connections mehr!");
                    int newRandomUnusedConnection = Random.Range(0, unusedConnections.Count);
                    Destroy(unusedConnectionsEnds[newRandomUnusedConnection]);
                    nextRooms.Enqueue(new PosRotSca(unusedConnections[newRandomUnusedConnection]));
                }
            }
            while (nextRooms.Count > 0)
            {
                PosRotSca newPos = nextRooms.Dequeue();
                GameObject conn = Instantiate(ends[Random.Range(0, ends.Length)], newPos.position, Quaternion.Euler(newPos.eulerAngles  + (Vector3.up * 180)), transform);
            }
        }
    }
}