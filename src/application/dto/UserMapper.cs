using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersFunctionApp.src.domain;

namespace UsersFunctionApp.src.application.dto
{
    public class UserMapper
    {
        public static UserResponseDTO ToUserResponseDTO(User user)
        {
            return new UserResponseDTO(user.Id, user.Name, user.Age, user.Cpf.ToString());
        }
        
    }
}