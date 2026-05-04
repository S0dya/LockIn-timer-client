namespace PT.Tools.Http
{
    public interface IAuthStorage
    {
        void SetToken(string token);
        string GetToken();
        void Clear();
    }
}