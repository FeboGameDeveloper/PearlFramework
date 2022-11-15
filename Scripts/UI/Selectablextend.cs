using UnityEngine.UI;

public static class Selectablextend
{
    public static void SetNavigation(this Selectable selectable, Selectable selectableUp, Selectable selectableRight, Selectable selectableLeft, Selectable selectableDown)
    {
        if (selectable != null)
        {
            selectable.SetUpNavigation(selectableUp);
            selectable.SetRightNavigation(selectableRight);
            selectable.SetDownNavigation(selectableDown);
            selectable.SetLeftNavigation(selectableLeft);
        }
    }


    public static void SetUpNavigation(this Selectable selectable, Selectable result)
    {
        if (selectable != null)
        {
            Navigation aux = selectable.navigation;
            aux.selectOnUp = result;
            selectable.navigation = aux;
        }
    }


    public static void SetRightNavigation(this Selectable selectable, Selectable result)
    {
        if (selectable != null)
        {
            Navigation aux = selectable.navigation;
            aux.selectOnRight = result;
            selectable.navigation = aux;
        }
    }


    public static void SetDownNavigation(this Selectable selectable, Selectable result)
    {
        if (selectable != null)
        {
            Navigation aux = selectable.navigation;
            aux.selectOnDown = result;
            selectable.navigation = aux;
        }
    }


    public static void SetLeftNavigation(this Selectable selectable, Selectable result)
    {
        if (selectable != null)
        {
            Navigation aux = selectable.navigation;
            aux.selectOnLeft = result;
            selectable.navigation = aux;
        }
    }

}
