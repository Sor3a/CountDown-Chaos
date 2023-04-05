using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuTypes
{
    Loading,
    General,
    CreateRoom,
    JoinedRoom,
    nickNameRoom,
}
public class Menu : MonoBehaviour
{
    public MenuTypes type;
}
