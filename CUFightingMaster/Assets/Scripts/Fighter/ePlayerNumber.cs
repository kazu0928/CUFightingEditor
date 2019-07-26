public enum PlayerNumber
{
	Player1,
	Player2,
	Player3,
	Player4,
	None,
}

public enum PlayerDirection
{
	Right,
	Left,
}
public enum SkillStatus
{
    Normal = 1 << 0,
    Special = 1 << 1,
    Unique = 1 << 2,
    Critical = 1 << 3,
    Move = 1 << 4,
}

public enum PlayerMoveState
{
    Idle,
    Front_Jump,
    Back_Jump,
    Jump,
    Crouching,
	Front_Walk,
	Back_Walk,
}

public enum Direction
{
    Neutral,
    Front,
    Back,
    Up,
    Down,
	UpFront,
	UpBack,
	DownFront,
	DownBack,
}

public enum HitPoint
{
	Top,
	Middle,
	Bottom,
}

public enum HitMode
{
	Normal,
	Grab,
}

public enum HitStrength
{
	Light,
	Middle,
	Strong,
}