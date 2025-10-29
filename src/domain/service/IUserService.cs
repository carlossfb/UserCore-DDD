namespace UsersFunctionApp.src.domain.service
{
    public interface IUserService
    {
        User Create(string name, int age, string cpf);
    }
}