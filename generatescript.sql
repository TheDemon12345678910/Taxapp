DROP TABLE IF EXISTS TaxApp.TaxiCompanies;
DROP TABLE IF EXISTS TaxApp.password_hash;
DROP TABLE IF EXISTS TaxApp.users;

DROP SCHEMA IF EXISTS TaxApp;

CREATE SCHEMA TaxApp;
CREATE TABLE TaxApp.TaxiCompanies
(
    taxiID        SERIAL PRIMARY KEY,
    companyName   varchar(20) NOT NULL,
    startPrice    float       NOT NULL,
    kmPrice       float       NOT NULL,
    minPrice      float       NOT NULL,
    companyImgUrl varchar(1000)
);

INSERT INTO TaxApp.TaxiCompanies (companyName, startPrice, kmPrice, minPrice) VALUES ('FalseTaxi', 25, 13, 7);
INSERT INTO TaxApp.TaxiCompanies (companyName, startPrice, kmPrice, minPrice) VALUES ('MockTaxi', 15, 15, 9);
INSERT INTO TaxApp.TaxiCompanies (companyName, startPrice, kmPrice, minPrice) VALUES ('DefinitelyNotATaxi', 25, 15 ,9);

--the email is uniqe, because there cannot be 2 users with the same email
create table TaxApp.users (
    id          SERIAL          PRIMARY KEY,
    username    VARCHAR(50)     NOT NULL,
    tlfnumber   INT,
    email       VARCHAR(50)     NOT NULL UNIQUE
);

create table TaxApp.password_hash (
    user_id         integer,                            
    hash            VARCHAR(350)    NOT NULL,
    salt            VARCHAR(180)    NOT NULL,
    algorithm       VARCHAR(12)     NOT NULL,
    FOREIGN KEY(user_id) REFERENCES TaxApp.users(id)
);

--adding a tester
--the test password is: 12345678
INSERT INTO TaxApp.users (username, tlfnumber, email)
VALUES ('Tester', 12345678, 'test@mail.com');
INSERT INTO TaxApp.password_hash (user_id, hash, salt, algorithm)
VALUES ((SELECT TaxApp.users.id FROM TaxApp.users WHERE TaxApp.users.email = 'test@mail.com'),
        '1EJybmIbon7kimzpBZXA17OxI3/iVLZK8euSAloQgK3W8ibEJ8G/Ql2J4kjtDDMRV5sN71LEgRuL+lXyP9dOHz9IuMXuWjTdFSwkKaDNbiNa9MsWy/dngKWo04jYvG8Tb26UV0Bnxd83V9zQZCPdPSQXENoRvPOhnDZKaayFYuRz4pVkBrooL9Hu9EgrCzE9Z3kExf+w1BwR/hqVip2wj+W3mxBwTWgm5hhsko1TZqr3d+HWPAeaFmaNTmwuG0miPhA8H9C4/V0mUs62V2zJkZEVP3QEipvTvkCyctxq7U89NSLwVIGiEsmFG/sZ1EqXnXpmpbV1PQ7pkDYFad+pzQ==',
        'sMQAck67hWo2asVpqlbmmGVFj3jo6i86oZVTQh3c3wOpKd0LO8oxqSYhveceXkLrXlCKIIVFB+IRPXrcE3ZkFdVKmG5A7gOyvWwkOltwOytSDoPHmT3+aWUS0sFjO89RMbJxCncsghBbtF3a9hHtr/7/NcexUj8wJQz48gq6izw=',
        'argon2id');

SELECT * FROM TaxApp.users;
SELECT * FROM TaxApp.password_hash;
SELECT * FROM TaxApp.TaxiCompanies;