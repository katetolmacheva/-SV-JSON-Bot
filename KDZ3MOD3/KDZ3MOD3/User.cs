
namespace KDZ3MOD3
{
    /// <summary>
    /// A class for storing the user's status.
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public UserState State { get; set; }
        public List<Monuments>? Monuments { get; set; }
        public string? SelectionField { get; set; }
        public bool SortingOrder { get; set; }

        /// <summary>
        /// A constructor with parameters.
        /// </summary>
        /// <param name="id"></param>
        public User(long id) => (Id, State, Monuments) = (id, UserState.Introduction, new List<Monuments>());
        
        /// <summary>
        /// A constructor without parameters.
        /// </summary>
        public User() { }
    }
}
