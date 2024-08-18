using Displayable;
using UnityEngine.UI;

public class PetFoodDisplay : ListDisplay<Pet, Food, Image>
{
    public override void UpdateGraphics()
    {
        var foods = displayObject as IListDisplayable<Food>;
        for (int i = 0; i < graphicInstances.Count; i++)
        {
            graphicInstances[i].sprite = foods[i].sprite;
        }
    }
}