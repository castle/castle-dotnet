#if !NET48
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Tests.SetUp;

public class RequestCookieCollection : Dictionary<string, string>, IRequestCookieCollection
{
    public new ICollection<string> Keys => base.Keys;
    public new string this[string key]
    {
        get
        {
            TryGetValue(key, out var value);
            return value;
        }
        set
        {
            base[key] = value;
        }
    }
}
#endif
