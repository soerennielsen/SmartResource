// Guids.cs
// MUST match guids.h
using System;

namespace DelegateAS.SmartResource
{
    static class GuidList
    {
        public const string GuidSmartResourcePkgString = "2e4a7618-f19e-422a-8c5f-f8a564806db6";
        public const string GuidSmartResourceCmdSetString = "28a9dad9-3d84-4a98-bb99-2ee8af906349";
        public static readonly Guid GuidSmartResourceCmdSet = new Guid(GuidSmartResourceCmdSetString);
    };
}