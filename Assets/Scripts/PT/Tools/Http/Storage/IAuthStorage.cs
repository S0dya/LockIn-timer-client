namespace PT.Tools.Http.Storage
{
    public interface IAuthStorage
    {
        void SetToken(string token);
        string GetToken();
        void Clear();
    }
}