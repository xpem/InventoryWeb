using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Resps
{
    public class ServResp
    {
        public bool Success { get; set; }

        public object? Content { get; set; }

        public bool TryRefreshToken { get; set; } = false;

        public ErrorTypes? Error { get; set; }

        public string ErrorMessage => Error switch
        {
            ErrorTypes.ServerUnavaliable => "Servidor indisponível",
            ErrorTypes.WrongEmailOrPassword => "Senha ou email inválidos",
            null => string.Empty,
            _ => throw new NotImplementedException("Erro não mapeado")
        };
    }
}
