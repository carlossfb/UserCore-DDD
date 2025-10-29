using UsersFunctionApp.src.domain.exception;

namespace UsersFunctionApp.src.domain
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Cpf { get; private set; }

        private User(string name, int age, string cpf)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
            Cpf = new Cpf(cpf).ToString();
        }

        public static User Create(string name, int age, string cpf)

        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required");

            if (int.IsNegative(age))
                throw new DomainException("Age cannot be negative");

            var user = new User(name, age, cpf);

            return user;  
        }
    }
}