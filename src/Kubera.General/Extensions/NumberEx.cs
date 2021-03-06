namespace Kubera.General.Extensions
{
    public static class NumberEx
    {
        public static float ProcentFrom(this decimal previous, decimal current)
        {
            if (previous == 0)
                return 0f;

            if (current == 0)
                return -100f;

            return (float)((current - previous) / previous * 100m);
        }
    }
}
