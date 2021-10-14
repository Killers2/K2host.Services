/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
'| Inspired By: vforteli Flexinets.Ldap.Core            |
' \====================================================/
*/

namespace K2host.Services.Ldap.Enums
{
    public enum LdapFilterChoice
    {
        and = 0,
        or = 1,
        not = 2,
        equalityMatch = 3,
        substrings = 4,
        greaterOrEqual = 5,
        lessOrEqual = 6,
        present = 7,
        approxMatch = 8,
        extensibleMatch = 9,
    }
}
