using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] MenuVisibility[] menus;

    public void Awake()
    {
        Instance = this;
    }

    public void OpenMenuName(string menuName)
    {
        for (int i = 0; i < menus.Length; i++) 
        {
            if (menus[i].nameMenu == menuName)
            {
                menus[i].Visible();
            }
            else if (menus[i].isOpen)
            {
                closeMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(MenuVisibility menuMenu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                closeMenu(menus[i]);
            }

            menuMenu.Visible();
        }
    }

    private void closeMenu(MenuVisibility menu)
    {
        menu.NoVisible(); 
    }
}