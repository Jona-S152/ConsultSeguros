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
        public const string InsuranceByInsuredList = "Seguros del asegurado";
        public const string Insurance = "Seguro";
        public const string InsuranceNotFound = "Seguro no encontrado";
        public const string SuccessfulUpdating = "Actualización exitosa";
        public const string SuccessfulRemoval = "Eliminación exitosa";
        public const string IdentificationAlreadyExist = "Ya existe un asegurado con esta identificación";
        public const string InsuredNotFound = "Asegurado no encontrado";
        public const string InsurancesNotFound = "El asegurado no tiene seguros asignados";
        public const string Insured = "Asegurado";
        public const string InsuredList = "Asegurados";
        public const string InsuredListNotFound = "Asegurados no encontrados";
        public const string OutOfLimitFileLength = "El archivo excede el límite de 10MB";
        public const string NotSupportedFormatFile = "Formato de archivo no compatible.";
        public const string EmptyFile = "El archivo se encuentra vacio";
    }
}
