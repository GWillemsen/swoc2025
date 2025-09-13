namespace swoq2025;

public class Player(Coord position, Swoq.Interface.Inventory inventory)
{
    public Coord Position { get; set; } = position;

    public Swoq.Interface.Inventory Inventory { get; set; } = inventory;
}