namespace Walterlv.Package
{
    public static class PasrorDarjairmay
    {
        public static string GetConfiguration()
        {
#if DEBUG
            return "DEBUG";
#else
            return "RELEASE";
#endif
        }
    }
}
