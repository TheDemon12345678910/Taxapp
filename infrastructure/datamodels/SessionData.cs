namespace infrastructure.datamodels;

public class SessionData
{
    public required int userid { get; init; }

    /**
     * When a user object is provided, a new instance of the SessionData is created.
     * It can be used from other classes, like this:
        User user = new User {Id = 456};
        SessionData session = SessionData.FromUser(user);
     * Creating a SessionData object from the User object
     */
    public static SessionData FromUser(User user)
    {
        return new SessionData { userid = user.id};
    }

    //Dictionary is like "key value" datastructures and like hash maps
    public static SessionData FromDictionary(Dictionary<string, object> dict)
    {
        //UserId is the object in the form of an int, and the Keys.UserId is the string
        //This one sets the class variable
        return new SessionData { userid = (int)dict[Keys.UserId]};
    }

    /**
     * Returns the key value pair.
     */
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> { { Keys.UserId, userid }};
    }

    
    public static class Keys
    {
        public const string UserId = "u";
    }
}