namespace MSTest.Extensions.Utils
{
    /// <summary>
    /// The member name in tesetfx, used for reflection
    /// </summary>
    public static class MSTestMemberName
    {
        public const string TestMethodInfoPropertyParent = "Parent";
        public const string TestClassInfoPropertyBaseTestInitializeMethodsQueue = "BaseTestInitializeMethodsQueue";
        public const string TestClassInfoPropertyBaseTestCleanupMethodsQueue = "BaseTestCleanupMethodsQueue";
        public const string TestClassInfoFieldTestInitializeMethod = "testInitializeMethod";
        public const string TestClassInfoFieldTestCleanupMethod = "testCleanupMethod";
    }
}
