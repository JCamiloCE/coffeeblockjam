namespace Enums
{
    //Each component of the enum need a identification
    //The identification must be the index of the first three letters
    //Index: A:00, B:01,...,Y:25,Z:25
    //Note: the  index -1 is reserved for Invalid and the index 0 for None
    public enum ETypeTray
    {
        Invalid = -1,
        None = 0,
        NoWalls = 131422, 
        OneWall = 141304, 
        TwoWalls = 192214, 
        ThreeWalls = 190717, 
        FourWall = 051421, 
        OppositeWall = 141515
    }
}
