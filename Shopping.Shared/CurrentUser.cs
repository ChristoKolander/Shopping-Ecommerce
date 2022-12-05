using System.Collections.Generic;


namespace Shopping.Shared
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Claims { get; set; }

    }
}
