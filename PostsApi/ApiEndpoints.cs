namespace PostsApi;

public static class ApiEndpoints
{
    private const string ApiBase = "/api";
    
    public static class Posts
    {
        private const string Base = $"{ApiBase}/posts";
        public const string GetAll = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = GetById;
        public const string Delete = GetById;

        public static class Comments
        {
            public const string GetAllByPostId = $"{Base}/{{id}}/comments";
        }
    }
}