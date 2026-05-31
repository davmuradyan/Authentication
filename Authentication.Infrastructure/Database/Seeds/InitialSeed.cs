using Authentication.Infrastructure;

namespace Authentication.Infrastructure.Database.Seeds;

public static class InitialSeed
{
    public static string Up() => @"
    INSERT INTO Roles (Id, Name, Description)
    VALUES ('a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'global-admin', 'Administrator with full access.');

    INSERT INTO Permissions (Id, Name, Description) VALUES
    ('c377d523-bd53-4543-b08e-00121595c9b7', 'Company.Create', 'Allows to create schools.'),
    ('a4e32294-5970-4dac-a637-aecd75d0d45d', 'Company.Read',   'Allows to view schools.'),
    ('876ebd02-68d0-4285-ae7b-9e698ecf9edf', 'Company.Update', 'Allows to update schools.'),
    ('32b20cee-15fa-4e4e-aa79-e799d886c567', 'Company.Delete', 'Allows to delete schools.');

    INSERT INTO RolePermissions (RoleId, PermissionId) VALUES
    ('a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'c377d523-bd53-4543-b08e-00121595c9b7'),
    ('a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'a4e32294-5970-4dac-a637-aecd75d0d45d'),
    ('a1b2c3d4-e5f6-7890-abcd-ef1234567890', '876ebd02-68d0-4285-ae7b-9e698ecf9edf'),
    ('a1b2c3d4-e5f6-7890-abcd-ef1234567890', '32b20cee-15fa-4e4e-aa79-e799d886c567');

    INSERT INTO Users (Id, Email, PasswordHash, IsEmailConfirmed, IsActive, CreatedAt, UpdatedAt)
    VALUES ('f9e8d7c6-b5a4-3210-fedc-ba9876543210', 'info.muradyan01@gmail.com', '$2a$12$MZFf1wuNBKwG0YF0Q1uOw.ggrnCnQwUd4pvMLvwuC7s.CxLpT80Wu', 1, 1, '2025-01-01', '2025-01-01');

    INSERT INTO UserRoles (UserId, RoleId, AssignedAt)
    VALUES ('f9e8d7c6-b5a4-3210-fedc-ba9876543210', 'a1b2c3d4-e5f6-7890-abcd-ef1234567890', '2025-01-01');

    INSERT INTO CompanyUsers (Id, UserId, Name, Surname, MiddleName)
    VALUES ('12345678-abcd-ef90-1234-567890abcdef', 'f9e8d7c6-b5a4-3210-fedc-ba9876543210', 'Davit', 'Muradyan', 'Kareni');
";
}