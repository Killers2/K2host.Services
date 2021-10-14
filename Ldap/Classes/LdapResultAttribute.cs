/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
'| Inspired By: vforteli Flexinets.Ldap.Core            |
' \====================================================/
*/
using System;

using K2host.Services.Ldap.Enums;

namespace K2host.Services.Ldap.Classes
{

    public class LdapResultAttribute : LdapAttribute
    {
        /// <summary>
        /// Create a new Ldap packet with message id
        /// </summary>
        /// <param name="messageId"></param>
        public LdapResultAttribute(LdapOperation operation, LdapResult result, string matchedDN = "", string diagnosticMessage = "") 
            : base(operation)
        {
            ChildAttributes.Add(new LdapAttribute(UniversalDataType.Enumerated, (byte)result));
            ChildAttributes.Add(new LdapAttribute(UniversalDataType.OctetString, matchedDN));
            ChildAttributes.Add(new LdapAttribute(UniversalDataType.OctetString, diagnosticMessage));
            // todo add referral if needed
            // todo bindresponse can contain more child attributes...
        }
    }

}
