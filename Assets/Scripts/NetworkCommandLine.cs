using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkCommandLine : MonoBehaviour
{
    NetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponentInParent<NetworkManager>();
        //je¿li pracujemy wedytorze to nie rób nic dalej
        if (Application.isEditor) return;

        var args = CommandLineArgs();
        //zobacz czy istnieje argument o nazwie -mode i jeœli jest zapisz jego wartoœæ do zmiennej mode
        if(args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    nm.StartServer();
                    break;
                case "host":
                    nm.StartHost(); 
                    break;
                case "client":
                    nm.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> CommandLineArgs()
    {
        //tworzymy pusty slownik
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();
        //wyciagamy sobie z systemu wszystkie argumenty
        var args = System.Environment.GetCommandLineArgs();
        //parsujemy argumenty
        for(int i = 0; i < args.Length; i++)
        {
            //jeden argument
            var arg = args[i].ToLower();
            //je¿eli argument zaczyna siê od "-" to jest to nazwa
            if(arg.StartsWith("-"))
            {
                //inicjujemy pust¹ wartoœci¹
                string value = "";
                //wartoœæ argumentu to nastêpny argument po nazwie
                //je¿eli nie jest to ostatni argument 
                if(i < args.Length)
                {
                    value = args[i + 1].ToLower();
                    i++;
                    argDictionary.Add(arg, value);
                } 
                else
                {
                    argDictionary.Add(arg, null);
                }
                    

            }

        }
        return argDictionary;
    }
}
