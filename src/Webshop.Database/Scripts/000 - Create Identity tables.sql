CREATE TABLE IdentityUsers(
    Id INT NOT NULL,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    EmailConfirmed BOOL NOT NULL,
    SecurityStamp VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    LockoutEnabled BOOL NOT NULL,
    LockoutEnd TIMESTAMP NULL,
    AccessFailedCount INT NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IdentityUserTokens(
    UserId INT NOT NULL,
    LoginProvider VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Value VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    FOREIGN KEY (UserId)
        REFERENCES IdentityUsers (Id)
);

CREATE TABLE IdentityUserLogins(
    UserId INT NOT NULL,
    LoginProvider VARCHAR(255) NOT NULL,
    ProviderKey VARCHAR(255) NOT NULL,
    ProviderDisplayName VARCHAR(255) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    FOREIGN KEY (UserId)
        REFERENCES IdentityUsers (Id)
);

CREATE TABLE IdentityUserClaims(
    Id INT NOT NULL,
    UserId INT NOT NULL,
    ClaimType VARCHAR(255) NOT NULL,
    ClaimValue VARCHAR(255) NOT NULL,
    FOREIGN KEY (UserId)
        REFERENCES IdentityUsers (Id)
);

CREATE TABLE IdentityRoles(
    Id INT NOT NULL,
    Name VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE IdentityRoleClaims(
    Id INT NOT NULL,
    RoleId INT NOT NULL,
    ClaimType VARCHAR(255) NOT NULL,
    ClaimValue VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (RoleId)
        REFERENCES IdentityRoles (Id)
);

CREATE TABLE IdentityUserRoles(
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId)
        REFERENCES IdentityUsers (Id),
    FOREIGN KEY (RoleId)
        REFERENCES IdentityRoles (Id)
);