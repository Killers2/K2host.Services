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
    // Ldap operations from https://tools.ietf.org/html/rfc4511#section-4.2
    public enum LdapOperation
    {
        BindRequest = 0,
        BindResponse = 1,
        UnbindRequest = 2,
        SearchRequest = 3,
        SearchResultEntry = 4,
        SearchResultDone = 5,
        ModifyRequest = 6,
        ModifyResponse = 7,
        AddRequest = 8,
        AddResponse = 9,
        DelRequest = 10,
        DelResponse = 11,
        ModifyDNRequest = 12,
        ModifyDNResponse = 13,
        CompareRequest = 14,
        CompareResponse = 15,
        AbandonRequest = 16,
        //SearchResultReference = 17,
        //ExtendedRequest = 18,
        //ExtendedResponse = 19,
        //IntermediateResponse = 20,
        SearchResultReference = 19,
        ExtendedRequest = 23,
        ExtendedResponse = 24,
        IntermediateResponse = 25
    }
}
