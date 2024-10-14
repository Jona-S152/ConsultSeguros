using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class MessageResponse
    {
        public const string InsuranceAlredyExist = "El seguro ya se encuentra registrado";
        public const string NoValidFields = "Campo(s) no valido(s)";
        public const string NegativeValues = "No se admiten valores negativos";
        public const string MaxAmount = "El valor supera el máximo, ingrese un valor mas bajo";
        public const string EmptyFields = "Campo(s) vacio(s)";
        public const string SuccessfulRegistration = "Registro exitoso";
        public const string InsuranceList = "Seguros";
        public const string Insurance = "Seguro";
        public const string InsuranceNotFound = "Seguro no encontrado";
        public const string SuccessfulUpdating = "Actualización exitosa";
        public const string SuccessfulRemoval = "Eliminación exitosa";
    }
}
