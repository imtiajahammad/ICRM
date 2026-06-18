using System.Runtime.Serialization;

namespace ICRM.Model.Enums;

public enum Gender
{
    [EnumMember(Value ="Male")]
    Male,
    [EnumMember(Value ="Female")]    
    Female,
    [EnumMember(Value ="Other")]
    Other
}
