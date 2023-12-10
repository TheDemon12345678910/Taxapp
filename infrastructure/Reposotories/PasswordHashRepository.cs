using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.Reposotories;

/**
 * The layer that is connected to the database, where all the users, passwords and hashes ect. are saved
 */
public class PasswordHashRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PasswordHashRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /**
     * A PasswordHash is returned here. It is found in the DB using the
     * email. The email is in one table, but the userId is in both tables,
     * so the two tables are joined together by the userId, and the PasswordHash is returned.
     */
    public PasswordHash GetByEmail(string email)
    {
        const string sql = $@"
SELECT 
    user_id as {nameof(PasswordHash.UserId)},
    hash as {nameof(PasswordHash.Hash)},
    salt as {nameof(PasswordHash.Salt)},
    algorithm as {nameof(PasswordHash.Algorithm)}
FROM taxapp.password_hash
JOIN taxapp.users ON taxapp.password_hash.user_id = users.id
WHERE email = @email;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingle<PasswordHash>(sql, new { email });
    }

    public void Create(int userId, string hash, string salt, string algorithm)
    {
        try
        {
            const string sql = $@"
                INSERT INTO taxapp.password_hash (user_id, hash, salt, algorithm)
                VALUES (@userId, @hash, @salt, @algorithm)
                ";
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { userId, hash, salt, algorithm });
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not register new user\n" + e.Message);
        }
    }

    public void Update(int userId, string hash, string salt, string algorithm)
    {
        const string sql = $@"
UPDATE taxapp.password_hash
SET hash = @hash, salt = @salt, algorithm = @algorithm
WHERE user_id = @userId
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { userId, hash, salt, algorithm });
    }
}
