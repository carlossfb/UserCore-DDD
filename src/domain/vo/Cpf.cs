using System.Text.RegularExpressions;
using UsersFunctionApp.src.domain.exception;

namespace UsersFunctionApp.src.domain
{
    public sealed record Cpf
  {
        public string Document { get; private set; }

        public Cpf(string document)
        {
            Document = ValidateAndNormalize(document);
        }

        private string ValidateAndNormalize(string document)
        {
            var digits = Regex.Replace(document ?? "", "[^0-9]", "");

            if (string.IsNullOrWhiteSpace(digits))
                throw new DomainException("CPF não pode ser vazio");

            if (digits.Length != 11)
                throw new DomainException("CPF deve conter 11 dígitos");

            if (digits.All(d => d == digits[0]))
                throw new DomainException("CPF inválido");

            var numbers = digits.Select(n => int.Parse(n.ToString())).ToList();


            int firstSum = 0;
            for (int i = 0; i < 9; i++)
                firstSum += numbers[i] * (10 - i);

            Math.DivRem(firstSum, 11, out int firstRest);
            int firstDigit = firstRest < 2 ? 0 : 11 - firstRest;

            if (numbers[9] != firstDigit)
                throw new DomainException("CPF inválido");

            int secondSum = 0;
            for (int i = 0; i < 10; i++)
                secondSum += numbers[i] * (11 - i);

            Math.DivRem(secondSum, 11, out int secondRest);
            int secondDigit = secondRest < 2 ? 0 : 11 - secondRest;

            if (numbers[10] != secondDigit)
                throw new DomainException("CPF inválido");

            return digits;
        }
        public override string ToString()
        {
            if (Document.Length == 11)
                return $"{Document[..3]}.{Document.Substring(3, 3)}.{Document.Substring(6, 3)}-{Document[9..]}";

            return Document;
        }
    }
}