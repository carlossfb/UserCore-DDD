using UsersFunctionApp.src.application.dto;
using UsersFunctionApp.src.domain;
using UsersFunctionApp.src.domain.service;

namespace UsersFunctionApp.src.application.service
{
    public class UserServiceImpl : IUserService
    {

        public UserResponseDTO Create(UserRequestDTO request)
        {
            var user = User.Create(request.Name, request.Age, request.Cpf);
            return UserMapper.ToUserResponseDTO(user);
        }
    }
}