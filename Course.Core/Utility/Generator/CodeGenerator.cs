using System;

namespace Course.Core.Utility.Generator
{
    public static class CodeGenerator
    {
        public static string GenerateActiveCode()
        {
            return SequentialGuidGenerator.NewSequentialGuid().ToString().Replace("-", "");
        }

        public static string GenerateUnicCodeForAvatar()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
