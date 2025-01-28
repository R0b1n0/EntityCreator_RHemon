using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Knight", fileName ="Knight")]
public class SoKnight : SoEntity
{
    public int faithPoints;
    [SerializeField]
    private string WarCry;

    private string FavoriteWarCrime;
}
