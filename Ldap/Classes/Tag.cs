﻿/*
' /====================================================\
'| Developed Tony N. Hyde (www.k2host.co.uk)            |
'| Projected Started: 2019-11-01                        | 
'| Use: General                                         |
'| Inspired By: vforteli Flexinets.Ldap.Core            |
' \====================================================/
*/
using System.Collections;

using K2host.Services.Ldap.Enums;

namespace K2host.Services.Ldap.Classes
{
    public class Tag
    {
        /// <summary>
        /// Tag in byte form
        /// </summary>
        public byte TagByte { get; internal set; }

        public bool IsConstructed
        {
            get
            {
                return (TagByte & (1 << 5)) != 0;
            }
            set
            {
                var foo = new BitArray(new byte[] { TagByte });
                foo.Set(5, value);
                var temp = new byte[1];
                foo.CopyTo(temp, 0);
                TagByte = temp[0];
            }
        }

        public TagClass Class => (TagClass)(TagByte >> 6);
        
        public UniversalDataType? DataType => Class == TagClass.Universal ? (UniversalDataType?)(TagByte & 31) : null;
        
        public LdapOperation? LdapOperation => Class == TagClass.Application ? (LdapOperation?)(TagByte & 31) : null;
        
        public byte? ContextType => Class == TagClass.Context ? (byte?)(TagByte & 31) : null;
       
        private Tag() { }

        /// <summary>
        /// Create an application tag
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="isSequence"></param>
        public Tag(LdapOperation operation)
        {
            TagByte = (byte)((byte)operation + ((byte)TagClass.Application << 6));
        }

        /// <summary>
        /// Create a universal tag
        /// </summary>
        /// <param name="isSequence"></param>
        /// <param name="operation"></param>
        public Tag(UniversalDataType dataType)
        {
            TagByte = (byte)(dataType + ((byte)TagClass.Universal << 6));
        }

        /// <summary>
        /// Create a context tag
        /// </summary>
        /// <param name="isSequence"></param>
        /// <param name="operation"></param>
        public Tag(byte context)
        {
            TagByte = (byte)(context + ((byte)TagClass.Context << 6));
        }

        /// <summary>
        /// Parses a raw tag byte
        /// </summary>
        /// <param name="tagByte"></param>
        /// <returns></returns>
        public static Tag Parse(byte tagByte)
        {
            return new Tag { TagByte = tagByte };
        }

    }
}
