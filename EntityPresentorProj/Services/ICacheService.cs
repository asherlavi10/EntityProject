namespace EntityPresentorProj.Services
{
    public interface ICacheService
    {
        public object GetValue(string key);
        public void SetValue(string key,byte[] val);
        public void SetStringValue(string key, string val);
        public string GetValueAsString(string key);
    }
}
