[System.Flags]
public enum E_INVADERSTATE
{
    NONE = 0, // Justin Case
    CLEAN = 1,
    VOMIT = 1 << 1,    // 0110 = 6
    POOP = 1 << 2,
}
