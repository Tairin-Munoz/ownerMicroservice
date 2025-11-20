using ownerMicroservice.Domain.Entities;
using System.Text.RegularExpressions;

namespace ownerMicroservice.Domain.Services;

public class OwnerValidator : IValidator<Owner>
{
    public Result Validate(Owner entity)
    {
        var errors = new List<string>();

        var name = entity.Name?.Trim() ?? string.Empty;
        var firstLastName = entity.FirstLastname?.Trim() ?? string.Empty;
        var secondLastName = entity.SecondLastname?.Trim();
        var email = entity.Email?.Trim() ?? string.Empty;
        var documentNumber = entity.DocumentNumber?.Trim() ?? string.Empty;
        var documentExt = entity.DocumentExtension?.Trim();
        var address = entity.Address?.Trim() ?? string.Empty;

        ValidateName(name, errors);
        ValidateFirstLastName(firstLastName, errors);
        ValidateSecondLastName(secondLastName, errors);
        ValidatePhoneNumber(entity.PhoneNumber, errors);
        ValidateEmail(email, errors);
        ValidateDocumentNumber(documentNumber, errors);
        ValidateDocumentExtension(documentExt, errors);
        ValidateAddress(address, errors);

        return errors.Count == 0 ? Result.Success() : Result.Failure(errors);
    }

    private static void ValidateName(string name, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("El nombre es requerido.");
            return;
        }

        if (name.Length < 3) errors.Add("El nombre debe tener al menos 3 caracteres.");
        if (name.Length > 100) errors.Add("El nombre no puede superar los 100 caracteres.");

        if (!char.IsLetter(name[0])) errors.Add("El nombre debe comenzar con una letra.");

        // Solo letras, espacios y tildes comunes
        var rx = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$");
        if (!rx.IsMatch(name)) errors.Add("El nombre solo puede contener letras y espacios.");

        ProhibitDangerousChars(name, "nombre", errors);
    }

    private static void ValidateFirstLastName(string lastName, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add("El apellido paterno es requerido.");
            return;
        }

        if (lastName.Length < 2) errors.Add("El apellido paterno debe tener al menos 2 caracteres.");
        if (lastName.Length > 100) errors.Add("El apellido paterno no puede superar los 100 caracteres.");

        var rx = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$");
        if (!rx.IsMatch(lastName)) errors.Add("El apellido paterno solo puede contener letras y espacios.");

        ProhibitDangerousChars(lastName, "apellido paterno", errors);
    }

    private static void ValidateSecondLastName(string? lastName, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(lastName)) return;

        if (lastName!.Length < 2) errors.Add("El apellido materno debe tener al menos 2 caracteres.");
        if (lastName.Length > 100) errors.Add("El apellido materno no puede superar los 100 caracteres.");

        var rx = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$");
        if (!rx.IsMatch(lastName)) errors.Add("El apellido materno solo puede contener letras y espacios.");

        ProhibitDangerousChars(lastName, "apellido materno", errors);
    }

    private static void ValidatePhoneNumber(int phone, List<string> errors)
    {
        // Bolivia: exactamente 8 dígitos
        if (phone <= 0)
        {
            errors.Add("El teléfono es requerido.");
            return;
        }

        var text = phone.ToString();
        if (text.Length != 8) errors.Add("El teléfono debe tener exactamente 8 dígitos.");
        if (!Regex.IsMatch(text, @"^\d{8}$")) errors.Add("El teléfono solo admite dígitos.");
    }

    private static void ValidateEmail(string email, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("El correo electrónico es requerido.");
            return;
        }

        if (email.Length > 255) errors.Add("El correo electrónico no puede superar los 255 caracteres.");

        // Patrón de email razonable
        var rx = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!rx.IsMatch(email)) errors.Add("Ingrese un correo electrónico válido.");

        ProhibitDangerousChars(email, "correo electrónico", errors);
    }

    private static void ValidateDocumentNumber(string doc, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(doc))
        {
            errors.Add("El número de documento (CI) es requerido.");
            return;
        }

        if (!Regex.IsMatch(doc, @"^\d{6,10}$"))
            errors.Add("El CI debe contener entre 6 y 10 dígitos.");
    }

    private static void ValidateDocumentExtension(string? ext, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(ext)) return;

        if (ext!.Length != 2)
            errors.Add("El complemento del CI debe tener exactamente 2 caracteres.");

        if (!Regex.IsMatch(ext, @"^[1-9][A-Z]$"))
            errors.Add("Formato de complemento inválido. Use dígito 1-9 seguido de letra mayúscula (ej. 1A).");
    }

    private static void ValidateAddress(string address, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            errors.Add("La dirección es requerida.");
            return;
        }

        if (address.Length < 5) errors.Add("La dirección debe tener al menos 5 caracteres.");
        if (address.Length > 200) errors.Add("La dirección no puede superar los 200 caracteres.");

        ProhibitDangerousChars(address, "dirección", errors);
    }

    private static void ProhibitDangerousChars(string value, string fieldName, List<string> errors)
    {
        var prohibited = new[] { '<', '>', '/', '\\', '|' };
        if (value.Any(c => prohibited.Contains(c)))
            errors.Add($"El campo {fieldName} contiene caracteres no permitidos: < > / \\ |");
    }
}
