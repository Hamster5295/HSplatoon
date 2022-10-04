using Godot;

public enum Team
{
    BLUE, RED, GREEN, YELLOW
}

public class TeamUtils
{
    private static Color swift = new Color(0.2f, 0.2f, 0.2f, 0);

    public static Color GetColor(Team t)
    {
        switch (t)
        {
            case Team.BLUE:
                return new Color("#11C2EE");


            case Team.RED:
                return new Color("#F37150");

            case Team.GREEN:
                return new Color("#88FA6B");

            case Team.YELLOW:
                return new Color("#F6DE80");
        }

        return Colors.Black;
    }

    public static Color GetDarkColor(Team t)
    {
        return GetColor(t) - swift;
    }

    public static Color GetLightColor(Team t)
    {
        return GetColor(t) + swift;
    }
}