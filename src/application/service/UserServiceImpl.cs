using UsersFunctionApp.src.domain;
using UsersFunctionApp.src.domain.service;

namespace UsersFunctionApp.src.application.service
{
    public class UserServiceImpl : IUserService
    {
        public User Create(string name, int age, string cpf)
        {
            var user = User.Create(name, age, cpf);
            return user;
        }
    }
}