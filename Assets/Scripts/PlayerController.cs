using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    //stwórz zmienn¹ na synchronizowan¹ po sieci zmienn¹ przechowuj¹c¹ pozycjê gracza
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    //funkcj auruchamiana po do³¹czeniu do serwera
    public override void OnNetworkSpawn()
    {
        //tylko jeœli jesteœmy w³aœcicielem w³aœnie zespawnowanego obiektu (gracza)
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        //je¿eli jesteœmy serwerem
        if (NetworkManager.Singleton.IsServer)
        {
            //przesuñ gracza na nowe losowe miejsce
            transform.position = GetRandomPosition();
            //zapisz jego now¹ pozycjê do sieciowo synchronizowanej zmiennej
            Position.Value = transform.position;
        }
        else
        {
            //nie jesteœmy serwerem - wyœlij proœbê o zmianê pozycji
            ServerSideMove();
        }
    }
    [ServerRpc]
    void ServerSideMove(ServerRpcParams rpcParams = default)
    {
        //ta funkcja porusza nas po stronie serwera na nasze rz¹danie
        Position.Value = GetRandomPosition();
    }
    static Vector3 GetRandomPosition()
    {
        float x, y, z;
        //wylosuj wspó³rzêdne mieszcz¹ce siê na p³aszczyŸnie
        x = Random.Range(-5f, 5f);
        y = 1;
        z = Random.Range(-5f, 5f);
        //zwróæ po³o¿enie
        return new Vector3(x, y, z);
    }

    private void Update()
    {
        //w ka¿dej klatce zaktualizuj lokaln¹ pozycjê gracza z sieciowej zmiennej
        transform.position = Position.Value;
    }
}