using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Repository.SitefinityCMS.OrchardCore
{
    public class OrchardCoreIdGenerator : IIdGenerator
    {
        private static readonly string Encode32Chars = "0123456789abcdefghjkmnpqrstvwxyz";

        public string GenerateUniqueId()
        {
            byte[] value = Guid.NewGuid().ToByteArray();
            long hs = BitConverter.ToInt64(value, 0);
            long ls = BitConverter.ToInt64(value, 8);
            return ToBase32(hs, ls);
        }

        private static string ToBase32(long hs, long ls)
        {
            return new string(new char[26]
            {
                Encode32Chars[(int)(hs >> 60) & 0x1F],
                Encode32Chars[(int)(hs >> 55) & 0x1F],
                Encode32Chars[(int)(hs >> 50) & 0x1F],
                Encode32Chars[(int)(hs >> 45) & 0x1F],
                Encode32Chars[(int)(hs >> 40) & 0x1F],
                Encode32Chars[(int)(hs >> 35) & 0x1F],
                Encode32Chars[(int)(hs >> 30) & 0x1F],
                Encode32Chars[(int)(hs >> 25) & 0x1F],
                Encode32Chars[(int)(hs >> 20) & 0x1F],
                Encode32Chars[(int)(hs >> 15) & 0x1F],
                Encode32Chars[(int)(hs >> 10) & 0x1F],
                Encode32Chars[(int)(hs >> 5) & 0x1F],
                Encode32Chars[(int)hs & 0x1F],
                Encode32Chars[(int)(ls >> 60) & 0x1F],
                Encode32Chars[(int)(ls >> 55) & 0x1F],
                Encode32Chars[(int)(ls >> 50) & 0x1F],
                Encode32Chars[(int)(ls >> 45) & 0x1F],
                Encode32Chars[(int)(ls >> 40) & 0x1F],
                Encode32Chars[(int)(ls >> 35) & 0x1F],
                Encode32Chars[(int)(ls >> 30) & 0x1F],
                Encode32Chars[(int)(ls >> 25) & 0x1F],
                Encode32Chars[(int)(ls >> 20) & 0x1F],
                Encode32Chars[(int)(ls >> 15) & 0x1F],
                Encode32Chars[(int)(ls >> 10) & 0x1F],
                Encode32Chars[(int)(ls >> 5) & 0x1F],
                Encode32Chars[(int)ls & 0x1F]
            });
        }
    }
}