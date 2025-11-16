using UsersFunctionApp.src.application.dto;

namespace UsersFunctionApp.src.domain.service
{
    public interface IUserService
    {
        UserResponseDTO Create(UserRequestDTO request);
    }
}