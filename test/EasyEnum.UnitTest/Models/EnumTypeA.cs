
namespace EasyEnum.UnitTest.Models
{
    using System;
    using System.ComponentModel;

    [Flags]
    public enum EnumTypeA
    {
        [Description("Two")]
        A = 2,
        B = 4
    }

}
