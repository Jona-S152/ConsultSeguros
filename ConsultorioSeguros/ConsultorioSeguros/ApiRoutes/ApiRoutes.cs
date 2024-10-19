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
            public const string GetAllInsuredsByInsurance = "GetAllInsuredsByInsurance/{code}";
        }

        public static class Insured
        {
            public const string Add = "Add";
            public const string UpdateById = "Update/{id}";
            public const string GetByIdentification = "GetByIdentification/{identification}";
            public const string GetById = "GetById/{id}";
            public const string GetAll = "GetAll";
            public const string DeleteById = "DeleteInsured/{id}";
            public const string GetAllInsurancesByInsured = "GetInsuranceByInsured/{identification}"
        }
    }
}
