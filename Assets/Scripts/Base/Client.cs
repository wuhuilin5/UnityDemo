using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Client : Singleton<Client>
{
    public void Start()
    {
        Debug.Log("Client.Start");
    }
}
