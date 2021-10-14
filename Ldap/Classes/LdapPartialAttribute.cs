/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
'| Inspired By: vforteli Flexinets.Ldap.Core            |
' \====================================================/
*/
using System.Linq;
using System.Collections.Generic;

using K2host.Services.Ldap.Enums;

namespace K2host.Services.Ldap.Classes
{
    /// <summary>
    /// Convenience class for creating PartialAttributes
    /// </summary>
    public class LdapPartialAttribute : LdapAttribute
    {
        /// <summary>
        /// Partial attribute description
        /// </summary>
        public string Description => (string)ChildAttributes.FirstOrDefault().GetValue();

        /// <summary>
        /// Partial attribute values
        /// </summary>
        public List<string> Values => ChildAttributes[1].ChildAttributes.Select(o => (string)o.GetValue()).ToList();

        /// <summary>
        /// Create a partial Attribute from list of values
        /// </summary>
        /// <param name="attributeDescription"></param>
        /// <param name="attributeValues"></param>
        public LdapPartialAttribute(string attributeDescription, IEnumerable<string> attributeValues) 
            : base(UniversalDataType.Sequence)
        {
           
            ChildAttributes.Add(new LdapAttribute(UniversalDataType.OctetString, attributeDescription));

            LdapAttribute value = new(UniversalDataType.Set);

            value.ChildAttributes.AddRange(attributeValues.Select(o => new LdapAttribute(UniversalDataType.OctetString, o)));
           
            ChildAttributes.Add(value);

        }


        /// <summary>
        /// Create a partial attribute with a single value
        /// </summary>
        /// <param name="attributeDescription"></param>
        /// <param name="attributeValue"></param>
        public LdapPartialAttribute(string attributeDescription, string attributeValue) 
            : this(attributeDescription, new List<string> { attributeValue })
        {

        }
    }
}
