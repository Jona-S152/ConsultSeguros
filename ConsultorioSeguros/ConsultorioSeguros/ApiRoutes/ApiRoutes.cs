namespace ConsultorioSeguros.ApiRoutes
{
    public static class ApiRoutes
    {
        public static class Insurance
        {
            public const string Add = "Add";
            public const string GetById = "Get/{id}";
            public const string GetAllInsurances = "GetAllInsurances";
            public const string UpdateById = "Update/{id}";
            public const string DeleteById = "Delete/{id}";
            public const string GetByCode = "GetByCode/{code}";
        }
    }
}
