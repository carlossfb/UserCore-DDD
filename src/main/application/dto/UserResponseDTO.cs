using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersFunctionApp.src.application.dto
{
    public record UserResponseDTO(Guid Id, string Name, int Age, string Cpf);
}